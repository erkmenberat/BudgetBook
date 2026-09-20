using System.ComponentModel.DataAnnotations;

namespace BudgetBook.ViewModels
{
    public class StatisticsViewModel
    {
        [Display(Name = "Von")]
        [DataType(DataType.Date)]
        public DateTime? From { get; set; }

        [Display(Name = "Bis")]
        [DataType(DataType.Date)]
        public DateTime? To { get; set; }

        public decimal TotalIncome { get; set; }

        public decimal TotalExpense { get; set; }

        public decimal Balance => TotalIncome - TotalExpense;

        public int TransactionCount { get; set; }

        public bool HasData => TransactionCount > 0;

        public List<CategorySummaryViewModel> ExpensesByCategory { get; set; } = new();

        public List<MonthlySummaryViewModel> MonthlySummaries { get; set; } = new();

        // Ausgabenkategorie mit der groessten Summe; null, wenn es keine Ausgaben gibt
        public CategorySummaryViewModel? TopExpenseCategory => ExpensesByCategory.FirstOrDefault();
    }
}
