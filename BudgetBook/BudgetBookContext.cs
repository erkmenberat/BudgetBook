using Microsoft.EntityFrameworkCore;

public class BudgetBookContext(DbContextOptions<BudgetBookContext> options) : DbContext(options)
{
    public DbSet<BudgetBook.Models.Category> Category { get; set; } = default!;
    public DbSet<BudgetBook.Models.User> User { get; set; } = default!;
    public DbSet<BudgetBook.Models.Transaction> Transaction { get; set; } = default!;
}
