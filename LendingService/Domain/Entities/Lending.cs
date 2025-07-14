using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LendingService.Domain.Entities
{
    public class Lending
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int BookId { get; set; }
        public int BorrowerUserId { get; set; }
        public DateTime LendDate { get; set; } = DateTime.UtcNow;
        public DateTime? ReturnDate { get; set; }
        public bool IsReturned => ReturnDate.HasValue;
    }
}