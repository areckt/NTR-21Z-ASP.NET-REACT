import React, { useState, useContext } from 'react';

const AppContext = React.createContext();

const AppProvider = ({ children }) => {
  let date = new Date();
  const [user, setUser] = useState(localStorage.getItem('user') || '');
  const [month, setMonth] = useState(
    parseInt(localStorage.getItem('month')) || date.getMonth() + 1
  );
  const [year, setYear] = useState(
    parseInt(localStorage.getItem('year')) || date.getFullYear()
  );
  const [currentProjectCode, setCurrentProjectCode] = useState(
    parseInt(localStorage.getItem('currentProjectCode')) || 1
  );
  const [isMonthFrozen, setIsMonthFrozen] = useState(false);
  const [userToChangeAcceptedTime, setUserToChangeAcceptedTime] =
    useState('jasio');

  return (
    <AppContext.Provider
      value={{
        user,
        setUser,
        month,
        setMonth,
        year,
        setYear,
        currentProjectCode,
        setCurrentProjectCode,
        userToChangeAcceptedTime,
        setUserToChangeAcceptedTime,
        isMonthFrozen,
        setIsMonthFrozen,
      }}
    >
      {children}
    </AppContext.Provider>
  );
};

export const useGlobalContext = () => {
  return useContext(AppContext);
};

export { AppContext, AppProvider };
