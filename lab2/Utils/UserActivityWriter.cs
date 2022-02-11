// using System;
// using System.Collections.Generic;
// using System.IO;
// using Newtonsoft.Json;
// using System.Linq;
// using lab1.Models;

// namespace lab1.Utils
// {
//     public class UserActivityWriter
//     {
//         string DirectoryPath;
//         string FilePath;
//         string Username;
//         int Year;
//         int Month;

//         public UserActivityWriter(string username, string year, string month)
//         {
//             Username = username;
//             Year = Convert.ToInt32(year);
//             Month = Convert.ToInt32(month);
//             DirectoryPath = "data_files/activity_files";
//             FilePath =
//                 DirectoryPath + "/" +
//                 username + "_" +
//                 year + "_" +
//                 month +
//                 ".json";
//         }

//         public void AddNewActivityEntry(ActivityEntry NewActivityEntry)
//         {

//             // create empty List<AcceptedActivity>
//             List<AcceptedActivity> emptyAcceptedActivityList = new List<AcceptedActivity>();

//             // create List<ActivityEntry> with one element - NewActivityEntry
//             List<ActivityEntry> newActivityEntryList = new List<ActivityEntry>();
//             newActivityEntryList.Add(NewActivityEntry);

//             // create UserActivity userActivityToUpdate
//             UserActivity userActivityToUpdate = new UserActivity(
//                 Username,
//                 Month,
//                 Year,
//                 false,
//                 newActivityEntryList,
//                 emptyAcceptedActivityList
//             );

//             // check if such file exists
//             if (File.Exists(FilePath))
//             {
//                 // if yes - read it, add new activity entry and save it
//                 UserActivityReader uar = new UserActivityReader();
//                 userActivityToUpdate = uar.UserActivities.Find(userActivity =>
//                     userActivity.Name == Username &&
//                     userActivity.Month == Month &&
//                     userActivity.Year == Year
//                 );

//                 userActivityToUpdate.Entries.Add(NewActivityEntry);
//             }
//             WriteToFile(userActivityToUpdate, false);
//         }

//         public void DeleteActivityEntry(string Date, string Code, int Time, UserActivity activityToModify)
//         {
//             // find activity entry to remove
//             ActivityEntry entryToDelete = activityToModify.Entries.Find(entry =>
//                 entry.Date == Date &&
//                 entry.Code == Code &&
//                 entry.Time == Time
//             );
//             activityToModify.Entries.Remove(entryToDelete);
//             WriteToFile(activityToModify, false);
//         }

//         public UserActivity CreateNewUserActivityFile()
//         {
//             // create empty List<AcceptedActivity>
//             List<AcceptedActivity> emptyAcceptedActivityList = new List<AcceptedActivity>();

//             // create empty List<ActivityEntry>
//             List<ActivityEntry> emptyActivityEntryList = new List<ActivityEntry>();

//             // create new UserActivity
//             UserActivity newUserActivity = new UserActivity(
//                 Username,
//                 Month,
//                 Year,
//                 false,
//                 emptyActivityEntryList,
//                 emptyAcceptedActivityList
//             );

//             WriteToFile(newUserActivity, false);
//             return newUserActivity;
//         }

//         public void FreezeUserActivity()
//         {
//             UserActivityReader uar = new UserActivityReader();
//             UserActivity userActivityToFreeze = uar.UserActivities.Find(userActivity =>
//                 userActivity.Name == Username &&
//                 userActivity.Month == Month &&
//                 userActivity.Year == Year
//             );

//             userActivityToFreeze.Frozen = true;

//             WriteToFile(userActivityToFreeze, true);
//         }

//         public void SetAccepted(string code, int acceptedTime)
//         {
//             UserActivityReader uar = new UserActivityReader();
//             UserActivity userActivityToSetAccepted = uar.UserActivities.Find(userActivity =>
//                 userActivity.Name == Username &&
//                 userActivity.Month == Month &&
//                 userActivity.Year == Year
//             );

