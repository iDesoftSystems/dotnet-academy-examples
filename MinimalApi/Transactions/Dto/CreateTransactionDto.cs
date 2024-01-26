namespace MinimalApi.Transactions.Dto;

public record CreateTransactionDto
{
    public string? Summary { get; init; }
    public double Amount { get; init; }
    public TransactionType TransactionTypeId { get; init; }
}
