import './App.css';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import Navbar from './components/Navbar';
import Footer from './components/Footer';
import Home from './pages/Home';
import Dashboard from './pages/Dashboard';
import AddUser from './pages/AddUser';
import ChangeDate from './pages/ChangeDate';
import AddActivityEntry from './pages/AddActivityEntry';
import ProjectDetails from './pages/ProjectDetails';
import ProjectDetailsAllTime from './pages/ProjectDetailsAllTime';
import SetAcceptedTime from './pages/SetAcceptedTime';

function App() {
  return (
    <div className="App">
      <Router>
        <Navbar />
        <Routes>
          <Route path="/" element={<Home />} />
          <Route path="/addUser" element={<AddUser />} />
          <Route path="/changeDate" element={<ChangeDate />} />
          <Route path="/dashboard" element={<Dashboard />} />
          <Route path="/addActivity" element={<AddActivityEntry />} />
          <Route path="/projectDetails" element={<ProjectDetails />} />
          <Route
            path="/projectDetailsAllTime"
            element={<ProjectDetailsAllTime />}
          />
          <Route path="/setAcceptedTime" element={<SetAcceptedTime />} />
        </Routes>
        <Footer />
      </Router>
    </div>
  );
}

export default App;
