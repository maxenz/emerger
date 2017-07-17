import React, { Component } from 'react';
import Nav from '../components/Nav';
import Filters from '../components/Prestaciones/Filters';
import withAuth from  '../utils/withAuth';

class PrestacionesContainer extends Component {

   render() {
     return (
       <div>
            <Nav properties={this.props}/>
            <Filters />
       </div>
     )
   }
}

export default withAuth(PrestacionesContainer) 