class DataBase {
  constructor() {
    this._fs = require('fs');

    this._dataDir = './data';
    this._usersFilename = `${this._dataDir}/users.json`;
    this._projectsFilename = `${this._dataDir}/projects.json`;
    this._activitiesFilename = `${this._dataDir}/activities.json`;
    this._monthsFilename = `${this._dataDir}/months.json`;
  }

  // USERS
  getUsers() {
    return JSON.parse(this._readFromFile(this._usersFilename));
  }

  createUser(newUser) {
    const users = this.getUsers();
    users.push(newUser);
    this._writeToFile(this._usersFilename, users);
    return newUser;
  }

  // PROJECTS
  getProjects() {
    return JSON.parse(this._readFromFile(this._projectsFilename));
  }

  getProjectMonthDetails(user, code, month, year) {
    // find project
    let allProjects = this.getProjects();
    let project = allProjects.find((p) => p.code == code);

    // init
    let allUsers = this.getUsers();
    let activitiesByUser = {};
    let declaredTimeByUser = {};
    let acceptedTimeByUser = {};
    let isMonthFrozenByUser = {};
    let totalTime = 0;
    let totalTimeAccepted = 0;

    for (const u of allUsers) {
      activitiesByUser[u.username] = [];
      declaredTimeByUser[u.username] = 0;
      acceptedTimeByUser[u.username] = 0;
      isMonthFrozenByUser[u.username] = false;
    }

    // activities
    let allActivities = this.getActivities();
    for (const entry of allActivities) {
      if (entry.code == code && entry.month == month && entry.year == year) {
        activitiesByUser[entry.user].push(entry);
        declaredTimeByUser[entry.user] += parseInt(entry.time);
        totalTime += parseInt(entry.time);
      }
    }

    // month info
    let allMonths = this.getMonths();
    for (const m of allMonths) {
      if (m.month == month && m.year == year) {
        isMonthFrozenByUser[m.user] = m.frozen;
        let projectInfo = m.projectInfo.find((p) => p.code == code);
        acceptedTimeByUser[m.user] = projectInfo.accepted;
        totalTimeAccepted += parseInt(projectInfo.accepted);
      }
    }

    let activitiesToSend = {};
    if (project.manager == user) {
      activitiesToSend = activitiesByUser;
    } else {
      activitiesToSend[user] = activitiesByUser[user];
    }

    let data = {
      project: project,
      activities: activitiesToSend,
      declaredTimeByUser: declaredTimeByUser,
      acceptedTimeByUser: acceptedTimeByUser,
      isMonthFrozenByUser: isMonthFrozenByUser,
      totalTime: totalTime,
      totalTimeAccepted: totalTimeAccepted,
    };

    return data;
  }

  getProjectAllTimeDetails(code) {
    // find project
    let allProjects = this.getProjects();
    let project = allProjects.find((p) => p.code == code);

    // init
    let allUsers = this.getUsers();
    let activitiesByUser = {};
    let declaredTimeByUser = {};
    let totalTime = 0;
    let totalTimeAccepted = 0;

    for (const u of allUsers) {
      activitiesByUser[u.username] = [];
      declaredTimeByUser[u.username] = 0;
    }

    // activities
    let allActivities = this.getActivities();
    for (const entry of allActivities) {
      if (entry.code == code) {
        activitiesByUser[entry.user].push(entry);
        declaredTimeByUser[entry.user] += parseInt(entry.time);
        totalTime += parseInt(entry.time);
      }
    }

    // month info
    let allMonths = this.getMonths();
    for (const m of allMonths) {
      let projectInfo = m.projectInfo.find((p) => p.code == code);
      totalTimeAccepted += parseInt(projectInfo.accepted);
    }

    let data = {
      project: project,
      activities: activitiesByUser,
      declaredTimeByUser: declaredTimeByUser,
      totalTime: totalTime,
      totalTimeAccepted: totalTimeAccepted,
    };

    return data;
  }

  // ACTIVITIES
  getActivities() {
    return JSON.parse(this._readFromFile(this._activitiesFilename));
  }

  getOneUserActivities(data) {
    let allActivities = this.getActivities();
    return allActivities.filter((entry) => {
      return (
        entry.user === data.username &&
        entry.month == data.month &&
        entry.year == data.year
      );
    });
  }

  addActivity(user, day, month, year, code, subcode, time, description) {
    let allActivities = this.getActivities();
    let newActivity = {
      id: new Date().getTime(),
      user: user,
      day: day,
      month: month,
      year: year,
      code: code,
      subcode: subcode,
      time: time,
      description: description,
    };
    allActivities.push(newActivity);
    this._writeToFile(this._activitiesFilename, allActivities);
    return newActivity;
  }

  deleteActivity(id) {
    let allActivities = this.getActivities();
    let updatedActivities = [];
    let deletedActivity;
    for (const entry of allActivities) {
      if (entry.id !== id) updatedActivities.push(entry);
      else deletedActivity = entry;
    }
    this._writeToFile(this._activitiesFilename, updatedActivities);
    return deletedActivity;
  }

  // MONTHS
  getMonths() {
    return JSON.parse(this._readFromFile(this._monthsFilename));
  }

  getOneUserMonth(user, month, year) {
    let allMonths = this.getMonths();
    let userMonth = allMonths.find(
      (m) => m.user === user && m.month == month && m.year == year
    );
    if (userMonth) {
      return userMonth;
    } else {
      userMonth = {
        month: month,
        year: year,
        user: user,
        frozen: false,
        projectInfo: [
          { code: 1, accepted: 0 },
          { code: 2, accepted: 0 },
          { code: 3, accepted: 0 },
        ],
      };

      allMonths.push(userMonth);
      this._writeToFile(this._monthsFilename, allMonths);
      return userMonth;
    }
  }

  freezeOneUserMonth(user, month, year) {
    let allMonths = this.getMonths();
    let userMonth = allMonths.find(
      (m) => m.user === user && m.month == month && m.year == year
    );
    userMonth.frozen = true;
    this._writeToFile(this._monthsFilename, allMonths);
    return userMonth;
  }

  setAccepted(user, code, month, year, value) {
    let allMonths = this.getMonths();
    let userMonth = allMonths.find(
      (m) => m.user === user && m.month == month && m.year == year
    );
    for (const p of userMonth.projectInfo) {
      if (p.code == code) p.accepted = value;
    }
    this._writeToFile(this._monthsFilename, allMonths);
    return userMonth;
  }

  // READ / WRITE TO FILE
  _writeToFile(filename, objects) {
    this._fs.writeFile(filename, JSON.stringify(objects), () => {});
  }
  _readFromFile(filename) {
    return this._fs.readFileSync(filename, 'utf8');
  }
}

module.exports = DataBase;
