namespace HelloBlazorWasm.Models;

public class Transaction
{
    public string? Id { get; set; }
    public string Description = string.Empty;
    public decimal Amount = 0;
    public DateTime CreatedAt;
    public TransactionType Type = TransactionType.None;
}

public enum TransactionType { None, Income, Outcome }