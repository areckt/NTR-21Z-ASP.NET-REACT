using System;
using System.Collections.Generic;
using System.Linq;
using lab1.Models;

namespace lab1.Storage
{
  public class ActivityEntryStorage
  {
      private readonly TMSContext _db;

      public ActivityEntryStorage(TMSContext db)
      {
        _db = db;
      }

      public IEnumerable<ActivityEntry> GetActivityEntries()
      {
        return _db.ActivityEntries.ToList();
      }

      public IEnumerable<ActivityEntry> GetUserActivityEntries(string username)
      {
        var activityEntries = _db.ActivityEntries.ToList();
        var activities = activityEntries.Where(entry => entry.Username == username);
        return activities;
      }

      public IEnumerable<ActivityEntry> GetActivityEntriesByUserAndMonth(string username, int month, int year) {
        var activityEntries = _db.ActivityEntries.ToList();
        var activities = activityEntries.Where(entry => entry.Username == username && entry.Month == month && entry.Year == year);
        return activities;
      }

      public IEnumerable<ActivityEntry> GetActivityEntriesByProjectAndMonth(string code, int month, int year) {
        var activityEntries = _db.ActivityEntries.ToList();
        var activities = activityEntries.Where(entry => entry.Code == code && entry.Month == month && entry.Year == year);
        return activities;
      }

      public IEnumerable<ActivityEntry> GetActivityEntriesByProjectAllTime(string code) {
        var activityEntries = _db.ActivityEntries.ToList();
        var activities = activityEntries.Where(entry => entry.Code == code);
        return activities;
      }

      public void AddActivityEntry(ActivityEntry activityEntry)
      {
        _db.ActivityEntries.Add(activityEntry);
        _db.SaveChanges();
      }
      public void RemoveActivityEntry(int id)
      {
        var activityEntries = _db.ActivityEntries.ToList();
        var entryToRemove = activityEntries.Find(entryToRemove => entryToRemove.Id == id);
        if (entryToRemove != null) _db.ActivityEntries.Remove(entryToRemove);
        _db.SaveChanges();
      }
  }
}