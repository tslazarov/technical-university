using System;
using System.ComponentModel.DataAnnotations;

namespace Lipwig.Models
{
    public class Currency
    {
        [Key]
        public Guid Id { get; set; }
        
        [Required]
        [MinLength(2)]
        [MaxLength(10)]
        public string Name { get; set; }

        [Required]
        public decimal Value { get; set; }
    }
}
