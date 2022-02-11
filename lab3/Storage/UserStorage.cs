using System;
using System.Collections.Generic;
using System.Linq;
using lab1.Models;

namespace lab1.Storage
{
  public class UserStorage
  {
      private readonly TMSContext _db;

      public UserStorage(TMSContext db)
      {
        _db = db;
      }

      public IEnumerable<User> GetUsers()
      {
        return _db.Users.ToList();
      }

      public void AddUser(User user)
      {
        _db.Users.Add(user);
        _db.SaveChanges();
      }
  }
}