import fetch from 'node-fetch';

const domainPath = 'http://localhost:3001';

const usersPath = `${domainPath}/users`;
const projectsPath = `${domainPath}/projects`;
const activitiesPath = `${domainPath}/activities`;
const monthsPath = `${domainPath}/months`;

// USERS
export const fetchUsers = async () => {
  return fetch(usersPath, { method: 'GET' })
    .then((resp) => {
      return resp.json();
    })
    .then((data) => {
      return data;
    });
};

export const addUser = async (username) => {
  return fetch(`${usersPath}`, {
    method: 'POST',
    body: JSON.stringify({ username: username }),
    headers: { 'Content-Type': 'application/json' },
  })
    .then((resp) => {
      return resp.json();
    })
    .then((data) => {
      return data;
    });
};

// PROJECTS
export const fetchProjects = async () => {
  return fetch(projectsPath, { method: 'GET' })
    .then((resp) => {
      return resp.json();
    })
    .then((data) => {
      return data;
    });
};

export const getProjectMonthDetails = async (user, code, month, year) => {
  return fetch(`${projectsPath}/details`, {
    method: 'POST',
    body: JSON.stringify({
      user: user,
      code: code,
      month: month,
      year: year,
    }),
    headers: { 'Content-Type': 'application/json' },
  })
    .then((resp) => {
      return resp.json();
    })
    .then((data) => {
      return data;
    });
};

export const getProjectAllTimeDetails = async (code) => {
  return fetch(`${projectsPath}/allTimeDetails`, {
    method: 'POST',
    body: JSON.stringify({ code: code }),
    headers: { 'Content-Type': 'application/json' },
  })
    .then((resp) => {
      return resp.json();
    })
    .then((data) => {
      return data;
    });
};

// ACTIVITIES
export const fetchActivities = async () => {
  return fetch(activitiesPath, { method: 'GET' })
    .then((resp) => {
      return resp.json();
    })
    .then((data) => {
      return data;
    });
};

export const fetchOneUserActivities = async (username, month, year) => {
  return fetch(`${activitiesPath}/oneUser`, {
    method: 'POST',
    body: JSON.stringify({ username: username, month: month, year: year }),
    headers: { 'Content-Type': 'application/json' },
  })
    .then((resp) => {
      return resp.json();
    })
    .then((data) => {
      return data;
    });
};

export const addActivity = async (data) => {
  return fetch(`${activitiesPath}/add`, {
    method: 'POST',
    body: JSON.stringify(data),
    headers: { 'Content-Type': 'application/json' },
  })
    .then((resp) => {
      return resp.json();
    })
    .then((data) => {
      return data;
    });
};

export const deleteActivity = async (id) => {
  return fetch(`${activitiesPath}/delete`, {
    method: 'POST',
    body: JSON.stringify({ id: id }),
    headers: { 'Content-Type': 'application/json' },
  })
    .then((resp) => {
      return resp.json();
    })
    .then((data) => {
      return data;
    });
};

// MONTHS
export const getOneUserMonth = async (user, month, year) => {
  return fetch(`${monthsPath}`, {
    method: 'POST',
    body: JSON.stringify({ user, month, year }),
    headers: { 'Content-Type': 'application/json' },
  })
    .then((resp) => {
      return resp.json();
    })
    .then((data) => {
      return data;
    });
};

export const freezeOneUserMonth = async (user, month, year) => {
  return fetch(`${monthsPath}/freeze`, {
    method: 'POST',
    body: JSON.stringify({ user, month, year }),
    headers: { 'Content-Type': 'application/json' },
  })
    .then((resp) => {
      return resp.json();
    })
    .then((data) => {
      return data;
    });
};

export const setAcceptedTime = async (user, code, month, year, value) => {
  return fetch(`${monthsPath}/setAccepted`, {
    method: 'POST',
    body: JSON.stringify({ user, code, month, year, value }),
    headers: { 'Content-Type': 'application/json' },
  })
    .then((resp) => {
      return resp.json();
    })
    .then((data) => {
      return data;
    });
};
