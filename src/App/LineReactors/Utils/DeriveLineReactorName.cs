using App.Common.Interfaces;

namespace App.LineReactors.Utils;

public static class DeriveLineReactorName
{
    public static string Execute(string lineName, string substationName, string elementNumber, IApplicationDbContext context, CancellationToken cancellationToken)
    {
        return $"{lineName} LR-{elementNumber} at {substationName}";
    }
}