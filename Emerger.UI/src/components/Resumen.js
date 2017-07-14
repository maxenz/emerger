import React, { Component } from 'react';
import Nav from './Nav';
import withAuth from  '../utils/withAuth';

class Resumen extends Component {

   render() {
     return (
       <div>
            <h2>Resumen</h2>
       </div>
     )
   }
}

export default withAuth(Resumen) 