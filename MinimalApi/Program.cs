using Microsoft.EntityFrameworkCore;
using MinimalApi.Context;
using MinimalApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<RepositoryContext>(opt => opt.UseInMemoryDatabase("SimpleDB"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

app.MapGet("/", () => "Minimal API");

var transactionsRoutes = app.MapGroup("/transactions");

transactionsRoutes.MapGet("/", GetAllTransactions);

transactionsRoutes.MapGet("/income", GetIncomeTransactions);

transactionsRoutes.MapGet("/outcome", GetOutcomeTransactions);

transactionsRoutes.MapGet("/{id}", GetTransaction);

transactionsRoutes.MapPost("/", CreateTransaction);

transactionsRoutes.MapPut("/{id}", UpdateTransaction);

transactionsRoutes.MapDelete("/{id}", DeleteTransaction);

app.Run();

static async Task<IResult> GetAllTransactions(RepositoryContext context)
{
    return TypedResults.Ok(await context.Transactions.ToListAsync());
}

static async Task<IResult> GetIncomeTransactions(RepositoryContext context)
{
    return TypedResults.Ok(await context.Transactions.Where(t => t.TransactionType == MinimalApi.TransactionType.Income)
        .ToListAsync());
}

static async Task<IResult> GetOutcomeTransactions(RepositoryContext context)
{
    return TypedResults.Ok(await context.Transactions
        .Where(t => t.TransactionType == MinimalApi.TransactionType.Outcome).ToListAsync());
}

static async Task<IResult> GetTransaction(int id, RepositoryContext context)
{
    var transaction = await context.Transactions.FindAsync(id);
    if (transaction is not null)
    {
        return TypedResults.Ok(transaction);
    }

    return TypedResults.NotFound();
}

static async Task<IResult> CreateTransaction(Transaction transaction, RepositoryContext context)
{
    context.Transactions.Add(transaction);
    await context.SaveChangesAsync();

    var location = $"/transactions/{transaction.Id}";
    return TypedResults.Created(location, transaction);
}

static async Task<IResult> UpdateTransaction(int id, Transaction updateTransaction, RepositoryContext context)
{
    var transaction = await context.Transactions.FindAsync(id);
    if (transaction is null)
    {
        return TypedResults.NotFound();
    }

    transaction.Summary = updateTransaction.Summary;
    transaction.Amount = updateTransaction.Amount;
    transaction.TransactionType = updateTransaction.TransactionType;

    await context.SaveChangesAsync();

    return TypedResults.NoContent();
}

static async Task<IResult> DeleteTransaction(int id, RepositoryContext context)
{
    if (await context.Transactions.FindAsync(id) is Transaction transaction)
    {
        context.Transactions.Remove(transaction);
        await context.SaveChangesAsync();

        return TypedResults.NoContent();
    }

    return TypedResults.NotFound();
}