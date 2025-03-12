using App.Common.Behaviours;
using App.Common.Interfaces;
using App.Owners.Utils;
using Ardalis.GuardClauses;
using Core.Entities;
using Core.Enums;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace App.Transformers.Commands.UpdateTransformer;

[Transactional(IsolationLevel = System.Data.IsolationLevel.Serializable)]
public record UpdateTransformerCommand : IRequest
{
    public int Id { get; set; }
    public int Substation1Id { get; set; }
    public int Substation2Id { get; set; }
    public required string OwnerIds { get; set; }
    public required string ElementNumber { get; set; }
    public DateTime CommissioningDate { get; set; }
    public DateTime? DeCommissioningDate { get; set; }
    public DateTime CommercialOperationDate { get; set; }
    public bool IsImportantGridElement { get; set; } = false;
    public TransformerTypeEnum TransformerType { get; set; } = null!;
    public double MvaCapacity { get; set; }
    public string? Make { get; set; }
}

public class UpdateTransformerCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdateTransformerCommand>
{
    public async Task Handle(UpdateTransformerCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Transformers
            .FindAsync([request.Id], cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        Substation sub1 = await context.Substations
                .AsNoTracking()
                .Include(s => s.VoltageLevel)
                .FirstOrDefaultAsync(e => e.Id == request.Substation1Id, cancellationToken: cancellationToken)
                ?? throw new Common.Exceptions.ValidationException([new ValidationFailure() {
                                                                                    ErrorMessage = "Substation 1 Id is not present in database"
                                                                                }]);
        Substation sub2 = await context.Substations
                .AsNoTracking()
                .Include(s => s.VoltageLevel)
                .FirstOrDefaultAsync(e => e.Id == request.Substation2Id, cancellationToken: cancellationToken)
                ?? throw new Common.Exceptions.ValidationException([new ValidationFailure() {
                                                                                    ErrorMessage = "Substation 1 Id is not present in database"
                                                                                }]);
        IOrderedEnumerable<string> hvLvLevels = new[] { sub1.VoltageLevel.Level, sub2.VoltageLevel.Level }.OrderByDescending(i => int.Parse(Regex.Match(i, @"\d+").Value));
        string transName = Utils.DeriveTransformerName.Execute(sub1.Location.Name, hvLvLevels.ElementAt(0), hvLvLevels.ElementAt(1), request.ElementNumber, request.TransformerType);

        // update ownerIds if required
        var newOwnerIds = request.OwnerIds.Split(',').Select(int.Parse).ToList();
        var numOwnerChanges = await OwnerUtils.UpdateElementOwnersAsync(request.Id, newOwnerIds, context, cancellationToken);
        if (numOwnerChanges > 0)
        {
            // update ownerNames cache
            List<Owner> newOwners = await context.Owners.Where(o => newOwnerIds.Contains(o.Id)).ToListAsync(cancellationToken: cancellationToken);
            var ownersCache = OwnerUtils.DeriveOwnersCache(newOwners);
            entity.OwnerNamesCache = ownersCache;
        }

        // update entity attributes
        entity.ElementNameCache = transName;
        entity.VoltageLevelCache = Utils.DeriveTransformerVoltageLevel.Execute(hvLvLevels.ElementAt(0), hvLvLevels.ElementAt(1));
        entity.RegionCache = sub1.RegionCache;
        entity.Substation1Id = request.Substation1Id;
        entity.Substation2Id = request.Substation1Id;
        entity.ElementNumber = request.ElementNumber;
        entity.CommissioningDate = request.CommissioningDate;
        entity.DeCommissioningDate = request.DeCommissioningDate;
        entity.CommercialOperationDate = request.CommercialOperationDate;
        entity.IsImportantGridElement = request.IsImportantGridElement;
        entity.MvaCapacity = request.MvaCapacity;
        entity.Make = request.Make;

        await context.SaveChangesAsync(cancellationToken);
    }
}
