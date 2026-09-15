using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
//using BudgetBook.Data;
using System;
using System.Linq;

namespace BudgetBook.Models;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new BudgetBookContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<BudgetBookContext>>()))
        {
            // Look for any movies.
            if (context.Transaction.Any() || context.User.Any() ||context.Category.Any())
            {
                return;   // DB has been seeded
            }
        }
    }
}