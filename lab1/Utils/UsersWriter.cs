using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using lab1.Models;

namespace lab1.Utils
{
    public class UsersWriter
    {
        string FilePath;

        public UsersWriter()
        {
            FilePath = "data_files/users.json";
        }

        public void WriteToFile(string NewUsername)
        {
            FileData data = new FileData();
            UsersReader ur = new UsersReader(); // read current file content
            User newUser = new User(NewUsername);   // create new User
            ur.Users.Add(newUser);  // add new User to ones from file
            data.users = ur.Users.Select(user => UserEntry.FromUser(user)).ToList();

            string json = JsonConvert.SerializeObject(data);

            StreamWriter r = new StreamWriter(FilePath);
            r.Write(json);
            r.Close();
        }

        private class FileData
        {
            public List<UserEntry> users { get; set; }

            public FileData()
            {
                users = new List<UserEntry>();
            }
        }

        private class UserEntry
        {
            public string name;

            private UserEntry(string nameData)
            {
                name = nameData;
            }

            public static UserEntry FromUser(User user)
            {
                return new UserEntry(user.Name);
            }
        }
    }
}
