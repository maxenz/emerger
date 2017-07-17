import React, {Component} from 'react'
import AuthService from './AuthService';

export default function withAuth(AuthComponent) {
    const Auth = new AuthService();
    return class Authenticated extends Component {
      
      componentDidMount () {
        if (!Auth.loggedIn()) {
          this.props.history.push('/unauthorized');
        }
      }

      render() {
        return (
          <div>    
            <AuthComponent {...this.props}  auth={Auth} />
          </div>
        )
      }
    }
}