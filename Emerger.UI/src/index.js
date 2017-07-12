import React from 'react';
import ReactDOM from 'react-dom';
import './index.css';
import registerServiceWorker from './registerServiceWorker';
import 'bootstrap/dist/css/bootstrap.css';
import {Route, BrowserRouter} from 'react-router-dom';
import About from './components/About';
import Login from './components/Login';
import Nav from './components/Nav';


const Root = () => {
  return (

      <BrowserRouter>
        <div>
            <Nav/>
            <Route exact path="/" component={Login}/>
            <Route path="/login" component={Login}/>
            <div className="container">
                <Route path="/about" component={About}/>
            </div>
        </div>
      </BrowserRouter>

  )
}

ReactDOM.render(<Root />, document.getElementById('root'));

registerServiceWorker();
