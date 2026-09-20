using BudgetBook.Models;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BudgetBook.ViewModels
{
    public class StatisticsViewModel : IValidatableObject
    {
        [Display(Name = "Von")]
        [DataType(DataType.Date)]
        public DateTime? From { get; set; }

        [Display(Name = "Bis")]
        [DataType(DataType.Date)]
        public DateTime? To { get; set; }

        [Display(Name = "Typ")]
        [RegularExpression(TransactionType.ValidationPattern, ErrorMessage = "Der Typ muss Einnahme oder Ausgabe sein.")]
        public string? Type { get; set; }

        [Display(Name = "Kategorie")]
        public int? CategoryId { get; set; }

        public IEnumerable<SelectListItem> TypeOptions { get; set; } = new List<SelectListItem>();

        public IEnumerable<SelectListItem> CategoryOptions { get; set; } = new List<SelectListItem>();

        public decimal TotalIncome { get; set; }

        public decimal TotalExpense { get; set; }

        public decimal Balance => TotalIncome - TotalExpense;

        public int TransactionCount { get; set; }

        public bool HasData => TransactionCount > 0;

        public List<CategorySummaryViewModel> ExpensesByCategory { get; set; } = new();

        public List<MonthlySummaryViewModel> MonthlySummaries { get; set; } = new();

        // Ausgabenkategorie mit der größten Summe; null, wenn es keine Ausgaben gibt
        public CategorySummaryViewModel? TopExpenseCategory => ExpensesByCategory.FirstOrDefault();

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (From.HasValue && To.HasValue && From.Value.Date > To.Value.Date)
            {
                yield return new ValidationResult(
                    "Das Von-Datum darf nicht nach dem Bis-Datum liegen.",
                    new[] { nameof(From), nameof(To) });
            }
        }
    }
}
