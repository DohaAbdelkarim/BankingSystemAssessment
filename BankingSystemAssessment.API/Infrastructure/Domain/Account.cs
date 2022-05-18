using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingSystemAssessment.API.Infrastructure.Domain
{
    public class Account
    {
        public Account()
        {
            Transaction = new HashSet<Transaction>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }

        [Required]
        [StringLength(DomainConstants.Account.AccountNumberMaxLength)]
        public string AccountNumber { get; set; }

        [Required]
        public decimal Balance { get; set; }

        [Required]
        [MaxLength(DomainConstants.Account.CurrencyMaxLength)]
        public string Currency { get; set; }

        [Required]
        [MaxLength(DomainConstants.Account.StatusMaxLength)]
        public string Status { get; set; }

        [Required]
        [Column("CreatedDate", TypeName = "date")]
        public DateTime CreatedDate { get; set; }

        public Customer Customer { get; set; }
        public virtual ICollection<Transaction> Transaction { get; set; }
    }
}