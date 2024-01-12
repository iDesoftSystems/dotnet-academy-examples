namespace MinimalApi.Dto;

public class CreateTransactionDto
{
    public string? Summary { get; set; }
    public double Amount { get; set; }
    public TransactionType TransactionTypeId { get; set; }
}
