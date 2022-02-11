import React, { useEffect, useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useGlobalContext } from '../context';
import { getProjectMonthDetails } from '../api';

function ProjectDetails() {
  const { user, currentProjectCode, month, year, setUserToChangeAcceptedTime } =
    useGlobalContext();

  const [data, setData] = useState({
    activities: {},
    project: { manager: '', name: '' },
  });

  useEffect(() => {
    getProjectMonthDetails(user, currentProjectCode, month, year).then(
      (res) => {
        setData(res);
      }
    );
  }, [user, currentProjectCode, month, year]);

  const navigate = useNavigate();

  let tempUsersToShow = Object.keys(data.activities) || [];
  let usersToShow = [];
  for (const user of tempUsersToShow) {
    if (data.activities[user] && data.activities[user].length > 0)
      usersToShow.push(user);
  }

  const handleSetAcceptedTimeClick = (u) => {
    setUserToChangeAcceptedTime(u);
    navigate('/setAcceptedTime');
  };

  return (
    <div className="container" style={{ maxWidth: '1000px' }}>
      <h3 style={{ marginBottom: '1rem' }}>You are logged in as {user}</h3>
      <div className="main-wrapper" style={{ gridTemplateColumns: 'auto' }}>
        <div className="activities-wrapper">
          <h4>
            {data.project.manager === user ? 'All' : 'Your'} activities for
            project{' '}
            <span style={{ fontWeight: '900' }}>{data.project.name}</span> this
            month:
          </h4>
          {usersToShow.length === 0 && <h3>No activity to show!</h3>}
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
                <h5>Declared Time: {data.declaredTimeByUser[oneUser]}</h5>

                {data.isMonthFrozenByUser[oneUser] ? (
                  <>
                    <h5>Accepted Time: {data.acceptedTimeByUser[oneUser]}</h5>
                    {data.project.manager === user && (
                      <input
                        type="button"
                        value="Set accepted time"
                        className="btn btn-primary"
                        style={{
                          margin: '0.25rem 0 2rem',
                          width: 'fit-content',
                        }}
                        onClick={() => handleSetAcceptedTimeClick(oneUser)}
                      />
                    )}
                  </>
                ) : (
                  <>
                    {data.project.manager === user && (
                      <h6 style={{ marginBottom: '1rem' }}>
                        You can't set Accepted Time until this user has frozen
                        their entries for this month
                      </h6>
                    )}
                  </>
                )}
              </div>
            );
          })}

          {data.project.manager === user && (
            <>
              <div className="my-container" style={{ paddingLeft: '2rem' }}>
                <h4 style={{ marginTop: '1rem' }}>
                  Project stats for this month
                </h4>
                <h5>Total time declared: {data.totalTime}</h5>
                <h5>Total accepted time: {data.totalTimeAccepted}</h5>
              </div>
              <input
                type="button"
                value="Show All Time Details"
                className="btn btn-secondary"
                style={{ marginTop: '0.5rem', width: 'fit-content' }}
                onClick={() => navigate('/projectDetailsAllTime')}
              />
            </>
          )}
          <input
            type="button"
            value="Back"
            className="btn btn-secondary"
            style={{ margin: '0.5rem', width: 'fit-content', display: 'block' }}
            onClick={() => navigate('/dashboard')}
          />
        </div>
      </div>
    </div>
  );
}

export default ProjectDetails;
