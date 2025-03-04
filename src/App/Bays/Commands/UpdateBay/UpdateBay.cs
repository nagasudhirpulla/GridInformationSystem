using App.Common.Behaviours;
using App.Common.Interfaces;
using App.Owners.Utils;
using Core.Entities.Elements;
using Core.Entities;
using Core.Enums;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Ardalis.GuardClauses;

namespace App.Bays.Commands.UpdateBay;

[Transactional(IsolationLevel = System.Data.IsolationLevel.Serializable)]
public record UpdateBayCommand : IRequest
{
    public int Id { get; set; }
    public int Element1Id { get; set; }
    public int Element2Id { get; set; }
    public required string OwnerIds { get; set; }
    public DateTime CommissioningDate { get; set; }
    public DateTime? DeCommissioningDate { get; set; }
    public DateTime CommercialOperationDate { get; set; }
    public bool IsImportantGridElement { get; set; } = false;
    public bool IsFuture { get; set; } = false;
    public BayTypeEnum BayType { get; set; } = null!;
}

public class UpdateBayCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdateBayCommand>
{
    public async Task Handle(UpdateBayCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.Bays
            .FindAsync([request.Id], cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        // query the elements names
        Element el1 = await context.Elements.FirstOrDefaultAsync(e => e.Id == request.Element1Id, cancellationToken) ?? throw new Common.Exceptions.ValidationException([new ValidationFailure() {
                                                                                    ErrorMessage = "Element1 Id is not present in database"
                                                                                }]);
        Element el2 = await context.Elements.FirstOrDefaultAsync(e => e.Id == request.Element2Id, cancellationToken) ?? throw new Common.Exceptions.ValidationException([new ValidationFailure() {
                                                                                    ErrorMessage = "Element1 Id is not present in database"
                                                                                }]);

        int substationId = el1.Substation1Id;

        // query substation from db
        Substation substation = await context.Substations
            .Include(s => s.VoltageLevel)
            .FirstOrDefaultAsync(s => s.Id == substationId, cancellationToken) ?? throw new Common.Exceptions.ValidationException([new ValidationFailure() {
                                                                                    ErrorMessage = "Substation Id is not present in database"
                                                                                }]);
        // derive element name 
        string name = Utils.DeriveBayName.Execute(el1.ElementNameCache, el2.ElementNameCache, request.BayType);

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


        entity.Element1Id = request.Element1Id;
        entity.Element2Id = request.Element2Id;
        entity.BayType = request.BayType;
        entity.IsFuture = request.IsFuture;
        entity.ElementNameCache = name;
        entity.VoltageLevelCache = voltLvl;
        entity.RegionCache = region;
        entity.CommissioningDate = request.CommissioningDate;
        entity.DeCommissioningDate = request.DeCommissioningDate;
        entity.CommercialOperationDate = request.CommercialOperationDate;
        entity.IsImportantGridElement = request.IsImportantGridElement;

        await context.SaveChangesAsync(cancellationToken);
    }
}