using System.ComponentModel.DataAnnotations;

namespace BudgetBook.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Bitte einen Namen angeben.")]
        [StringLength(50, ErrorMessage = "Der Name darf maximal 50 Zeichen lang sein.")]
        public required string Name { get; set; }

        [Required(ErrorMessage = "Bitte einen Typ angeben.")]
        [StringLength(50, ErrorMessage = "Der Typ darf maximal 50 Zeichen lang sein.")]
        public required string Type { get; set; }

        public bool IsActive { get; set; }

        public List<Transaction>? Transactions { get; set; }
    }
}
