using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.OpenApi;
using MinimalApi;
using MinimalApi.Context;
using MinimalApi.Dto;
using MinimalApi.Models;
using Microsoft.AspNetCore.Http.HttpResults;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<RepositoryContext>(opt => opt.UseInMemoryDatabase("SimpleDB"));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "Minimal API").WithOpenApi(operation => new(operation)
{
    Deprecated = true,
    Tags = [new() { Name = "Root" }],
}); ;

var transactionsRoutes = app.MapGroup("/transactions");

transactionsRoutes.MapGet("/", GetAllTransactions);

transactionsRoutes.MapGet("/income", GetIncomeTransactions)
    .WithOpenApi(operation => new(operation)
    {
        Summary = "Get only income transactions",
        Description = "Full description about income transactions",
    });

transactionsRoutes.MapGet("/outcome", GetOutcomeTransactions)
    .WithOpenApi();

transactionsRoutes.MapGet("/{id}", GetTransaction)
    .WithOpenApi(generatedOperation =>
    {
        var parameter = generatedOperation.Parameters[0];
        parameter.Description = "The ID associated with created transaction";
        return generatedOperation;
    }
    );

transactionsRoutes.MapPost("/", CreateTransaction)
    .WithOpenApi();

transactionsRoutes.MapPut("/{id}", UpdateTransaction)
    .WithOpenApi();

transactionsRoutes.MapDelete("/{id}", DeleteTransaction)
    .WithOpenApi();

app.Run();

static async Task<IResult> GetAllTransactions(RepositoryContext context)
{
    return TypedResults.Ok(await context.Transactions
        .Select(t => TransactionItemDto.From(t))
        .ToListAsync());
}

static async Task<IResult> GetIncomeTransactions(RepositoryContext context)
{
    return TypedResults.Ok(await context.Transactions
        .Where(t => t.TransactionType == TransactionType.Income)
        .Select(t => TransactionItemDto.From(t))
        .ToListAsync());
}

static async Task<IResult> GetOutcomeTransactions(RepositoryContext context)
{
    return TypedResults.Ok(await context.Transactions
        .Where(t => t.TransactionType == TransactionType.Outcome)
        .Select(t => TransactionItemDto.From(t))
        .ToListAsync());
}

static async Task<Results<Ok<TransactionItemDto>, NotFound>> GetTransaction(int id, RepositoryContext context)
{
    var transaction = await context.Transactions.FindAsync(id);
    if (transaction is not null)
    {
        return TypedResults.Ok(TransactionItemDto.From(transaction));
    }

    return TypedResults.NotFound();
}

static async Task<IResult> CreateTransaction(CreateTransactionDto transactionDto, RepositoryContext context)
{
    var transactionItem = new Transaction
    {
        Summary = transactionDto.Summary,
        Amount = transactionDto.Amount,
        TransactionType = transactionDto.TransactionTypeId
    };

    context.Transactions.Add(transactionItem);
    await context.SaveChangesAsync();

    var location = $"/transactions/{transactionItem.Id}";
    return TypedResults.Created(location, transactionItem);
}

static async Task<IResult> UpdateTransaction(int id, UpdateTransactionDto updateTransactionDto, RepositoryContext context)
{
    var transaction = await context.Transactions.FindAsync(id);
    if (transaction is null)
    {
        return TypedResults.NotFound();
    }

    transaction.Summary = updateTransactionDto.Summary;

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