import React, { useState, useEffect } from 'react';
import { useGlobalContext } from '../context';
import AddActivityButton from './AddActivityButton';
import {
  fetchOneUserActivities,
  deleteActivity,
  getOneUserMonth,
} from '../api';

function EntriesList() {
  const [activities, setActivities] = useState([]);

  const { user, month, year, isMonthFrozen, setIsMonthFrozen } =
    useGlobalContext();

  useEffect(() => {
    fetchOneUserActivities(user, month, year).then((res) => {
      setActivities(res);
    });
  }, [user, month, year]);

  useEffect(() => {
    getOneUserMonth(user, month, year).then((res) => {
      setIsMonthFrozen(res.frozen);
    });
  }, []);

  const deleteEntry = async (id) => {
    let activitiesTemp = JSON.parse(JSON.stringify(activities));
    activitiesTemp = activitiesTemp.filter((entry) => {
      return entry.id !== id;
    });
    setActivities(activitiesTemp);

    const res = await deleteActivity(id);
    console.log(res);
  };

  return (
    <div className="activities-wrapper">
      <h5>Your current activities:</h5>
      <div className="activities-table">
        <div className="activities-table-header">
          <div className="activities-table-cell">Date</div>
          <div className="activities-table-cell">Project</div>
          <div className="activities-table-cell">Subactivity</div>
          <div className="activities-table-cell">Time</div>
          <div className="activities-table-cell">Description</div>
          <div className="activities-table-cell">Delete</div>
        </div>

        {activities.map((entry) => {
          return (
            <div className="activities-table-row" key={'entry' + entry.id}>
              <div className="activities-table-cell">
                {entry.day} / {entry.month} / {entry.year}
              </div>
              <div className="activities-table-cell">{entry.code}</div>
              <div className="activities-table-cell">{entry.subcode}</div>
              <div className="activities-table-cell">{entry.time}</div>
              <div className="activities-table-cell">{entry.description}</div>

              {isMonthFrozen ? (
                <div className="activities-table-cell"> </div>
              ) : (
                <div className="activities-table-cell">
                  <input
                    type="button"
                    value="X"
                    className="btn btn-danger"
                    style={{
                      fontSize: '14px',
                      lineHeight: '0',
                      height: '18px',
                      width: 'fit-content',
                    }}
                    onClick={() => deleteEntry(entry.id)}
                  />
                </div>
              )}
            </div>
          );
        })}
      </div>

      <AddActivityButton key={isMonthFrozen} />
    </div>
  );
}

export default EntriesList;
