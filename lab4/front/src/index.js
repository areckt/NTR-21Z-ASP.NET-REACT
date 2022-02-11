import React from 'react';
import ReactDOM from 'react-dom';
import App from './App';
import './index.css';
import './custom-bootstrap.css';
import 'bootstrap/dist/css/bootstrap.min.css';
import reportWebVitals from './reportWebVitals';
import { AppProvider } from './context';

ReactDOM.render(
  <React.StrictMode>
    <AppProvider>
      <App />
    </AppProvider>
  </React.StrictMode>,
  document.getElementById('root')
);
reportWebVitals();
