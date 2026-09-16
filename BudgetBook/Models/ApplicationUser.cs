using Microsoft.AspNetCore.Identity;

namespace BudgetBook.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;

        public List<Transaction> Transactions { get; set; } = new();
    }
}