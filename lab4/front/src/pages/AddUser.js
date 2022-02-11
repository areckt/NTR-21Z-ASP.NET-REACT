import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { addUser } from '../api';

export default function AddUser() {
  const [username, setUsername] = useState('');

  const navigate = useNavigate();

  const handleChange = (e) => setUsername(e.target.value);

  const handleSubmit = async (e) => {
    e.preventDefault();
    const res = await addUser(username);
    setTimeout(() => {
      navigate('/');
    }, 350);
  };

  return (
    <div className="container">
      <h4>Add new user</h4>
      <div className="text-center">
        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <input
              type="text"
              value={username}
              className="form-control"
              style={{ marginTop: '10px' }}
              onChange={handleChange}
            />
            <input
              type="submit"
              value="Add"
              className="btn btn-primary"
              style={{ marginTop: '10px' }}
            />
          </div>
        </form>
      </div>
    </div>
  );
}
