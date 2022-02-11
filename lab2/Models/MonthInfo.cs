using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace lab1.Models
{
    public class MonthInfo
    {
        public int Id { get; set; }
        [Required]
        public int Month { get; set; }
        public int Year { get; set; }
        public string ProjectCode { get; set; }
        public string Username { get; set; }
        public bool Frozen { get; set; }
        public int AcceptedTime { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}
