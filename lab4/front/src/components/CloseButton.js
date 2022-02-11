import React from 'react';

function CloseButton({ user, manager, active }) {
  let CButton;
  if (user === manager && active) {
    CButton = (
      <div className="projects-table-cell">
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
        />
      </div>
    );
  } else if (user === manager && !active) {
    CButton = (
      <div className="projects-table-cell">
        <input
          type="button"
          value="Closed"
          className="btn disabled"
          style={{
            fontSize: '14px',
            lineHeight: '0',
            height: '18px',
            width: 'fit-content',
          }}
        />
      </div>
    );
  } else {
    CButton = <div className="projects-table-cell"> </div>;
  }

  return CButton;
}

export default CloseButton;
