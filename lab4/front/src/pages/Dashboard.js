import React from 'react';
import { useGlobalContext } from '../context';
import ProjectsList from '../components/ProjectsList';
import EntriesList from '../components/EntriesList';

function Dashboard() {
  const { user } = useGlobalContext();
  return (
    <div className="container" style={{ maxWidth: '100vw', minHeight: '90vh' }}>
      <h3 style={{ marginBottom: '1rem' }}>You are logged in as {user}</h3>
      <div className="main-wrapper">
        <ProjectsList />
        <EntriesList />
      </div>
    </div>
  );
}

export default Dashboard;
