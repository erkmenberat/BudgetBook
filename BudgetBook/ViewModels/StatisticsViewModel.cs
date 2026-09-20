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
    }
}
