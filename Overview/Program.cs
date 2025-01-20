using Overview.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

IList<Transaction> transactions = new List<Transaction>();

Owner mainOwner = new Owner("Jhon", "Doe");
Owner anotherOwner = new Owner("Jhenny", "Doe");
Owner guestOwner = new Owner("Dan", "Doe");

Wallet mainWallet = new Wallet(mainOwner);
Wallet anotherWallet = new Wallet(anotherOwner);

transactions.Add(new Transaction(
    mainWallet,
    anotherWallet
));
transactions.Add(new Transaction(
    mainWallet,
    anotherWallet
));

foreach (var transaction in transactions)
{
    Console.WriteLine($"transaction Id: {transaction.Id}");

    Console.WriteLine($"\t\tFrom Id: {transaction.From.Id}");
    Console.WriteLine($"\t\t\tOwner: {transaction.From.Owner.FullName}");

    Console.WriteLine($"\t\tTo Id: {transaction.To.Id}");
    Console.WriteLine($"\t\t\tOwner: {transaction.To.Owner.FullName}");
}

IEnumerable<Transaction> mainOwnerTransactions = from transaction in transactions
                                                 where transaction.From.Owner.Id.Equals(guestOwner.Id)
                                                 orderby transaction.Id descending
                                                 select transaction;

Console.WriteLine("Main owner transactions");

foreach (var transaction in mainOwnerTransactions)
{
    Console.WriteLine($"transaction Id: {transaction.Id}");

    Console.WriteLine($"\t\tFrom Id: {transaction.From.Id}");
    Console.WriteLine($"\t\t\tOwner: {transaction.From.Owner.FullName}");

    Console.WriteLine($"\t\tTo Id: {transaction.To.Id}");
    Console.WriteLine($"\t\t\tOwner: {transaction.To.Owner.FullName}");
}

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
