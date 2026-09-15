namespace BudgetBook.Models
{
    public class Category
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Type { get; set; }
        public required bool IsActive { get; set; }
        public List<Transaction>? Transactions { get; set; }
    }
}
