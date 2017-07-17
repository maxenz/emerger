import React, { Component } from 'react';
import withAuth from  '../utils/withAuth';

class Faltantes extends Component {

   render() {
     return (
       <div>
            <h2>Faltantes</h2>
       </div>
     )
   }
}

export default withAuth(Faltantes) 