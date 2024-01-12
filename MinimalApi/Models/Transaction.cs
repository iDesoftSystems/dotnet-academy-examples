namespace MinimalApi.Models;

public class Transaction
{
    public int Id { get; set; }
    public string? Summary { get; set; }
    public double Amount { get; set; }
    public TransactionType TransactionType { get; set; }
}
