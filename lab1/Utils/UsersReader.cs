using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using lab1.Models;

namespace lab1.Utils
{
    public class UsersReader
    {
        public List<User> Users { get; set; }

        public UsersReader()
        {
            Users = new List<User>();
            ReadFile();
        }

        private void ReadFile()
        {
            string FilePath = "data_files/users.json";

            StreamReader r = new StreamReader(FilePath);
            string json = r.ReadToEnd();
            r.Close();
            dynamic data = JsonConvert.DeserializeObject(json);

            DeserializeData(data);
        }

        private void DeserializeData(dynamic data)
        {

            foreach (var user in data.users)
            {
                User newUser = new User(user["name"].ToString());
                Users.Add(newUser);
            }
        }
    }
}
