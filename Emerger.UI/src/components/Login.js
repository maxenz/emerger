import React, { Component } from 'react';
import logo from '../images/logo_emerger.png';
import Nav from './Nav';
import AuthService from '../utils/AuthService';
import './../App.css';

const auth = new AuthService();

class Login extends Component {

    constructor(props) {
        super(props);
    }

    state = { 
        userName: '',
        userPassword: ''
    };

    componentDidMount() {
        if (auth.loggedIn()) {
            this.props.history.push('/about');
        }
    }

    handleSubmit = (event) => {    
        event.preventDefault();
        auth.login(this.state.userName, this.state.userPassword)
        .then(res => {
            console.log(res);
            this.props.history.push('/about');
        })
        .catch(e => console.log(e));
    }

    render() {
        return (
            <div>
                <Nav/>
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
            </div>


        );
    }
}

export default Login;