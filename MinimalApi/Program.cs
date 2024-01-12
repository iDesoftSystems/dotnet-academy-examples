using Microsoft.EntityFrameworkCore;
using MinimalApi.Context;
using MinimalApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<RepositoryContext>(opt => opt.UseInMemoryDatabase("SimpleDB"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("/transactions", async (RepositoryContext context) => await context.Transactions.ToListAsync());

app.MapGet("/transactions/income", async (RepositoryContext context) => await context.Transactions.Where(t => t.TransactionType == MinimalApi.TransactionType.Income).ToListAsync());

app.MapGet("/transactions/outcome", async (RepositoryContext context) => await context.Transactions.Where(t => t.TransactionType == MinimalApi.TransactionType.Outcome).ToListAsync());

app.MapGet("/transactions/{id}", async (int id, RepositoryContext context) =>
{
    var transaction = await context.Transactions.FindAsync(id);
    if (transaction is not null)
    {
        return Results.Ok(transaction);
    }

    return Results.NotFound();
});

app.MapPost("/transactions", async (Transaction transaction, RepositoryContext context) =>
{
    context.Transactions.Add(transaction);
    await context.SaveChangesAsync();

    var location = $"/transactions/{transaction.Id}";
    return Results.Created(location, transaction);
});

app.MapPut("/transactions/{id}", async (int id, Transaction updateTransaction, RepositoryContext context) =>
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

app.MapDelete("/transactions/{id}", async (int id, RepositoryContext context) =>
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
