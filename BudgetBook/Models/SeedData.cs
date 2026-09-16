using Microsoft.AspNetCore.Identity;
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
            // Kategorien nur anlegen, wenn noch keine existieren
            if (!context.Category.Any())
            {
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

            // Buchungen nur anlegen, wenn noch keine existieren
            if (context.Transaction.Any())
            {
                return;   // DB has been seeded
            }

            // Jede Buchung gehört zu einem User -> Test-User anlegen
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var testUser = userManager.FindByEmailAsync("test@example.com").GetAwaiter().GetResult();
            if (testUser == null)
            {
                testUser = new ApplicationUser
                {
                    UserName = "test@example.com",
                    Email = "test@example.com",
                    Name = "Max",
                    Surname = "Mustermann"
                };
                var result = userManager.CreateAsync(testUser, "Test123!").GetAwaiter().GetResult();
                if (!result.Succeeded)
                {
                    throw new InvalidOperationException("Test-User konnte nicht angelegt werden.");
                }
            }

            var miete = context.Category.First(c => c.Type == "Miete");
            var supermarkt = context.Category.First(c => c.Type == "Supermarkt");
            var oeffis = context.Category.First(c => c.Type == "Öffis");
            var abos = context.Category.First(c => c.Type == "Abos");
            var ausgehen = context.Category.First(c => c.Type == "Ausgehen");

            context.Transaction.AddRange(
                new Transaction
                {
                    Id = 0,
                    Type = "Ausgabe",
                    Amount = 650.00m,
                    BookingDate = new DateTime(2026, 9, 1),
                    CreatedAt = DateTime.Now,
                    Description = "Miete September",
                    CategoryId = miete.Id,
                    Category = miete,
                    UserId = testUser.Id
                },
                new Transaction
                {
                    Id = 0,
                    Type = "Ausgabe",
                    Amount = 54.30m,
                    BookingDate = new DateTime(2026, 9, 3),
                    CreatedAt = DateTime.Now,
                    Description = "Wocheneinkauf",
                    CategoryId = supermarkt.Id,
                    Category = supermarkt,
                    UserId = testUser.Id
                },
                new Transaction
                {
                    Id = 0,
                    Type = "Ausgabe",
                    Amount = 58.00m,
                    BookingDate = new DateTime(2026, 9, 5),
                    CreatedAt = DateTime.Now,
                    Description = "Deutschlandticket",
                    CategoryId = oeffis.Id,
                    Category = oeffis,
                    UserId = testUser.Id
                },
                new Transaction
                {
                    Id = 0,
                    Type = "Ausgabe",
                    Amount = 13.99m,
                    BookingDate = new DateTime(2026, 9, 10),
                    CreatedAt = DateTime.Now,
                    Description = "Netflix",
                    CategoryId = abos.Id,
                    Category = abos,
                    UserId = testUser.Id
                },
                new Transaction
                {
                    Id = 0,
                    Type = "Ausgabe",
                    Amount = 32.50m,
                    BookingDate = new DateTime(2026, 9, 12),
                    CreatedAt = DateTime.Now,
                    Description = "Kino mit Freunden",
                    CategoryId = ausgehen.Id,
                    Category = ausgehen,
                    UserId = testUser.Id
                }
            );

            context.SaveChanges();
        }
    }
}