import React from 'react';
import { Link } from 'react-router-dom';
import { useGlobalContext } from '../context';
import { freezeOneUserMonth } from '../api';

function AddActivityButton() {
  let AAButton;

  const { user, month, year, isMonthFrozen, setIsMonthFrozen } =
    useGlobalContext();

  const freezeMonth = async (e) => {
    e.preventDefault();
    setIsMonthFrozen(true);
    const result = await freezeOneUserMonth(user, month, year);
    console.log(result);
  };

  if (!isMonthFrozen) {
    AAButton = (
      <>
        <Link to="/addActivity" style={{ display: 'block' }}>
          + Add new activity
        </Link>
        <input
          type="button"
          value="Freeze entries for this month"
          className="btn btn-danger"
          onClick={freezeMonth}
          style={{ marginTop: '0.5rem', width: 'fit-content' }}
        />
      </>
    );
  } else {
    AAButton = (
      <button
        className="btn disabled"
        style={{ marginTop: '0.5rem', width: 'fit-content' }}
      >
        Month frozen
      </button>
    );
  }

  return AAButton;
}

export default AddActivityButton;
