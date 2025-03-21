using App.Buses.Commands.UpdateBus;
using App.Common.Behaviours;
using App.Common.Interfaces;
using App.Owners.Utils;
using Ardalis.GuardClauses;
using Core.Entities;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.BusReactors.Commands.UpdateBusReactor;

[Transactional(IsolationLevel = System.Data.IsolationLevel.Serializable)]
public record UpdateBusReactorCommand : IRequest
{
    public int Id { get; set; }
    public required string OwnerIds { get; set; }
    public required string ElementNumber { get; set; }
    public DateTime CommissioningDate { get; set; }
    public DateTime? DeCommissioningDate { get; set; }
    public DateTime CommercialOperationDate { get; set; }
    public bool IsImportantGridElement { get; set; } = false;
    public int BusId { get; set; }
    public double MvarCapacity { get; set; }
}

public class UpdateBusReactorCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdateBusReactorCommand>
{
    public async Task Handle(UpdateBusReactorCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.BusReactors
            .FindAsync([request.Id], cancellationToken);

        Guard.Against.NotFound(request.Id, entity);


        // query bus from db
        var bus = await context.Buses
            .FirstOrDefaultAsync(s => s.Id == request.BusId, cancellationToken) ?? throw new Common.Exceptions.ValidationException([new ValidationFailure() {
                                                                            ErrorMessage = "Bus Id is not present in database"
                                                                        }]);

        // query substation from db
        var substation = await context.Substations
            .Include(s => s.VoltageLevel)
            .FirstOrDefaultAsync(s => s.Id == bus.Substation1Id, cancellationToken) ?? throw new Common.Exceptions.ValidationException([new ValidationFailure() {
                                                                                    ErrorMessage = "Substation Id is not present in database"
                                                                                }]);

        // derive element name 
        string name = Utils.DeriveBusReactorName.Execute(substation.NameCache, request.ElementNumber);

        // derive voltage level, region from substation
        string voltLvl = substation.VoltageLevel.Level;
        string region = substation.RegionCache;

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
        entity.ElementNameCache = name;
        entity.VoltageLevelCache = voltLvl;
        entity.RegionCache = region;
        entity.Substation1Id = bus.Substation1Id;
        entity.ElementNumber = request.ElementNumber;
        entity.CommissioningDate = request.CommissioningDate;
        entity.DeCommissioningDate = request.DeCommissioningDate;
        entity.CommercialOperationDate = request.CommercialOperationDate;
        entity.IsImportantGridElement = request.IsImportantGridElement;
        entity.BusId = request.BusId;
        entity.MvarCapacity = request.MvarCapacity;

        await context.SaveChangesAsync(cancellationToken);
    }
}