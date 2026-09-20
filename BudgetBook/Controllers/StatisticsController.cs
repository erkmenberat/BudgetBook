using BudgetBook.Models;
using BudgetBook.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
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

            return View(filter);
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

            return query;
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
