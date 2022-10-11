using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FinalProject.Models
{
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AccountId { get; set; }

        [Required]
        public User Customer { get; set; }

        [Required]
        public string AccountType { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public float CurrAmount { get; set; }

        [Required]
        public string Status { get; set; }


    }
}
