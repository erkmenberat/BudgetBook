namespace BudgetBook.Models
{
    public class Category
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Type { get; set; }
        public required bool IsActive { get; set; }
        public required List<Transaction> Transactions { get; set; }
    }
}
