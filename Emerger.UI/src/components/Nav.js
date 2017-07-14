import React, {Component} from 'react';
import AuthService from '../utils/AuthService';
import {Link} from 'react-router-dom';
import '../App.css';

const auth = new AuthService();

class Nav extends Component {

    getNavItemClass = (path) => {
        var _class = "nav-item ";
        _class += path === this.props.properties.location.pathname ? "active " : "";
        _class += !auth.loggedIn() ? "not-visible" : "";
        return _class;
    }

    logout = () => {
        auth.logout();
        this.props.properties.history.push("/");
    }

    render(){

        return (
            <div className="navbar-container">
                <nav className="navbar navbar-inverse navbar-toggleable-sm justify-content-between">
                    <button className="navbar-toggler navbar-toggler-right" type="button" data-toggle="collapse" data-target=".dual-nav">
                        <span className="navbar-toggler-icon"></span>
                    </button>
                    <div className="navbar-collapse collapse dual-nav">
                        <ul className="navbar-nav">
                            <li className="nav-item active">
                                <h1 className="navbar-brand mb-0" href="#">Extranet de Prestadores</h1>
                            </li>
                            <li className={this.getNavItemClass("/prestaciones")}>
                                <Link to="/prestaciones" className="nav-link">Prestaciones</Link>
                            </li>
                            <li className={this.getNavItemClass("/faltantes")}>
                                <Link to="/faltantes" className="nav-link">Faltantes</Link>
                            </li>
                            <li className={this.getNavItemClass("/resumen")}>
                                <Link to="/resumen" className="nav-link">Resumen</Link>
                            </li>
                        </ul>
                    </div>
                    {(() => {
                        if (auth.loggedIn()) {
                            return (
                                <div className="navbar-collapse collapse dual-nav">
                                    <ul className="nav navbar-nav ml-auto">
                                        <li className="nav-item nav-profile-wrapper">
                                            <span>
                                                <i className="fa fa-user"></i> {auth.getProfile().name}
                                            </span>
                                        </li>
                                        <li className="nav-item margin-top-6">                                
                                            <button className="btn btn-danger btn-sm" onClick={this.logout}>
                                                <i className="fa fa-power-off fa-lg margin-right-5"></i>
                                                Salir
                                            </button>
                                        </li>
                                    </ul>
                                </div>
                            )
                        }
                    })()}
                </nav>        
            </div>
        )
    }
  
}

export default Nav;