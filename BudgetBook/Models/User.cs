using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BudgetBook.Models
{
    public class User
    {
        public required int Id { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }

        [EmailAddress]
        public required string Email { get; set; }
        public required string PasswordHash { get; set; }
        public required List<Transaction> Transactions { get; set; }

    }
}
