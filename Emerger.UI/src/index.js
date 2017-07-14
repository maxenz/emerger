import React from 'react';
import ReactDOM from 'react-dom';
import registerServiceWorker from './registerServiceWorker';
import {Route, Switch, BrowserRouter as Router} from 'react-router-dom';
import About from './components/About';
import Login from './components/Login';
import NotFound from './components/NotFound/NotFound';
import Unauthorized from './components/Unauthorized/Unauthorized';
import PrestacionesContainer from './containers/PrestacionesContainer';
import ResumenContainer from './containers/ResumenContainer';
import FaltantesContainer from './containers/FaltantesContainer';
import 'bootstrap/dist/css/bootstrap.css';
import './fonts/css/font-awesome.css';

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

registerServiceWorker();
