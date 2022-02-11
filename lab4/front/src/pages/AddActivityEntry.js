import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { fetchProjects, addActivity } from '../api';
import { useGlobalContext } from '../context';

function AddActivityEntry() {
  const [inputs, setInputs] = useState({ day: 1, code: 1, time: 0 });
  const [projects, setProjects] = useState([]);

  const { user, month, year } = useGlobalContext();

  const navigate = useNavigate();

  useEffect(() => {
    fetchProjects().then((res) => {
      setProjects(res);
    });
  }, []);

  const handleChange = (e) => {
    const name = e.target.name;
    const value = e.target.value;
    setInputs((values) => ({ ...values, [name]: value }));
  };

  const handleSubmit = async (e) => {
    e.preventDefault();
    const res = await addActivity({
      ...inputs,
      user: user,
      month: month,
      year: year,
    });
    console.log(res);
    setTimeout(() => {
      navigate('/dashboard');
    }, 350);
  };

  return (
    <div className="container">
      <h4>Create new activity</h4>
      <div className="container" style={{ maxWidth: '400px' }}>
        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <label htmlFor="day">Day of month</label>
            <input
              type="number"
              value={inputs.day || 1}
              onChange={handleChange}
              min="1"
              max="31"
              name="day"
              className="form-control"
            />
          </div>
          <div className="form-group">
            <label htmlFor="code">Project</label>
            <select
              name="code"
              value={inputs.code || 0}
              onChange={handleChange}
              className="form-control"
            >
              {projects.map((project) => {
                return (
                  <option key={project.code} value={project.code}>
                    {project.name}
                  </option>
                );
              })}
            </select>
          </div>
          <div className="form-group">
            <label htmlFor="subcode">Subactivity (optional)</label>
            <input
              name="subcode"
              value={inputs.subcode || ''}
              onChange={handleChange}
              className="form-control"
            />
          </div>
          <div className="form-group">
            <label htmlFor="time">Declared Time</label>
            <input
              type="number"
              onChange={handleChange}
              value={inputs.time || 0}
              min="0"
              name="time"
              className="form-control"
            />
          </div>
          <div className="form-group">
            <label htmlFor="description">Description (optional)</label>
            <input
              name="description"
              onChange={handleChange}
              value={inputs.description || ''}
              className="form-control"
            />
          </div>
          <div className="form-group">
            <input type="submit" value="Add" className="btn btn-primary" />
          </div>
        </form>
      </div>
    </div>
  );
}

export default AddActivityEntry;
