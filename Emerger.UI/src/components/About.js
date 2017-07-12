import React, { Component } from 'react';
import { Link } from 'react-router';
import {login, isLoggedIn} from './../utils/AuthService';

class About extends Component {

  render() {

    return (
        <div>
            { isLoggedIn() ? <h3>Logueado perrito</h3> : <h1>SIN ACCESO</h1>}
        </div>
    );
  }
}

export default About;