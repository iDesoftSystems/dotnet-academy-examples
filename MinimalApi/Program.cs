using Microsoft.EntityFrameworkCore;
using MinimalApi.Context;
using MinimalApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<RepositoryContext>(opt => opt.UseInMemoryDatabase("SimpleDB"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

var transactionsRoutes = app.MapGroup("/transactions");

transactionsRoutes.MapGet("/", async (RepositoryContext context) => await context.Transactions.ToListAsync());

transactionsRoutes.MapGet("/income", async (RepositoryContext context) => await context.Transactions.Where(t => t.TransactionType == MinimalApi.TransactionType.Income).ToListAsync());

transactionsRoutes.MapGet("/outcome", async (RepositoryContext context) => await context.Transactions.Where(t => t.TransactionType == MinimalApi.TransactionType.Outcome).ToListAsync());

transactionsRoutes.MapGet("/{id}", async (int id, RepositoryContext context) =>
{
    var transaction = await context.Transactions.FindAsync(id);
    if (transaction is not null)
    {
        return Results.Ok(transaction);
    }

    return Results.NotFound();
});

transactionsRoutes.MapPost("/", async (Transaction transaction, RepositoryContext context) =>
{
    context.Transactions.Add(transaction);
    await context.SaveChangesAsync();

    var location = $"/transactions/{transaction.Id}";
    return Results.Created(location, transaction);
});

transactionsRoutes.MapPut("/{id}", async (int id, Transaction updateTransaction, RepositoryContext context) =>
{
    var transaction = await context.Transactions.FindAsync(id);
    if (transaction is null)
    {
        return Results.NotFound();
    }

    transaction.Summary = updateTransaction.Summary;
    transaction.Amount = updateTransaction.Amount;
    transaction.TransactionType = updateTransaction.TransactionType;

    await context.SaveChangesAsync();

    return Results.NoContent();
});

transactionsRoutes.MapDelete("/{id}", async (int id, RepositoryContext context) =>
{
    if (await context.Transactions.FindAsync(id) is Transaction transaction)
    {
        context.Transactions.Remove(transaction);
        await context.SaveChangesAsync();

        return Results.NoContent();
    }

    return Results.NotFound();
});

app.Run();
