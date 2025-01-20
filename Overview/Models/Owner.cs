using Overview.Abstracts;

namespace Overview.Models;

public class Owner(string name, string surname) : IFullName
{
    public readonly string Id = Guid.NewGuid().ToString();
    public readonly string Name = name;
    public readonly string Surname = surname;

    public string FullName => $"{Surname} {Name}";
}
