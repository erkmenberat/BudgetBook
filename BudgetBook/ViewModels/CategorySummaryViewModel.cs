namespace BudgetBook.ViewModels
{
    // Summe der Ausgaben einer einzelnen Kategorie im gewählten Zeitraum
    public class CategorySummaryViewModel
    {
        public int CategoryId { get; set; }

        public string CategoryName { get; set; } = string.Empty;

        public decimal Total { get; set; }

        public int TransactionCount { get; set; }

        // Anteil an den gesamten Ausgaben in Prozent
        public decimal Share { get; set; }
    }
}
