using Microsoft.EntityFrameworkCore;
using MinimalApi.Transactions.Models;

namespace MinimalApi.Context;

class MinimalContext(DbContextOptions<MinimalContext> options) : DbContext(options)
{
    public DbSet<Transaction> Transactions => Set<Transaction>();
}