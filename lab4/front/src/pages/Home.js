import React, { useEffect, useState } from 'react';
import { Link } from 'react-router-dom';
import { useNavigate } from 'react-router-dom';
import { useGlobalContext } from '../context';
import { fetchUsers } from '../api';

const Home = () => {
  const [users, setUsers] = useState([]);

  useEffect(() => {
    fetchUsers().then((res) => {
      setUsers(res);
    });
  }, []);

  const { setUser } = useGlobalContext();

  const navigate = useNavigate();

  const handleClick = (e) => {
    e.preventDefault();
    localStorage.setItem('user', e.target.value);
    setUser(e.target.value);
    navigate('/dashboard');
  };

  return (
    <div className="text-center" style={{ minHeight: '90vh' }}>
      <h1 className="display-4" style={{ marginBottom: '2rem' }}>
        Welcome to time tracker app
      </h1>

      <h3 style={{ marginBottom: '1rem' }}>Select user or create a new one.</h3>
      <div
        className="btn-group"
        style={{ maxWidth: '50ch', marginBottom: '2rem' }}
      >
        <form>
          {users.map((user) => {
            return (
              <input
                type="button"
                key={user.username}
                value={user.username}
                className="btn btn-primary"
                style={{ margin: '2px 3px' }}
                onClick={handleClick}
              />
            );
          })}
        </form>
      </div>

      <div className="container">
        <Link to="/addUser">Click here to add new user</Link>
      </div>
    </div>
  );
};

export default Home;
