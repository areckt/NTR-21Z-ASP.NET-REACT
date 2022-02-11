import React from 'react';
import { useNavigate } from 'react-router-dom';
import { useGlobalContext } from '../context';

export default function ChangeDate() {
  const { month, setMonth, year, setYear } = useGlobalContext();

  const navigate = useNavigate();

  const monthChange = (e) => setMonth(e.target.value);
  const yearChange = (e) => setYear(e.target.value);

  const handleClick = () => {
    localStorage.setItem('month', month);
    localStorage.setItem('year', year);
    navigate(-1);
  };

  return (
    <div className="container">
      <h4>Change Date</h4>
      <div className="text-center">
        <div className="form-group">
          <label htmlFor="Month">Month</label>
          <input
            type="number"
            min="1"
            max="12"
            name="Month"
            className="form-control"
            value={month}
            onChange={monthChange}
          />
        </div>
        <div className="form-group">
          <label htmlFor="Year">Year</label>
          <input
            type="number"
            min="2018"
            max="2050"
            name="Year"
            className="form-control"
            value={year}
            onChange={yearChange}
          />
        </div>
        <div className="form-group">
          <input
            type="button"
            value="Confirm and go back"
            className="btn btn-primary"
            onClick={handleClick}
          />
        </div>
      </div>
    </div>
  );
}
