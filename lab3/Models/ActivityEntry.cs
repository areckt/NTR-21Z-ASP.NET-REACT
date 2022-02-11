using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace lab1.Models
{
    public class ActivityEntry
    {
        public int Id { get; set; }
        [Required]
        public string Username { get; init; }
        public int Day { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string Code { get; set; }
        public string Subcode { get; set; }
        public int Time { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}
