using BudgetBook.Models;
using BudgetBook.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace BudgetBook.Controllers
{
    [Authorize]
    public class StatisticsController : Controller
    {
        private readonly BudgetBookContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public StatisticsController(BudgetBookContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: STATISTICS
        public async Task<IActionResult> Index(StatisticsViewModel filter)
        {
            var query = BuildOwnTransactionsQuery(filter);

            filter.TotalIncome = await SumAmountAsync(query, "Einnahme");
            filter.TotalExpense = await SumAmountAsync(query, "Ausgabe");
            filter.TransactionCount = await query.CountAsync();
            filter.ExpensesByCategory = await GetExpensesByCategoryAsync(query, filter.TotalExpense);
            filter.MonthlySummaries = await GetMonthlySummariesAsync(query);

            filter.TypeOptions = BuildTypeOptions(filter.Type);
            filter.CategoryOptions = await BuildCategoryOptionsAsync(filter.CategoryId);

            return View(filter);
        }

        // Ausgaben je Kategorie, absteigend nach Summe. Das erste Element ist die Top-Kategorie.
        private static async Task<List<CategorySummaryViewModel>> GetExpensesByCategoryAsync(
            IQueryable<Transaction> query, decimal totalExpense)
        {
            var summaries = await query
                .Where(t => t.Type == "Ausgabe")
                .GroupBy(t => new { t.CategoryId, CategoryName = t.Category!.Name })
                .Select(g => new CategorySummaryViewModel
                {
                    CategoryId = g.Key.CategoryId,
                    CategoryName = g.Key.CategoryName,
                    Total = g.Sum(t => t.Amount),
                    TransactionCount = g.Count()
                })
                .ToListAsync();

            // SQLite kann nicht zuverlaessig nach decimal sortieren, darum erst nach dem Laden sortieren
            summaries = summaries
                .OrderByDescending(s => s.Total)
                .ThenBy(s => s.CategoryName)
                .ToList();

            foreach (var summary in summaries)
            {
                summary.Share = totalExpense == 0m ? 0m : summary.Total / totalExpense * 100m;
            }

            return summaries;
        }

        // Alle Buchungen je Monat, neuester Monat zuerst
        private static async Task<List<MonthlySummaryViewModel>> GetMonthlySummariesAsync(IQueryable<Transaction> query)
        {
            var summaries = await query
                .GroupBy(t => new { t.BookingDate.Year, t.BookingDate.Month })
                .Select(g => new MonthlySummaryViewModel
                {
                    Year = g.Key.Year,
                    Month = g.Key.Month,
                    TotalIncome = g.Sum(t => t.Type == "Einnahme" ? t.Amount : 0m),
                    TotalExpense = g.Sum(t => t.Type == "Ausgabe" ? t.Amount : 0m),
                    TransactionCount = g.Count()
                })
                .ToListAsync();

            return summaries
                .OrderByDescending(s => s.Year)
                .ThenByDescending(s => s.Month)
                .ToList();
        }

        // Liefert nur die Buchungen der angemeldeten Person, eingeschraenkt auf den gewaehlten Zeitraum.
        private IQueryable<Transaction> BuildOwnTransactionsQuery(StatisticsViewModel filter)
        {
            var userId = _userManager.GetUserId(User);

            var query = _context.Transaction
                .Where(t => t.UserId == userId);

            if (filter.From.HasValue)
            {
                query = query.Where(t => t.BookingDate >= filter.From.Value.Date);
            }

            if (filter.To.HasValue)
            {
                // Bis-Datum einschliessen, auch wenn ein Buchungsdatum eine Uhrzeit enthaelt
                var endOfDay = filter.To.Value.Date.AddDays(1);
                query = query.Where(t => t.BookingDate < endOfDay);
            }

            if (!string.IsNullOrEmpty(filter.Type))
            {
                query = query.Where(t => t.Type == filter.Type);
            }

            if (filter.CategoryId.HasValue)
            {
                query = query.Where(t => t.CategoryId == filter.CategoryId.Value);
            }

            return query;
        }

        private static IEnumerable<SelectListItem> BuildTypeOptions(string? selectedType)
        {
            var types = new[] { "Einnahme", "Ausgabe" };

            return types
                .Select(type => new SelectListItem
                {
                    Value = type,
                    Text = type,
                    Selected = type == selectedType
                })
                .ToList();
        }

        // Fuer den Filter auch inaktive Kategorien anbieten, damit aeltere Buchungen auffindbar bleiben.
        private async Task<IEnumerable<SelectListItem>> BuildCategoryOptionsAsync(int? selectedCategoryId)
        {
            return await _context.Category
                .OrderBy(c => c.Name)
                .Select(c => new SelectListItem
                {
                    Value = c.Id.ToString(),
                    Text = c.Name + " - " + c.Type,
                    Selected = selectedCategoryId.HasValue && c.Id == selectedCategoryId.Value
                })
                .ToListAsync();
        }

        // Summiert die Betraege eines Typs; ohne Treffer ist das Ergebnis 0 und nicht NULL.
        private static async Task<decimal> SumAmountAsync(IQueryable<Transaction> query, string type)
        {
            return await query
                .Where(t => t.Type == type)
                .SumAsync(t => (decimal?)t.Amount) ?? 0m;
        }
    }
}
