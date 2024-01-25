using MinimalApi.Models;

namespace MinimalApi.Dto;

public record TransactionItemDto
{
    public int Id { get; init; }
    public string? Summary { get; init; }
    public double Amount { get; init; }
    public TransactionType TransactionTypeId { get; init; }
    public string TransactionTypeName { get; init; } = string.Empty;

    public static TransactionItemDto From(Transaction transaction)
    {
        return new TransactionItemDto
        {
            Id = transaction.Id,
            Summary = transaction.Summary,
            Amount = transaction.Amount,
            TransactionTypeId = transaction.TransactionType,
            TransactionTypeName = transaction.TransactionType.ToString()
        };
    }
}
