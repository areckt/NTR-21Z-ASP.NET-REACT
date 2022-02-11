using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using lab1.Models;

namespace lab1.Utils
{
    public class ProjectsWriter
    {
        string FilePath;

        public ProjectsWriter()
        {
            FilePath = "data_files/projects.json";
        }

        public void WriteToFile(Project NewProject)
        {
            FileData data = new FileData();
            ProjectsReader pr = new ProjectsReader(); // read current file content

            // check if there is already a project with code == NewProject.code
            // if there is, remove it
            if (pr.Projects.Any(project => project.Code == NewProject.Code))
            {
                var projectToUpdate = pr.Projects.Find(project => project.Code == NewProject.Code);
                pr.Projects.Remove(projectToUpdate);
            }
            // add the NewProject
            pr.Projects.Add(NewProject);

            data.projects = pr.Projects.Select(project => ProjectEntry.FromProject(project)).ToList();

            string json = JsonConvert.SerializeObject(data);

            StreamWriter r = new StreamWriter(FilePath);
            r.Write(json);
            r.Close();
        }

        private class FileData
        {
            public List<ProjectEntry> projects { get; set; }

            public FileData()
            {
                projects = new List<ProjectEntry>();
            }
        }

        private class ProjectEntry
        {
            public string code;
            public string manager;
            public string name;
            public int budget;
            public bool active;
            public List<SubactivityEntry> subactivities;

            private ProjectEntry(string codeData, string managerData, string nameData, int budgetData, bool activeData, string[] subactivitiesData)
            {
                List<SubactivityEntry> subactivityEntries = new List<SubactivityEntry>();
                foreach (var subactivity in subactivitiesData)
                {
                    subactivityEntries.Add(new SubactivityEntry(subactivity));
                }

                code = codeData;
                manager = managerData;
                name = nameData;
                budget = budgetData;
                active = activeData;
                subactivities = subactivityEntries;
            }

            public static ProjectEntry FromProject(Project project)
            {
                return new ProjectEntry(
                    project.Code,
                    project.Manager,
                    project.Name,
                    project.Budget,
                    project.Active,
                    project.Subactivities);
            }

            public class SubactivityEntry
            {
                public string code { get; set; }
                public SubactivityEntry(string Code)
                {
                    code = Code;
                }
            }
        }
    }
}
