using App.Common.Behaviours;
using App.Common.Interfaces;
using App.Owners.Utils;
using Core.Entities.Elements;
using Core.Entities;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.BusReactors.Commands.CreateBusReactor;

[Transactional(IsolationLevel = System.Data.IsolationLevel.Serializable)]
public record CreateBusReactorCommand : IRequest<int>
{
    public required string OwnerIds { get; set; }
    public required string ElementNumber { get; set; }
    public DateTime CommissioningDate { get; set; }
    public DateTime? DeCommissioningDate { get; set; }
    public DateTime CommercialOperationDate { get; set; }
    public bool IsImportantGridElement { get; set; } = false;
    public int BusId { get; set; }
    public double MvarCapacity { get; set; }
}

public class CreateBusReactorCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateBusReactorCommand, int>
{
    public async Task<int> Handle(CreateBusReactorCommand request, CancellationToken cancellationToken)
    {
        // query bus from db
        var bus = await context.Buses
            .FirstOrDefaultAsync(s => s.Id == request.BusId, cancellationToken) ?? throw new Common.Exceptions.ValidationException([new ValidationFailure() {
                                                                                    ErrorMessage = "Bus Id is not present in database"
                                                                                }]);

        // query substation from db
        var substation = await context.Substations
            .Include(s => s.VoltageLevel)
            .FirstOrDefaultAsync(s => s.Id == bus.Substation1Id, cancellationToken) ?? throw new Common.Exceptions.ValidationException([new ValidationFailure() {
                                                                                    ErrorMessage = "Substation Id of bus is not present in database"
                                                                                }]);

        // derive element name 
        string name = Utils.DeriveBusReactorName.Execute(substation.Name, request.ElementNumber);

        // derive voltage level, region from substation
        string voltLvl = substation.VoltageLevel.Level;
        string region = substation.RegionCache;

        // derive owner names cache
        List<Owner> owners = await OwnerUtils.GetOwnersFromIdsAsync(request.OwnerIds, context, cancellationToken);
        string ownersNames = OwnerUtils.DeriveOwnersCache(owners);

        // insert bus to db
        var entity = new BusReactor()
        {
            Name = name,
            VoltageLevelCache = voltLvl,
            RegionCache = region,
            Substation1Id = bus.Substation1Id,
            OwnerNamesCache = ownersNames,
            ElementNumber = request.ElementNumber,
            CommissioningDate = request.CommissioningDate,
            DeCommissioningDate = request.DeCommissioningDate,
            CommercialOperationDate = request.CommercialOperationDate,
            IsImportantGridElement = request.IsImportantGridElement,
            BusId = request.BusId,
            MvarCapacity = request.MvarCapacity
        };

        context.BusReactors.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        // create ElementOwners entries
        await OwnerUtils.InsertElementOwnersAsync(entity.Id, owners.Select(o => o.Id), context, cancellationToken);

        return entity.Id;
    }
}