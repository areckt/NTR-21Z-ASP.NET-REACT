using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using lab1.Models;

namespace lab1.Utils
{
    public class UserActivityReader
    {
        public List<UserActivity> UserActivities { get; set; }

        public UserActivityReader()
        {
            UserActivities = new List<UserActivity>();
            ReadFiles();
        }

        private void ReadFiles()
        {
            string folderPath = "data_files/activity_files";

            // get all filenames in the folder path
            DirectoryInfo di = new DirectoryInfo(folderPath);
            FileInfo[] files = di.GetFiles("*.json");
            List<string> filenamesList = new List<string>();
            foreach (FileInfo file in files)
            {
                filenamesList.Add(file.Name);
            }
            string[] filenames = filenamesList.ToArray();

            foreach (var filename in filenames)
            {
                StreamReader r = new StreamReader(folderPath + "/" + filename);
                string json = r.ReadToEnd();
                r.Close();
                dynamic data = JsonConvert.DeserializeObject(json);

                DeserializeData(data);
            }
        }

        private void DeserializeData(dynamic data)
        {
            string username = data["name"].ToString();
            int month = Convert.ToInt32(data["month"].ToString());
            int year = Convert.ToInt32(data["year"].ToString());
            bool frozen = Convert.ToBoolean(data["frozen"].ToString());

            // deserialize entries in foreach loop
            List<ActivityEntry> newActivityEntries = new List<ActivityEntry>();
            foreach (var activityEntry in data.entries)
            {
                ActivityEntry newActivityEntry = new ActivityEntry(
                    activityEntry["date"].ToString(),
                    activityEntry["code"].ToString(),
                    activityEntry["subcode"].ToString(),
                    Convert.ToInt32(activityEntry["time"].ToString()),
                    activityEntry["description"].ToString()
                );
                newActivityEntries.Add(newActivityEntry);
            }

            // deserialize acceptedActivities in foreach loop
            List<AcceptedActivity> newAcceptedActivities = new List<AcceptedActivity>();
            foreach (var acceptedActivity in data.accepted)
            {
                AcceptedActivity newAcceptedActivity = new AcceptedActivity(
                    acceptedActivity["code"].ToString(),
                    Convert.ToInt32(acceptedActivity["time"].ToString())
                );
                newAcceptedActivities.Add(newAcceptedActivity);
            }

            // create new UserActivity object and add it to the UserActivities property
            UserActivity newUserActivity = new UserActivity(
                username,
                month,
                year,
                frozen,
                newActivityEntries,
                newAcceptedActivities
            );

            UserActivities.Add(newUserActivity);
        }
    }
}
