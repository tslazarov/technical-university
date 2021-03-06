﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartherThanYou.Models
{
    public class User
    {
        public User()
        {

        }

        public User(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        [Index(IsUnique = true)]
        public string Username { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(30)]
        public string Password { get; set; }
    }
}
