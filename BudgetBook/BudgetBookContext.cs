using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

public class BudgetBookContext(DbContextOptions<BudgetBookContext> options)
    : IdentityDbContext<BudgetBook.Models.ApplicationUser>(options)
{
    public DbSet<BudgetBook.Models.Category> Category { get; set; } = default!;
    public DbSet<BudgetBook.Models.Transaction> Transaction { get; set; } = default!;
}