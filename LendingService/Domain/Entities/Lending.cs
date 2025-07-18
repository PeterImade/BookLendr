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
        public int UserId { get; set; }
        public DateTime LendDate { get; set; } = DateTime.UtcNow;
        public DateTime DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public Status Status { get; set; } 
    }
}