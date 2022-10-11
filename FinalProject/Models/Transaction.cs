using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace FinalProject.Models
{
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionId { get; set; }

        [AllowNull]
        public Account? ToAccount { get; set; }

        [AllowNull]
        public Account? FromAccount { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        public int Amount { get; set; }

        public string Memo { get; set; }

    }
}