//             if (userActivityToSetAccepted.AcceptedActivities.Any(acceptedActivity => acceptedActivity.Code == code))
//             {
//                 AcceptedActivity acceptedActivityToUpdate = userActivityToSetAccepted.AcceptedActivities.Find(acceptedActivity => acceptedActivity.Code == code);
//                 acceptedActivityToUpdate.Time = acceptedTime;
//                 WriteToFile(userActivityToSetAccepted, true);
//             }
//             else
//             {
//                 AcceptedActivity newAcceptedActivity = new AcceptedActivity(code, acceptedTime);
//                 userActivityToSetAccepted.AcceptedActivities.Add(newAcceptedActivity);
//                 WriteToFile(userActivityToSetAccepted, true);
//             }
//         }

//         public void SetAcceptedToZeroIfUndefined(string code)
//         {
//             UserActivityReader uar = new UserActivityReader();
//             UserActivity userActivityToSetAccepted = uar.UserActivities.Find(userActivity =>
//                 userActivity.Name == Username &&
//                 userActivity.Month == Month &&
//                 userActivity.Year == Year
//             );

//             if (!userActivityToSetAccepted.AcceptedActivities.Any(acceptedActivity => acceptedActivity.Code == code))
//             {
//                 AcceptedActivity newAcceptedActivity = new AcceptedActivity(code, 0);
//                 userActivityToSetAccepted.AcceptedActivities.Add(newAcceptedActivity);
//                 WriteToFile(userActivityToSetAccepted, true);
//             }
//         }

//         public void WriteToFile(UserActivity userActivityToUpdate, bool shouldBeFrozen)
//         {
//             FileData data = new FileData();

//             data.name = Username;
//             data.month = Month;
//             data.year = Year;
//             data.frozen = shouldBeFrozen;

//             data.entries = userActivityToUpdate.Entries.Select(entry =>
//                 ActivityEntryForFile.FromActivityEntry(entry)).ToList();

//             data.accepted = userActivityToUpdate.AcceptedActivities.Select(accepted =>
//                 AcceptedActivityEntryForFile.FromAcceptedActivity(accepted)).ToList();

//             string json = JsonConvert.SerializeObject(data);

//             StreamWriter r = new StreamWriter(FilePath);
//             r.Write(json);
//             r.Close();
//         }

//         private class FileData
//         {
//             public string name { get; set; }
//             public int month { get; set; }
//             public int year { get; set; }
//             public bool frozen { get; set; }
//             public List<ActivityEntryForFile> entries { get; set; }
//             public List<AcceptedActivityEntryForFile> accepted { get; set; }

//             public FileData()
//             {
//                 name = "";
//                 month = 1;
//                 year = 2021;
//                 frozen = false;
//                 entries = new List<ActivityEntryForFile>();
//                 accepted = new List<AcceptedActivityEntryForFile>();
//             }
//         }

//         private class ActivityEntryForFile
//         {
//             public string date;
//             public string code;
//             public string subcode;
//             public int time;
//             public string description;

//             private ActivityEntryForFile(string dateData, string codeData, string subcodeData, int timeData, string descriptionData)
//             {
//                 date = dateData;
//                 code = codeData;
//                 subcode = subcodeData;
//                 time = timeData;
//                 description = descriptionData;
//             }

//             public static ActivityEntryForFile FromActivityEntry(ActivityEntry activityEntry)
//             {
//                 return new ActivityEntryForFile(
//                     activityEntry.Date,
//                     activityEntry.Code,
//                     activityEntry.Subcode,
//                     activityEntry.Time,
//                     activityEntry.Description
//                 );
//             }
//         }

//         private class AcceptedActivityEntryForFile
//         {
//             public string code;
//             public int time;

//             private AcceptedActivityEntryForFile(string codeData, int timeData)
//             {
//                 code = codeData;
//                 time = timeData;
//             }

//             public static AcceptedActivityEntryForFile FromAcceptedActivity(AcceptedActivity acceptedActivity)
//             {
//                 return new AcceptedActivityEntryForFile(
//                     acceptedActivity.Code,
//                     acceptedActivity.Time
//                 );
//             }
//         }
//     }
// }
