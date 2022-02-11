using System;
using System.Collections.Generic;

namespace lab1.Models
{
    public class UserActivity
    {
        public string Name { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public bool Frozen { get; set; }

        public List<ActivityEntry> Entries { get; set; }
        public List<AcceptedActivity> AcceptedActivities { get; set; }

        public UserActivity(string name, int month, int year, bool frozen, List<ActivityEntry> entries, List<AcceptedActivity> acceptedActivities)
        {
            Name = name;
            Month = month;
            Year = year;
            Frozen = frozen;

            Entries = entries;
            AcceptedActivities = acceptedActivities;
        }
    }

    public class ActivityEntry
    {
        public string Date { get; set; }
        public string Code { get; set; }
        public string Subcode { get; set; }
        public int Time { get; set; }
        public string Description { get; set; }

        public ActivityEntry(string date, string code, string subcode, int time, string description)
        {
            Date = date;
            Code = code;
            Subcode = subcode;
            Time = time;
            Description = description;
        }
    }

    public class AcceptedActivity
    {
        public string Code { get; set; }
        public int Time { get; set; }

        public AcceptedActivity(string code, int time)
        {
            Code = code;
            Time = time;
        }
    }
}
