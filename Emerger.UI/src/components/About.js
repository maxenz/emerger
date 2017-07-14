import React, { Component } from 'react';
import Unauthorized from './Unauthorized/Unauthorized';
import Nav from './Nav';
import withAuth from  '../utils/withAuth';

class About extends Component {

   render() {
     const user = this.props.auth.getProfile();
     return (
       <div>
          <Nav properties={this.props} />
          <div>Current user: {user.email}</div>
       </div>
     )
   }
}

export default withAuth(About) 