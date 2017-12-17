using Lipwig.Models.Contracts;
using Lipwig.Utilities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
            this.Amount = amount / ViewBag.CurrencyValue;
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

        [NotMapped]
        public decimal LocalizedAmount
        {
            get
            {
                return decimal.Round(this.Amount * ViewBag.CurrencyValue);
            }
        }

        [Required]
        [MinLength(3)]
        [MaxLength(40)]
        public string Side { get; set; }

        [MinLength(3)]
        [MaxLength(100)]
        public string Description { get; set; }

        public PaymentType PaymentType { get; set; }

        [NotMapped]
        public string CategoryName
        {
            get
            {
                return "N/A";
            }
        }

        [NotMapped]
        public bool IsExpense
        {
            get
            {
                return false;
            }
        }

        [NotMapped]
        public bool IsIncome
        {
            get
            {
                return true;
            }
        }
    }
}
