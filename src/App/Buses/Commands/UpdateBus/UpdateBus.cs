﻿using App.Common.Behaviours;
using App.Common.Interfaces;
using App.Owners.Utils;
using Ardalis.GuardClauses;
using Core.Entities;
using Core.Enums;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.Buses.Commands.UpdateBus;

[Transactional(IsolationLevel = System.Data.IsolationLevel.Serializable)]
public record UpdateBusCommand : IRequest
{
    public int Id { get; set; }
    public int SubstationId { get; set; }
    public required string OwnerIds { get; set; }
    public required string ElementNumber { get; set; }
    public DateTime CommissioningDate { get; set; }
    public DateTime? DeCommissioningDate { get; set; }
    public DateTime CommercialOperationDate { get; set; }
    public bool IsImportantGridElement { get; set; } = false;
    public BusTypeEnum BusType { get; set; } = null!;
}

public class UpdateBusCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdateBusCommand>
{
    public async Task Handle(UpdateBusCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Buses
            .FindAsync([request.Id], cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        // query substation from db
        var substation = await context.Substations
            .Include(s => s.VoltageLevel)
            .FirstOrDefaultAsync(s => s.Id == request.SubstationId, cancellationToken) ?? throw new Common.Exceptions.ValidationException([new ValidationFailure() {
                                                                                    ErrorMessage = "Substation Id is not present in database"
                                                                                }]);

        // derive element name 
        string name = Utils.DeriveBusName.Execute(substation.Name, request.BusType, request.ElementNumber);

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
        entity.BusType = request.BusType;
        entity.Name = name;
        entity.VoltageLevelCache = voltLvl;
        entity.RegionCache = region;
        entity.Substation1Id = request.SubstationId;
        entity.ElementNumber = request.ElementNumber;
        entity.CommissioningDate = request.CommissioningDate;
        entity.DeCommissioningDate = request.DeCommissioningDate;
        entity.CommercialOperationDate = request.CommercialOperationDate;
        entity.IsImportantGridElement = request.IsImportantGridElement;

        await context.SaveChangesAsync(cancellationToken);
    }
}