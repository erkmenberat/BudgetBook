namespace BudgetBook.ViewModels
{
    // Einnahmen und Ausgaben eines einzelnen Monats im gewählten Zeitraum
    public class MonthlySummaryViewModel
    {
        public int Year { get; set; }

        public int Month { get; set; }

        public decimal TotalIncome { get; set; }

        public decimal TotalExpense { get; set; }

        public decimal Balance => TotalIncome - TotalExpense;

        public int TransactionCount { get; set; }

        public string Label => $"{Month:00}.{Year}";
    }
}
