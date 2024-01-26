using Microsoft.EntityFrameworkCore;
using MinimalApi.Context;
using MinimalApi.Transactions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MinimalContext>(opt => opt.UseInMemoryDatabase("SimpleDB"));
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
});

TransactionsEndpoints.Map(app);

app.Run();
