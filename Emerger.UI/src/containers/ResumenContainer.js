import React, { Component } from 'react';
import Nav from '../components/Nav';
import Resumen from '../components/Resumen';
import withAuth from  '../utils/withAuth';

class ResumenContainer extends Component {

   render() {
     return (
       <div>
            <Nav properties={this.props}/>
            <Resumen />
       </div>
     )
   }
}

export default withAuth(ResumenContainer) 