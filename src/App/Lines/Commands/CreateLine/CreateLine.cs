using App.Common.Behaviours;
using App.Common.Interfaces;
using App.Owners.Utils;
using Core.Entities;
using Core.Entities.Elements;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.Lines.Commands.CreateLine;

[Transactional(IsolationLevel = System.Data.IsolationLevel.Serializable)]
public record CreateLineCommand : IRequest<int>
{
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
    public bool IsAutoReclosurePresent { get; set; }
}

public class CreateLineCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateLineCommand, int>
{
    public async Task<int> Handle(CreateLineCommand request, CancellationToken cancellationToken)
    {
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
        (string lineName, string voltLevel, string region) = await Utils.DeriveLineName.ExecuteAsync(request.Bus1Id, request.Bus2Id, request.ElementNumber, context, cancellationToken);

        // derive owner names cache
        List<Owner> owners = await OwnerUtils.GetOwnersFromIdsAsync(request.OwnerIds, context, cancellationToken);
        string ownersNames = OwnerUtils.DeriveOwnersCache(owners);

        // insert element to db
        var entity = new Line()
        {
            Name = lineName,
            VoltageLevelCache = voltLevel,
            RegionCache = region,
            Substation1Id = bus1.Substation1Id,
            Substation2Id = bus2.Substation1Id,
            OwnerNamesCache = ownersNames,
            ElementNumber = request.ElementNumber,
            CommissioningDate = request.CommissioningDate,
            DeCommissioningDate = request.DeCommissioningDate,
            CommercialOperationDate = request.CommercialOperationDate,
            IsImportantGridElement = request.IsImportantGridElement,
            Bus1Id = request.Bus1Id,
            Bus2Id = request.Bus2Id,
            Length = request.Length,
            ConductorType = request.ConductorType,
            IsAutoReclosurePresent = request.IsAutoReclosurePresent
        };

        context.Lines.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        // create ElementOwners entries
        await OwnerUtils.InsertElementOwnersAsync(entity.Id, owners.Select(o => o.Id), context, cancellationToken);

        return entity.Id;
    }
}