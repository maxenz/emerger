import React, { Component } from 'react';
import Nav from '../components/Nav';
import Prestaciones from '../components/Prestaciones';
import withAuth from  '../utils/withAuth';

class PrestacionesContainer extends Component {

   render() {
     return (
       <div>
            <Nav properties={this.props}/>
            <Prestaciones />
       </div>
     )
   }
}

export default withAuth(PrestacionesContainer) 