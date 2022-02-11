using System;
using System.ComponentModel.DataAnnotations;

namespace lab1.Models
{
    public class User
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; init; }
    }
}
