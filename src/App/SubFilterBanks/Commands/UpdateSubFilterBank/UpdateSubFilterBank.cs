using App.Common.Behaviours;
using App.Common.Interfaces;
using App.Owners.Utils;
using Ardalis.GuardClauses;
using Core.Entities;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.SubFilterBanks.Commands.UpdateSubFilterBank;

[Transactional(IsolationLevel = System.Data.IsolationLevel.Serializable)]
public record UpdateSubFilterBankCommand : IRequest
{
    public int Id { get; set; }
    public int FilterBankId { get; set; }
    public required string OwnerIds { get; set; }
    public DateTime CommissioningDate { get; set; }
    public DateTime? DeCommissioningDate { get; set; }
    public DateTime CommercialOperationDate { get; set; }
    public bool IsImportantGridElement { get; set; } = false;
    public required string SubFilterTag { get; set; }
    public double Mvar { get; set; }
    public bool IsSwitchable { get; set; }
}

public class UpdateSubFilterBankCommandHandler(IApplicationDbContext context) : IRequestHandler<UpdateSubFilterBankCommand>
{
    public async Task Handle(UpdateSubFilterBankCommand request, CancellationToken cancellationToken)
    {
        var entity = await context.SubFilterBanks
            .FindAsync([request.Id], cancellationToken);

        Guard.Against.NotFound(request.Id, entity);

        // query filter bank and substation from db
        var filterBank = await context.FilterBanks
            .Include(s => s.Substation1)
            .FirstOrDefaultAsync(s => s.Id == request.FilterBankId, cancellationToken) ?? throw new Common.Exceptions.ValidationException([new ValidationFailure() {
                                                                                    ErrorMessage = "FilterBank Id is not present in database"
                                                                                }]);

        // derive element elName 
        string elName = Utils.DeriveSubFilterBankName.Execute(filterBank.Substation1.Name, filterBank.ElementNumber, request.SubFilterTag);

        // derive voltage level, region from substation
        string voltLvl = filterBank.VoltageLevelCache;
        string region = filterBank.Substation1.RegionCache;

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
        entity.Name = elName;
        entity.VoltageLevelCache = voltLvl;
        entity.RegionCache = region;
        entity.Substation1Id = filterBank.Substation1Id;
        entity.ElementNumber = request.SubFilterTag;
        entity.CommissioningDate = request.CommissioningDate;
        entity.DeCommissioningDate = request.DeCommissioningDate;
        entity.CommercialOperationDate = request.CommercialOperationDate;
        entity.IsImportantGridElement = request.IsImportantGridElement;
        entity.FilterBankId = request.FilterBankId;
        entity.Mvar = request.Mvar;
        request.IsSwitchable = request.IsSwitchable;

        await context.SaveChangesAsync(cancellationToken);
    }
}
