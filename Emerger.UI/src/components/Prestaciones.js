import React, { Component } from 'react';
import Nav from './Nav';
import withAuth from  '../utils/withAuth';

class Prestaciones extends Component {

   render() {
     return (
       <div>
            <h2>Prestaciones</h2>
       </div>
     )
   }
}

export default withAuth(Prestaciones) 