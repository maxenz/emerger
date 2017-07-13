import React from 'react';
import ReactDOM from 'react-dom';
import registerServiceWorker from './registerServiceWorker';
import {Route, Switch, BrowserRouter as Router} from 'react-router-dom';
import About from './components/About';
import Login from './components/Login';
import NotFound from './components/NotFound/NotFound';
import Unauthorized from './components/Unauthorized/Unauthorized';
import 'bootstrap/dist/css/bootstrap.css';
import './fonts/css/font-awesome.css';
import './index.css';


const Root = () => {
  return (
    <Router>
        <div>
            <Switch>
                <Route exact path="/" component={Login}/>
                <Route exact path="/about" component={About}/>
                <Route exact path="/unauthorized" component={Unauthorized}/>
                <Route component={NotFound} />
            </Switch>
        </div>
    </Router>

  )
}

ReactDOM.render(<Root />, document.getElementById('root'));

registerServiceWorker();
