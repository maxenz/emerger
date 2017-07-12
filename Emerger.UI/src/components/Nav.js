import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import '../App.css';

class Nav extends Component {

  render() {
    return (
    <div className="navbar-container">
        <nav className="navbar">
            <h1 className="navbar-brand mb-0">Extranet de Prestadores</h1>
        </nav>
    </div>
    );
  }
}

export default Nav;