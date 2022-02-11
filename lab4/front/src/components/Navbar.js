import React from 'react';
import { Link } from 'react-router-dom';
import { useGlobalContext } from '../context';

const Navbar = () => {
  const { month, year, user } = useGlobalContext();

  return (
    <header>
      <nav className="navbar navbar-expand-sm navbar-toggleable-sm navbar-light bg-white border-bottom box-shadow mb-3">
        <div className="container">
          <Link to="/" className="navbar-brand">
            lab4
          </Link>
          <div className="navbar-collapse collapse d-sm-inline-flex justify-content-between">
            <ul className="navbar-nav flex-grow-1">
              <li className="nav-item">
                <Link to="/" className="nav-link text-dark">
                  Home
                </Link>
              </li>
            </ul>
          </div>
        </div>
        <div className="container">
          {user !== '' && <div className="nav-item">Current user: {user}</div>}
          <div className="nav-item">Month: {month}</div>
          <div className="nav-item">Year: {year}</div>
          <Link to="/changeDate">
            <button className="btn btn-secondary">Change Date</button>
          </Link>
        </div>
      </nav>
    </header>
  );
};

export default Navbar;
