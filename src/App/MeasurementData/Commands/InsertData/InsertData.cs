using App.MeasurementData.Interfaces;
using MediatR;

namespace App.MeasurementData.Commands.InsertData;

public record InsertDataCommand : IRequest
{
    public required List<(int measId, int timestamp, float value)> Samples { get; init; }
}

public class InsertDataCommandHandler(IMeasDataStore measDataStore) : IRequestHandler<InsertDataCommand>
{
    public async Task Handle(InsertDataCommand request, CancellationToken cancellationToken)
    {
        await measDataStore.InsertSamples(request.Samples);
    }
}
