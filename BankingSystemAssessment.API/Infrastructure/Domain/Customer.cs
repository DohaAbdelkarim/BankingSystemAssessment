using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankingSystemAssessment.API.Infrastructure.Domain
{
    [Index(nameof(CustomerID), IsUnique = true)]
    public class Customer
    {
        public Customer()
        {
            Account = new HashSet<Account>();
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(DomainConstants.Customer.CustomerIDLength,MinimumLength = (DomainConstants.Customer.CustomerIDLength))]
        public string CustomerID { get; set; }
        
        [Required]
        [MaxLength(DomainConstants.Customer.FirstNameMaxLength)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(DomainConstants.Customer.LastNameMaxLength)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(DomainConstants.Customer.MobileMaxLength)]
        public string Mobile { get; set; }

        [Required]
        [MaxLength(DomainConstants.Customer.EmailMaxLength)]
        public string Email { get; set; }

        [MaxLength(DomainConstants.Customer.AddressMaxLength)]
        public string Address { get; set; }

        public ICollection<Account> Account { get; set; }
    }
}