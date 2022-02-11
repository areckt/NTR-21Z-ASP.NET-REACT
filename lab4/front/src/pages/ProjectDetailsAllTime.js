import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useGlobalContext } from '../context';
import { getProjectAllTimeDetails } from '../api';

function ProjectDetailsAllTime() {
  const { user, currentProjectCode } = useGlobalContext();
  const [data, setData] = useState({
    activities: {},
    project: { manager: '', name: '', budget: 0 },
    totalTime: 0,
    totalTimeAccepted: 0,
  });

  useEffect(() => {
    getProjectAllTimeDetails(currentProjectCode).then((res) => {
      setData(res);
    });
  }, [currentProjectCode]);

  const navigate = useNavigate();

  let tempUsersToShow = Object.keys(data.activities) || [];
  let usersToShow = [];
  for (const user of tempUsersToShow) {
    if (data.activities[user] && data.activities[user].length > 0)
      usersToShow.push(user);
  }

  return (
    <div className="container" style={{ maxWidth: '1000px' }}>
      <h3 style={{ marginBottom: '1rem' }}>You are logged in as {user}</h3>
      <div className="main-wrapper" style={{ gridTemplateColumns: 'auto' }}>
        <div className="activities-wrapper">
          <h4>
            All activities for project{' '}
            <span style={{ fontWeight: '900' }}>{data.project.name}</span>
          </h4>
          {usersToShow.map((oneUser) => {
            return (
              <div
                className="my-container"
                key={oneUser}
                style={{ paddingLeft: '2rem', paddingRight: '2rem' }}
              >
                <h5 style={{ marginTop: '0.75rem' }}>
                  User: <span style={{ fontWeight: '600' }}>{oneUser}</span>
                </h5>
                <div
                  className="activities-table"
                  style={{ marginBottom: '0.5rem' }}
                >
                  <div className="activities-table-header">
                    <div className="activities-table-cell">Date</div>
                    <div className="activities-table-cell">Subactivity</div>
                    <div className="activities-table-cell">Time</div>
                    <div className="activities-table-cell">Description</div>
                  </div>

                  {data.activities[oneUser].map((entry) => {
                    return (
                      <div
                        key={'entry' + entry.id}
                        className="activities-table-row"
                      >
                        <div className="activities-table-cell">
                          {entry.day} / {entry.month} / {entry.year}
                        </div>
                        <div className="activities-table-cell">
                          {entry.subcode}
                        </div>
                        <div className="activities-table-cell">
                          {entry.time}
                        </div>
                        <div className="activities-table-cell">
                          {entry.description}
                        </div>
                      </div>
                    );
                  })}
                </div>
              </div>
            );
          })}

          <div className="my-container" style={{ paddingLeft: '2rem' }}>
            <h4 style={{ marginTop: '1rem' }}>All-time project stats</h4>
            <h5>Project Budget: {data.project.budget}</h5>
            <h5>Total time declared: {data.totalTime}</h5>
            <h5>Total accepted time: {data.totalTimeAccepted}</h5>
            <h5>Balance: {data.project.budget - data.totalTimeAccepted}</h5>
          </div>
          <input
            type="button"
            value="Back"
            className="btn btn-secondary"
            style={{ marginTop: '0.5rem', width: 'fit-content' }}
            onClick={() => navigate('/dashboard')}
          />
        </div>
      </div>
    </div>
  );
}

export default ProjectDetailsAllTime;
