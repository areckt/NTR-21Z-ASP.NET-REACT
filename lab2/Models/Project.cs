using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace lab1.Models
{
    public class Project
    {
        public int Id { get; set; }
        [Required]
        public string Code { get; set; }

        public string Manager { get; set; }

        public string Name { get; set; }

        public int Budget { get; set; }

        public bool Active { get; set; }
        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}
