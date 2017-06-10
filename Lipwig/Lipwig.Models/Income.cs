using Lipwig.Models.Contracts;
using System;
using System.ComponentModel.DataAnnotations;

namespace Lipwig.Models
{
    public class Income : ITransaction
    {
        public Income()
        {

        }

        public Income(Guid id,
            DateTime date,
            decimal amount,
            string side,
            string description,
            PaymentType paymentType)
        {
            this.Id = id;
            this.Date = date;
            this.Amount = amount;
            this.Side = side;
            this.Description = description;
            this.PaymentType = paymentType;
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(40)]
        public string Side { get; set; }

        [MinLength(3)]
        [MaxLength(100)]
        public string Description { get; set; }

        public PaymentType PaymentType { get; set; }
    }
}
