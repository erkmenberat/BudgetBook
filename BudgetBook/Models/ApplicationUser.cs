using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BudgetBook.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required(ErrorMessage = "Bitte einen Vornamen angeben.")]
        [StringLength(50, ErrorMessage = "Der Vorname darf maximal 50 Zeichen lang sein.")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bitte einen Nachnamen angeben.")]
        [StringLength(50, ErrorMessage = "Der Nachname darf maximal 50 Zeichen lang sein.")]
        public string Surname { get; set; } = string.Empty;

        public List<Transaction> Transactions { get; set; } = new();
    }
}
