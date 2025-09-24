using App.MeasurementData.Interfaces;
using MediatR;

namespace App.MeasurementData.Commands.InsertData;

public record InsertDataCommand : IRequest
{
    public required List<InsertDataRecord> Samples { get; init; }
}

public class InsertDataRecord
{
    public int MeasId { get; set; }
    public int Timestamp { get; set; }
    public float Value { get; set; }
}

public class InsertDataCommandHandler(IMeasDataStore measDataStore) : IRequestHandler<InsertDataCommand>
{
    public async Task Handle(InsertDataCommand request, CancellationToken cancellationToken)
    {
        await measDataStore.InsertSamples(request.Samples);
    }
}
