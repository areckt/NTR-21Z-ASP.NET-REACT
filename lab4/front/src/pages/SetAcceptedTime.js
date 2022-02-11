import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { useGlobalContext } from '../context';
import { setAcceptedTime } from '../api';

function SetAcceptedTime() {
  const { month, year, userToChangeAcceptedTime, currentProjectCode } =
    useGlobalContext();
  const [accepted, setAccepted] = useState(0);

  const navigate = useNavigate();

  const handleChange = (e) => setAccepted(e.target.value);

  const handleSubmit = async (e) => {
    e.preventDefault();

    const res = await setAcceptedTime(
      userToChangeAcceptedTime,
      currentProjectCode,
      month,
      year,
      accepted
    );
    console.log(res);

    setTimeout(() => {
      navigate(-1);
    }, 350);
  };

  return (
    <div className="container">
      <h4>Set Accepted Time</h4>
      <div className="constainer">
        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <label for="accepted">Time</label>
            <input
              type="number"
              value={accepted}
              onChange={handleChange}
              name="accepted"
              className="form-control"
            />
          </div>
          <div className="form-group">
            <input type="submit" value="Set" className="btn btn-primary" />
          </div>
        </form>
      </div>
    </div>
  );
}

export default SetAcceptedTime;
