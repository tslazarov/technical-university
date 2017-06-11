﻿using Lipwig.Models.Contracts;
using Lipwig.Utilities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lipwig.Models
{
    public class Expense : ITransaction
    {
        public Expense()
        {

        }

        public Expense(Guid id,
            DateTime date,
            decimal amount,
            string side,
            string description,
            PaymentType paymentType,
            CategoryType categoryType)
        {
            this.Id = id;
            this.Date = date;
            this.Amount = amount / Constants.CurrencyValue;
            this.Side = side;
            this.Description = description;
            this.PaymentType = paymentType;
            this.CategoryType = categoryType;
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
                return decimal.Round(this.Amount * Constants.CurrencyValue);
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

        public CategoryType CategoryType { get; set; }
    }
}