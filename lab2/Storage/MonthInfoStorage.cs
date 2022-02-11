using System;
using System.Collections.Generic;
using System.Linq;
using lab1.Models;

namespace lab1.Storage
{
  public class MonthInfoStorage
  {
      private readonly TMSContext _db;

      public MonthInfoStorage(TMSContext db)
      {
        _db = db;
      }

      public IEnumerable<MonthInfo> GetAllMonthInfos()
      {
        return _db.MonthInfos.ToList();
      }
      public void AddMonthInfo(MonthInfo newMonthInfo)
      {
        _db.MonthInfos.Add(newMonthInfo);
        _db.SaveChanges();
      }
      public void FreezeMonth(string username, int month, int year)
      {
        var monthInfos = _db.MonthInfos.ToList().Where(m => m.Username == username && m.Month == month && m.Year == year);
        foreach (var monthInfo in monthInfos)
        {
        monthInfo.Frozen = true;
        }
        _db.SaveChanges();
      }

      public bool CheckIfMonthFrozen(string username, int month, int year)
      {
        var monthInfo = _db.MonthInfos.FirstOrDefault(m => m.Username == username && m.Month == month && m.Year == year);
        if (monthInfo != null) return monthInfo.Frozen;
        return false;
      }
      public void SetAcceptedTime(string username, string projectCode, int month, int year, int time) {
        var monthInfo = _db.MonthInfos.FirstOrDefault(m => m.Username == username && m.ProjectCode == projectCode && m.Month == month && m.Year == year);
        monthInfo.AcceptedTime = time;
        _db.SaveChanges();
      }
      public int GetAcceptedTime(string username, string projectCode, int month, int year) {
        var monthInfo = _db.MonthInfos.FirstOrDefault(m => m.Username == username && m.ProjectCode == projectCode && m.Month == month && m.Year == year);
        if (monthInfo != null) return monthInfo.AcceptedTime;
        return 0;
      }
      public bool CheckIfMonthInfoExists(string username, string projectCode, int month, int year) {
        var monthInfo = _db.MonthInfos.FirstOrDefault(m => m.Username == username && m.ProjectCode == projectCode && m.Month == month && m.Year == year);
        if (monthInfo != null) return true;
        return false;
      }
  }
}