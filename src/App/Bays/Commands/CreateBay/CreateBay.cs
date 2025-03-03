using App.Common.Behaviours;
using App.Common.Interfaces;
using Core.Entities.Elements;
using Core.Entities;
using Core.Enums;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;
using App.Owners.Utils;

namespace App.Bays.Commands.CreateBay;

/*
ConnectingElement1 and ConnectingElement2 should be present in the same substation
For a Tie Bay Element1 and Element2 would be Non-Bus
For a Main Bay exactly one of the element1 or element2 would be bus
For a Bus Coupler, Bus Sectionalizer, TBC Bay elment1 and element2 both are bus type
fix element1 as bus for main bay
TODO
spare bay column may be added
 */
[Transactional(IsolationLevel = System.Data.IsolationLevel.Serializable)]
public class CreateBayCommand : IRequest<int>
{
    public int Element1Id { get; set; }
    public int Element2Id { get; set; }
    public required string OwnerIds { get; set; }
    public DateTime CommissioningDate { get; set; }
    public DateTime? DeCommissioningDate { get; set; }
    public DateTime CommercialOperationDate { get; set; }
    public bool IsImportantGridElement { get; set; } = false;
    public BayTypeEnum BayType { get; set; } = null!;
    public bool IsFuture { get; set; } = false;
}

public class CreateBayCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateBayCommand, int>
{
    public async Task<int> Handle(CreateBayCommand request, CancellationToken cancellationToken)
    {
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

        // derive owner names cache
        List<Owner> owners = await OwnerUtils.GetOwnersFromIdsAsync(request.OwnerIds, context, cancellationToken);
        string ownersNames = OwnerUtils.DeriveOwnersCache(owners);

        //// insert bay to db
        var entity = new Bay()
        {
            Element1Id = request.Element1Id,
            Element2Id = request.Element2Id,
            BayType = request.BayType,
            IsFuture = request.IsFuture,
            ElementNameCache = name,
            VoltageLevelCache = voltLvl,
            RegionCache = region,
            Substation1Id = substationId,
            OwnerNamesCache = ownersNames,
            ElementNumber = "1",
            CommissioningDate = request.CommissioningDate,
            DeCommissioningDate = request.DeCommissioningDate,
            CommercialOperationDate = request.CommercialOperationDate,
            IsImportantGridElement = request.IsImportantGridElement
        };

        context.Bays.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        // create ElementOwners entries
        await OwnerUtils.InsertElementOwnersAsync(entity.Id, owners.Select(o => o.Id), context, cancellationToken);

        return entity.Id;
    }
}
