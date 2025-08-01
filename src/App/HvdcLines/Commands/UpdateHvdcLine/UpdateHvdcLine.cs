using App.Common.Behaviours;
using App.Common.Interfaces;
using App.Owners.Utils;
using Ardalis.GuardClauses;
using Core.Entities.Elements;
using Core.Entities;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.HvdcLines.Commands.UpdateHvdcLine;

[Transactional(IsolationLevel = System.Data.IsolationLevel.Serializable)]
public record UpdateHvdcLineCommand : IRequest
{
    public int Id { get; set; }
    public int Bus1Id { get; set; }
    public int Bus2Id { get; set; }
    public required string OwnerIds { get; set; }
    public required string ElementNumber { get; set; }
    public DateTime CommissioningDate { get; set; }
    public DateTime? DeCommissioningDate { get; set; }
    public DateTime CommercialOperationDate { get; set; }
    public bool IsImportantGridElement { get; set; } = false;
    public double Length { get; set; }
    public required string ConductorType { get; set; }
    public bool IsSpsPresent { get; set; }
}

public class UpdateHvdcLineCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdateHvdcLineCommand>
{
    public async Task Handle(UpdateHvdcLineCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.HvdcLines
            .FindAsync([request.Id], cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        Bus bus1 = await context.Buses
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == request.Bus1Id, cancellationToken: cancellationToken)
                ?? throw new Common.Exceptions.ValidationException([new ValidationFailure() {
                                                                                    ErrorMessage = "Bus Id is not present in database"
                                                                                }]);
        Bus bus2 = await context.Buses
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Id == request.Bus2Id, cancellationToken: cancellationToken)
                ?? throw new Common.Exceptions.ValidationException([new ValidationFailure() {
                                                                                    ErrorMessage = "Bus Id is not present in database"
                                                                                }]);
        (string lineName, string voltLevel, string region) = await Utils.DeriveHvdcLineName.ExecuteAsync(request.Bus1Id, request.Bus2Id, request.ElementNumber, context, cancellationToken);

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
        entity.Name = lineName;
        entity.VoltageLevelCache = voltLevel;
        entity.RegionCache = region;
        entity.Substation1Id = bus1.Substation1Id;
        entity.Substation2Id = bus2.Substation1Id;
        entity.ElementNumber = request.ElementNumber;
        entity.CommissioningDate = request.CommissioningDate;
        entity.DeCommissioningDate = request.DeCommissioningDate;
        entity.CommercialOperationDate = request.CommercialOperationDate;
        entity.IsImportantGridElement = request.IsImportantGridElement;
        entity.Bus1Id = request.Bus1Id;
        entity.Bus2Id = request.Bus2Id;
        entity.Length = request.Length;
        entity.ConductorType = request.ConductorType;
        entity.IsSpsPresent = request.IsSpsPresent;

        await context.SaveChangesAsync(cancellationToken);
    }
}