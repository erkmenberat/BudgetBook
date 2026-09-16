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
            if (context.Transaction.Any() ||context.Category.Any())
            {
                return;   // DB has been seeded
            }

            context.Category.AddRange(
                new Category
                {
                    Name = "Wohnen",
                    Type = "Miete",
                    IsActive = true
                },
                new Category
                {
                    Name = "Wohnen",
                    Type = "Nebenkosten",
                    IsActive = true
                },
                new Category
                {
                    Name = "Lebensmittel",
                    Type = "Supermarkt",
                    IsActive = true
                },
                new Category
                {
                    Name = "Lebensmittel",
                    Type = "Restaurant",
                    IsActive = true
                },
                new Category
                {
                    Name = "Transport",
                    Type = "Öffis",
                    IsActive = true
                },
                new Category
                {
                    Name = "Transport",
                    Type = "Sprit",
                    IsActive = true
                },
                new Category
                {
                    Name = "Freizeit",
                    Type = "Abos",
                    IsActive = true
                },
                new Category
                {
                    Name = "Freizeit",
                    Type = "Ausgehen",
                    IsActive = true
                }
            );

            context.SaveChanges();
        }
    }
}