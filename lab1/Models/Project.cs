using System;

namespace lab1.Models
{
    public class Project
    {
        public string Code { get; set; }

        public string Manager { get; set; }

        public string Name { get; set; }

        public int Budget { get; set; }

        public bool Active { get; set; }

        public string[] Subactivities { get; set; }

        public Project(string code, string manager, string name, int budget, bool active, string[] subactivities)
        {
            Code = code;
            Manager = manager;
            Name = name;
            Budget = budget;
            Active = active;
            Subactivities = subactivities;
        }
    }
}
