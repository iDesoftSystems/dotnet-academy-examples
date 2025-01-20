namespace Overview.Models;

public class Wallet(Owner owner)
{
    public readonly string Id = Guid.NewGuid().ToString();
    public readonly Owner Owner = owner;
}
