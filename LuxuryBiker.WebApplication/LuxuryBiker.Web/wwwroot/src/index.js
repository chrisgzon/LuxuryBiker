
import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './App';

import './styles/styles.css';
import './styles/vendor/css/all.min.css';

import './vendor/jquery-easing/jquery.easing.min.js';
import './vendor/bootstrap/js/bootstrap.bundle.min.js';

const root = ReactDOM.createRoot(document.getElementById('root'))

root.render(
    <React.StrictMode>
        <App />
    </React.StrictMode>
);