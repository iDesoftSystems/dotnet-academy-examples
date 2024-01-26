using MinimalApi.Transactions.Handlers;

namespace MinimalApi.Transactions;

static class TransactionsEndpoints
{
    public static void Map(WebApplication app)
    {
        var transactionsRoutes = app.MapGroup("/transactions");

        transactionsRoutes.MapGet("/", TransactionHandler.GetAllTransactions);

        transactionsRoutes.MapGet("/income", TransactionHandler.GetIncomeTransactions)
            .WithOpenApi(operation => new(operation)
            {
                Summary = "Get only income transactions",
                Description = "Full description about income transactions",
            });

        transactionsRoutes.MapGet("/outcome", TransactionHandler.GetOutcomeTransactions)
            .WithOpenApi();

        transactionsRoutes.MapGet("/{id}", TransactionHandler.GetTransaction)
            .WithOpenApi(generatedOperation =>
            {
                var parameter = generatedOperation.Parameters[0];
                parameter.Description = "The ID associated with created transaction";
                return generatedOperation;
            });

        transactionsRoutes.MapPost("/", TransactionHandler.CreateTransaction)
            .WithOpenApi();

        transactionsRoutes.MapPut("/{id}", TransactionHandler.UpdateTransaction)
            .WithOpenApi();

        transactionsRoutes.MapDelete("/{id}", TransactionHandler.DeleteTransaction)
            .WithOpenApi();
    }
}
