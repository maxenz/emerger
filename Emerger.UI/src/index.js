import React from 'react';
import ReactDOM from 'react-dom';
import registerServiceWorker from './registerServiceWorker';
import {Route, Switch, BrowserRouter as Router} from 'react-router-dom';
import Login from './components/Login';
import NotFound from './components/NotFound/NotFound';
import Unauthorized from './components/Unauthorized/Unauthorized';
import PrestacionesContainer from './containers/PrestacionesContainer';
import ResumenContainer from './containers/ResumenContainer';
import FaltantesContainer from './containers/FaltantesContainer';
import AuthService from './utils/AuthService';
import 'bootstrap/dist/css/bootstrap.css';
import 'react-select/dist/react-select.css';
import './fonts/css/font-awesome.css';
import 'react-dates/lib/css/_datepicker.css';
import 'react-table/react-table.css';
import './App.css';
import axios from 'axios';
import moment from 'moment';

const auth = new AuthService();
axios.defaults.baseURL = 'http://localhost/Emerger.WebAPI/api';
axios.defaults.headers.common['Authorization'] = 'Bearer ' + auth.getJwtToken();
axios.defaults.headers.common['User'] = auth.getProfile().id;

moment.locale('es');

const Root = () => {
  return (
    <Router>
        <div>
            <Switch>
                <Route exact path="/" component={Login}/>
                <Route exact path="/prestaciones" component={PrestacionesContainer}/>
                <Route exact path="/faltantes" component={FaltantesContainer}/>
                <Route exact path="/resumen" component={ResumenContainer}/>
                <Route exact path="/unauthorized" component={Unauthorized}/>
                <Route component={NotFound} />
            </Switch>
        </div>
    </Router>

  )
}

ReactDOM.render(<Root />, document.getElementById('root'));

// --> Descomentar para produccion

//registerServiceWorker();
