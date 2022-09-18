import 'bootstrap/dist/css/bootstrap.css';
import './styles/styles.min.css';
import './styles/vendor/css/all.min.css';
import './vendor/jquery/jquery.min.js';
import './vendor/bootstrap/js/bootstrap.bundle.min.js';
import './vendor/jquery-easing/jquery.easing.min.js'
import './js/LuxuryBiker.js'
import './vendor/chart.js/Chart.min.js';
import './js/demo/chart-area-demo.js';
import './js/demo/chart-pie-demo.js';

import React from 'react';
import ReactDOM from 'react-dom';
import { BrowserRouter } from 'react-router-dom';
import App from './App';
import registerServiceWorker from './registerServiceWorker';

const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href');
const rootElement = document.getElementById('root');

ReactDOM.render(
  <BrowserRouter basename={baseUrl}>
    <App />
  </BrowserRouter>,
  rootElement);

registerServiceWorker();