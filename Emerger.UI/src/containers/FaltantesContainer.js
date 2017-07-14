import React, { Component } from 'react';
import Nav from '../components/Nav';
import Faltantes from '../components/Faltantes';
import withAuth from  '../utils/withAuth';

class FaltantesContainer extends Component {

   render() {
     return (
       <div>
            <Nav properties={this.props}/>
            <Faltantes />
       </div>
     )
   }
}

export default withAuth(FaltantesContainer) 