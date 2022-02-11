using System;
using System.Collections.Generic;
using System.Linq;
using lab1.Models;

namespace lab1.Storage
{
  public class ProjectStorage
  {
      private readonly TMSContext _db;

      public ProjectStorage(TMSContext db)
      {
        _db = db;
      }

      public IEnumerable<Project> GetAllProjects()
      {
        return _db.Projects.ToList();
      }

      public IEnumerable<Project> GetActiveProjects()
      {
        return _db.Projects.Where(project => project.Active == true).ToList();
      }

      public Project GetProjectByCode(string code)
      {
        var allProjects = _db.Projects.ToList();
        return allProjects.Find(p => p.Code == code);
      }

      public void AddProject(Project newProject)
      {
        _db.Projects.Add(newProject);
        _db.SaveChanges();
      }

      public void UpdateProject(string code, string newName, int newBudget) {
        var project = _db.Projects.First(project => project.Code == code);
        project.Name = newName;
        project.Budget = newBudget;
        _db.SaveChanges();
      }

      public void CloseProject(string code) {
        var project = _db.Projects.First(project => project.Code == code);
        project.Active = false;
        _db.SaveChanges();
      }
  }
}