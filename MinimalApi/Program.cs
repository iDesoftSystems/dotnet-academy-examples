using Microsoft.EntityFrameworkCore;
using MinimalApi;
using MinimalApi.Context;
using MinimalApi.Dto;
using MinimalApi.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MinimalApi.Handlers;

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

app.MapGet("/", () => "Minimal API")
.WithOpenApi(operation => new(operation)
{
    Deprecated = true,
    Tags = [new() { Name = "Root" }],
}); ;

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

app.Run();
