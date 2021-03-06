﻿using Lipwig.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lipwig.Models
{
    public class User
    {
        private decimal balance;
        private ICollection<Income> incomes;
        private ICollection<Expense> expenses;

        public User()
        {

        }

        public User(Guid id,
            string email,
            string firstName,
            string lastName,
            decimal balance,
            Currency currency)
        {
            this.Id = id;
            this.Email = email;
            this.FirstName = firstName;
            this.LastName = lastName;
            this.Balance = balance / currency.Value;
            this.CurrencyId = currency.Id;
            this.Currency = currency;

            this.Incomes = new HashSet<Income>();
            this.Expenses = new HashSet<Expense>();
        }

        [Key]
        public Guid Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(40)]
        [Index(IsUnique = true)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string FirstName { get; set; }


        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string LastName { get; set; }

        [Required]
        public string Salt { get; set; }

        [Required]
        public string HashedPassword { get; set; }

        public decimal Balance { get; set; }
        
        [NotMapped]
        public decimal LocalizedBalance
        {
            get
            {
                return decimal.Round(this.Balance * this.Currency.Value);
            }
        }

        public Guid CurrencyId { get; set; }

        public virtual Currency Currency { get; set; }

        public virtual ICollection<Income> Incomes
        {
            get
            {
                return this.incomes;
            }
            set
            {
                this.incomes = value;
            }
        }

        public virtual ICollection<Expense> Expenses
        {
            get
            {
                return this.expenses;
            }
            set
            {
                this.expenses = value;
            }
        }
    }

}
