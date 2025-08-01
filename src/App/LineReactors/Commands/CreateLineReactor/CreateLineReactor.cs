using App.Common.Behaviours;
using App.Common.Interfaces;
using App.Owners.Utils;
using Core.Entities.Elements;
using Core.Entities;
using FluentValidation.Results;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace App.LineReactors.Commands.CreateLineReactor;

[Transactional(IsolationLevel = System.Data.IsolationLevel.Serializable)]
public record CreateLineReactorCommand : IRequest<int>
{
    public required string OwnerIds { get; set; }
    public required string ElementNumber { get; set; }
    public DateTime CommissioningDate { get; set; }
    public DateTime? DeCommissioningDate { get; set; }
    public DateTime CommercialOperationDate { get; set; }
    public bool IsImportantGridElement { get; set; } = false;
    public int SubstationId { get; set; }
    public int LineId { get; set; }
    public double MvarCapacity { get; set; }
    public bool IsConvertible { get; set; }
    public bool IsSwitchable { get; set; }
}

public class CreateLineReactorCommandHandler(IApplicationDbContext context) : IRequestHandler<CreateLineReactorCommand, int>
{
    public async Task<int> Handle(CreateLineReactorCommand request, CancellationToken cancellationToken)
    {
        Line line = await context.Lines
                .AsNoTracking()
                .Include(x => x.VoltageLevelCache)
                .Include(x => x.Substation1)
                .FirstOrDefaultAsync(e => e.Id == request.SubstationId, cancellationToken: cancellationToken)
                ?? throw new Common.Exceptions.ValidationException([new ValidationFailure() {
                                                                                    ErrorMessage = "Line is not present in database"
                                                                                }]);
        // derive owner names cache
        List<Owner> owners = await OwnerUtils.GetOwnersFromIdsAsync(request.OwnerIds, context, cancellationToken);
        string ownersNames = OwnerUtils.DeriveOwnersCache(owners);

        string lrName = Utils.DeriveLineReactorName.Execute(line.Name, line.Substation1.Name, request.ElementNumber, context, cancellationToken);

        // insert element to db
        var entity = new LineReactor()
        {
            Name = lrName,
            VoltageLevelCache = line.VoltageLevelCache,
            RegionCache = line.RegionCache,
            Substation1Id = request.SubstationId,
            OwnerNamesCache = ownersNames,
            ElementNumber = request.ElementNumber,
            CommissioningDate = request.CommissioningDate,
            DeCommissioningDate = request.DeCommissioningDate,
            CommercialOperationDate = request.CommercialOperationDate,
            IsImportantGridElement = request.IsImportantGridElement,
            LineId = request.LineId,
            MvarCapacity = request.MvarCapacity,
            IsConvertible = request.IsConvertible,
            IsSwitchable = request.IsSwitchable
        };

        context.LineReactors.Add(entity);

        await context.SaveChangesAsync(cancellationToken);

        // create ElementOwners entries
        await OwnerUtils.InsertElementOwnersAsync(entity.Id, owners.Select(o => o.Id), context, cancellationToken);

        return entity.Id;
    }
}