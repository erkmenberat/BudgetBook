using System.Runtime.CompilerServices;
using System.ComponentModel.DataAnnotations; 

namespace BudgetBook.Models
{
    public class Transaction
    {
        public required int Id { get; set; }
        public required string Type { get; set; }
        public required decimal Amount { get; set; }

        [DataType(DataType.Date)]
        public required DateTime BookingDate { get; set; }
        public required DateTime CreatedAt { get; set; }
        public string? Description { get; set; }
        public required int CategoryId { get; set; }
        public required Category Category { get; set; }
        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }

    }
}
