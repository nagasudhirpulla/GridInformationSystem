using App.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System.Data;
using System.Reflection;
namespace App.Common.Behaviours;

public class TransactionBehavior<TRequest, TResponse>(IApplicationDbContext dbContext,
    ILogger<TransactionBehavior<TRequest, TResponse>> logger) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestType = request.GetType();

        //var tranAttri = requestType.GetCustomAttribute(typeof(TransactionalAttribute));
        var tranAttri = request.GetType().GetCustomAttribute<TransactionalAttribute>();

        if (tranAttri is null)
            return await next();

        if (dbContext.HasActiveTransaction)
            return await next();

        var response = default(TResponse);
        var typeName = requestType.Name;

        await using var transaction = await dbContext.BeginTransactionAsync(tranAttri.IsolationLevel);

        logger.LogInformation("----- Begin transaction for {CommandName} ({@Command})", typeName, request);

        response = await next();

        logger.LogInformation("----- Commit transaction for {CommandName}", typeName);

        await dbContext.CommitTransactionAsync(transaction);

        return response;
    }
}

[AttributeUsage(AttributeTargets.Class)]
public class TransactionalAttribute : Attribute
{
    public IsolationLevel IsolationLevel { get; set; } = IsolationLevel.Serializable;

}