import React, { Component } from 'react';
import axios from 'axios';
import './../App.css';
import logo from '../images/logo_emerger.png';
import {login} from './../utils/AuthService';

class Login extends Component {

    state = { 
        userName: '',
        userPassword: ''
    };

    handleSubmit = (event) => {    
        event.preventDefault();
        login(this.state.userName, this.state.userPassword)
            .then(function(data){
                console.log(data);
            })
            .catch(function(xhr, status, error){
                console.log(error);
                console.log(xhr);
                console.log(status);
            })
    }

  render() {
    return (
        <div className="outer">
            <div className="middle">
                <div className="inner">                   
                    <div className="login-block">
                        <div className="text-centered">
                            <img src={logo} style={{width:'400px'}} alt="Emerger"  />
                        </div>
                        <form onSubmit={this.handleSubmit}>
                            <input type="text"
                                id="username"
                                onChange={(event) => this.setState({ userName: event.target.value })}
                                placeholder="Usuario"
                                required
                            />
                            <input type="password"
                                id="password"
                                onChange={(event) => this.setState({ userPassword: event.target.value })}
                                placeholder="Contraseña"
                                required
                            />
                            <button type="submit">Ingresar</button>
                        </form>
                    </div>
                </div>
            </div>
        </div>


    );
  }
}

export default Login;