using MinimalApi.Models;

namespace MinimalApi.Dto;

public class TransactionItemDto
{
    public int Id { get; set; }
    public string? Summary { get; set; }
    public double Amount { get; set; }
    public TransactionType TransactionTypeId { get; set; }
    public string TransactionTypeName { get; set; } = string.Empty;

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
