using HelloBlazorWasm.Models;

namespace HelloBlazorWasm.Services;

public class TransactionService
{
    public readonly List<Transaction> Transactions = [];

    public void AddTransaction(Transaction transaction)
    {
        Transactions.Add(transaction);
    }
}