namespace Overview.Models;

public class Transaction(Wallet from, Wallet to)
{
    public readonly string Id = Guid.NewGuid().ToString();
    public readonly Wallet From = from;
    public readonly Wallet To = to;
}
