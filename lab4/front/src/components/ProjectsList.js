import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import { useGlobalContext } from '../context';
import CloseButton from './CloseButton';
import { fetchProjects } from '../api';

function ProjectsList() {
  const { user, setCurrentProjectCode } = useGlobalContext();
  const [projects, setProjects] = useState([]);

  useEffect(() => {
    fetchProjects().then((res) => {
      setProjects(res);
    });
  }, []);

  const navigate = useNavigate();

  const detailsClicked = (code) => {
    setCurrentProjectCode(code);
    localStorage.setItem('currentProjectCode', code);
    navigate('/projectDetails');
  };

  return (
    <div className="projects-wrapper">
      <h5>All projects:</h5>
      <div className="projects-table">
        <div className="projects-table-header">
          <div className="projects-table-cell">Code</div>
          <div className="projects-table-cell">Manager</div>
          <div className="projects-table-cell">Name</div>
          <div className="projects-table-cell">Budget</div>
          <div className="projects-table-cell">Active</div>
          <div className="projects-table-cell">Details</div>
          <div className="projects-table-cell">Close</div>
        </div>

        {projects.map((project) => {
          return (
            <div className="projects-table-row" key={project.code}>
              <div className="projects-table-cell">{project.code}</div>
              <div className="projects-table-cell">{project.manager}</div>
              <div className="projects-table-cell">{project.name}</div>
              <div className="projects-table-cell">{project.budget}</div>
              <div className="projects-table-cell">
                {project.active ? 'Yes' : 'No'}
              </div>
              <div className="projects-table-cell">
                <input
                  type="button"
                  value="i"
                  className="btn btn-secondary"
                  onClick={() => detailsClicked(project.code)}
                  style={{
                    fontSize: '14px',
                    lineHeight: '0',
                    height: '18px',
                    width: 'fit-content',
                  }}
                />
              </div>
              <CloseButton
                user={user}
                manager={project.manager}
                active={project.active}
              />
            </div>
          );
        })}
      </div>
    </div>
  );
}

export default ProjectsList;
