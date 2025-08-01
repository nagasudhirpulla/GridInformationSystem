using App.Common.Behaviours;
using App.Common.Interfaces;
using App.Owners.Utils;
using Core.Entities.Elements;
using Core.Entities;
using Core.Enums;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace App.Transformers.Commands.CreateTransformer;

[Transactional(IsolationLevel = System.Data.IsolationLevel.Serializable)]
public record CreateTransformerCommand : IRequest<int>
{
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

public class CreateTransformerCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateTransformerCommand, int>
{
    public async Task<int> Handle(CreateTransformerCommand request, CancellationToken cancellationToken)
    {
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

        // derive owner names cache
        List<Owner> owners = await OwnerUtils.GetOwnersFromIdsAsync(request.OwnerIds, context, cancellationToken);
        string ownersNames = OwnerUtils.DeriveOwnersCache(owners);

        // insert transformer to db
        var entity = new Transformer()
        {
            Name = transName,
            VoltageLevelCache = Utils.DeriveTransformerVoltageLevel.Execute(hvLvLevels.ElementAt(0), hvLvLevels.ElementAt(1)),
            RegionCache = sub1.RegionCache,
            Substation1Id = request.Substation1Id,
            Substation2Id = request.Substation1Id,
            OwnerNamesCache = ownersNames,
            ElementNumber = request.ElementNumber,
            CommissioningDate = request.CommissioningDate,
            DeCommissioningDate = request.DeCommissioningDate,
            CommercialOperationDate = request.CommercialOperationDate,
            IsImportantGridElement = request.IsImportantGridElement,
            MvaCapacity = request.MvaCapacity,
            Make = request.Make
        };

        context.Transformers.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        // create ElementOwners entries
        await OwnerUtils.InsertElementOwnersAsync(entity.Id, owners.Select(o => o.Id), context, cancellationToken);

        return entity.Id;
    }
}
