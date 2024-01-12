using Microsoft.EntityFrameworkCore;
using MinimalApi.Models;

namespace MinimalApi.Context;

class RepositoryContext(DbContextOptions<RepositoryContext> options) : DbContext(options)
{
    public DbSet<Transaction> Transactions => Set<Transaction>();
}