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
        userPassword: '',
        errorMessage: null
    };

    componentDidMount() {
        if (auth.loggedIn()) {
            this.props.history.push('/prestaciones');
        }
    }

    handleSubmit = (event) => {    
        event.preventDefault();
        auth.login(this.state.userName, this.state.userPassword)
        .then(res => {
            this.props.history.push('/prestaciones');
        })
        .catch(e => {
            if (e.response.status === 401) {
                this.setState({errorMessage: "Los datos ingresados son incorrectos"});
            } else {
                this.setState({errorMessage: "Contáctese con el administrador."});
            }
        });
    }

    render() {
        return (
            <div>
                <Nav properties={this.props}/>
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
                                    {
                                        this.state.errorMessage ? 
                                        <div className="alert alert-danger" role="alert">
                                            <strong>Error! </strong> {this.state.errorMessage}
                                        </div> : null

                                    }
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