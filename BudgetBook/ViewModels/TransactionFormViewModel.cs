using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BudgetBook.ViewModels
{
    public class TransactionFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Bitte einen Typ angeben.")]
        [RegularExpression("^(Einnahme|Ausgabe)$", ErrorMessage = "Der Typ muss Einnahme oder Ausgabe sein.")]
        [StringLength(20, ErrorMessage = "Der Typ darf maximal 20 Zeichen lang sein.")]
        public string Type { get; set; } = string.Empty;

        [Range(0.01, 999999999.99, ErrorMessage = "Der Betrag muss groesser als 0 sein.")]
        public decimal Amount { get; set; }

        [Required(ErrorMessage = "Bitte ein Buchungsdatum angeben.")]
        [DataType(DataType.Date)]
        public DateTime BookingDate { get; set; } = DateTime.Today;

        [StringLength(250, ErrorMessage = "Die Beschreibung darf maximal 250 Zeichen lang sein.")]
        public string? Description { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Bitte eine Kategorie auswaehlen.")]
        public int CategoryId { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
    }
}
