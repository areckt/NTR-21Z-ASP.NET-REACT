using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using lab1.Models;

namespace lab1.Utils
{
    public class ProjectsReader
    {
        public List<Project> Projects { get; set; }

        public ProjectsReader()
        {
            Projects = new List<Project>();
            ReadFile();
        }

        private void ReadFile()
        {
            string FilePath = "data_files/projects.json";

            StreamReader r = new StreamReader(FilePath);
            string json = r.ReadToEnd();
            r.Close();
            dynamic data = JsonConvert.DeserializeObject(json);

            DeserializeData(data);
        }

        private void DeserializeData(dynamic data)
        {

            foreach (var project in data.projects)
            {
                List<string> subactivitiesList = new List<string>();
                foreach (var subactivity in project.subactivities)
                {
                    subactivitiesList.Add(subactivity["code"].ToString());
                }

                string[] subactivities = subactivitiesList.ToArray();

                Project newProject = new Project(
                    project["code"].ToString(),
                    project["manager"].ToString(),
                    project["name"].ToString(),
                    Convert.ToInt32(project["budget"].ToString()),
                    Convert.ToBoolean(project["active"].ToString()),
                    subactivities
                );
                Projects.Add(newProject);
            }
        }
    }
}
