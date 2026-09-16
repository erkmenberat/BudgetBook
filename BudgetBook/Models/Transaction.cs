using System.ComponentModel.DataAnnotations;

namespace BudgetBook.Models
{
    public class Transaction
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Bitte einen Typ angeben.")]
        [RegularExpression("^(Einnahme|Ausgabe)$", ErrorMessage = "Der Typ muss Einnahme oder Ausgabe sein.")]
        [StringLength(20, ErrorMessage = "Der Typ darf maximal 20 Zeichen lang sein.")]
        public required string Type { get; set; }

        [Range(0.01, 999999999.99, ErrorMessage = "Der Betrag muss groesser als 0 sein.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Bitte ein Buchungsdatum angeben.")]
        [DataType(DataType.Date)]
        public DateTime BookingDate { get; set; }

        [Required]
        public required DateTime CreatedAt { get; set; }

        [StringLength(250, ErrorMessage = "Die Beschreibung darf maximal 250 Zeichen lang sein.")]
        public string? Description { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Bitte eine Kategorie auswaehlen.")]
        public int CategoryId { get; set; }

        public Category? Category { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        public ApplicationUser? User { get; set; }
    }
}
