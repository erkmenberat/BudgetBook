namespace BudgetBook.Models
{
    // Zentrale Definition der beiden Buchungsarten, damit die Texte nicht an vielen Stellen einzeln stehen
    public static class TransactionType
    {
        public const string Income = "Einnahme";

        public const string Expense = "Ausgabe";

        // Fuer Attribute, die nur Konstanten erlauben; wird zur Compilezeit zusammengesetzt
        public const string ValidationPattern = "^(" + Income + "|" + Expense + ")$";

        public static IReadOnlyList<string> All { get; } = new[] { Income, Expense };
    }
}
