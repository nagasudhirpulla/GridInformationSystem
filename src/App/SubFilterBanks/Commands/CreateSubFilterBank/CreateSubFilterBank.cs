using App.Common.Behaviours;
using App.Common.Interfaces;
using App.Owners.Utils;
using Core.Entities.Elements;
using Core.Entities;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.SubFilterBanks.Commands.CreateSubFilterBank;

[Transactional(IsolationLevel = System.Data.IsolationLevel.Serializable)]
public record CreateSubFilterBankCommand : IRequest<int>
{
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

public class CreateSubFilterBankCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateSubFilterBankCommand, int>
{
    public async Task<int> Handle(CreateSubFilterBankCommand request, CancellationToken cancellationToken)
    {
        // query filter bank and substation from db
        var filterBank = await context.FilterBanks
            .Include(s => s.Substation1)
            .FirstOrDefaultAsync(s => s.Id == request.FilterBankId, cancellationToken) ?? throw new Common.Exceptions.ValidationException([new ValidationFailure() {
                                                                                    ErrorMessage = "FilterBank Id is not present in database"
                                                                                }]);

        // derive element elName 
        string elName = Utils.DeriveSubFilterBankName.Execute(filterBank.Substation1.NameCache, filterBank.ElementNumber, request.SubFilterTag);

        // derive voltage level, region from substation
        string voltLvl = filterBank.VoltageLevelCache;
        string region = filterBank.Substation1.RegionCache;

        // derive owner names cache
        List<Owner> owners = await OwnerUtils.GetOwnersFromIdsAsync(request.OwnerIds, context, cancellationToken);
        string ownersNames = OwnerUtils.DeriveOwnersCache(owners);

        // insert element to db
        var entity = new SubFilterBank()
        {
            ElementNameCache = elName,
            VoltageLevelCache = voltLvl,
            RegionCache = region,
            OwnerNamesCache = ownersNames,
            Substation1Id = filterBank.Substation1.Id,
            ElementNumber = request.SubFilterTag,
            CommissioningDate = request.CommissioningDate,
            DeCommissioningDate = request.DeCommissioningDate,
            CommercialOperationDate = request.CommercialOperationDate,
            IsImportantGridElement = request.IsImportantGridElement,
            FilterBankId = request.FilterBankId,
            Mvar = request.Mvar,
            IsSwitchable = request.IsSwitchable
        };

        context.SubFilterBanks.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        // create ElementOwners entries
        await OwnerUtils.InsertElementOwnersAsync(entity.Id, owners.Select(o => o.Id), context, cancellationToken);

        return entity.Id;
    }
}
