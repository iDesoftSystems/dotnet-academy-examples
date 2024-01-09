using Microsoft.EntityFrameworkCore;

class ProgramDBContext : DbContext
{
    public ProgramDBContext(DbContextOptions<ProgramDBContext> options) : base(options) { }

    public DbSet<CashRegister> CashRegisters => Set<CashRegister>();
}