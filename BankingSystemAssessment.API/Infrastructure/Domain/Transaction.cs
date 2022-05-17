using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingSystemAssessment.API.Infrastructure.Domain
{
    public class Transaction
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Account")]
        public int AccountId { get; set; }

        [Required]
        [MaxLength(DomainConstants.Transaction.ReferenceNumberMaxLength)]
        public string ReferenceNumber { get; set; }

        [MaxLength(DomainConstants.Transaction.DescriptionMaxLength)]
        public string Description { get; set; }

        [Required]
        public decimal Debit { get; set; }

        [Required]
        public decimal Credit { get; set; }

        [Required]
        public DateTimeOffset TransactionDate { get; set; }

        [Required]
        [MaxLength(DomainConstants.Transaction.TranscationTypeMaxLength)]
        public string TranscationType { get; set; }

        [Required]
        public decimal BalanceAfter { get; set; }

        public Account Account { get; set; }
    }
}