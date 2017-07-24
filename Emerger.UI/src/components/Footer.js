import React, { Component } from 'react';
import withAuth from  '../utils/withAuth';
import {Badge} from 'reactstrap';

class Footer extends Component {

    render() {
        return (
            <div className="row no-margins footer-container">
                <div className="col-2 offset-2">
                    <div>
                        <i className="fa fa-calendar"></i> Período: <Badge>07/10</Badge>
                    </div>                            
                </div>
                <div className="col-2">
                    <div>
                        <i className="fa fa-list"></i>Servicios: <Badge>22</Badge>
                    </div>
                </div>
                        
                <div className="col-2">
                    <div>
                        <i className="fa fa-ambulance"></i>Prestador: <Badge>Telecom</Badge>
                    </div>
                </div>
                <div className="col-2">
                    <div>
                        <i className="fa fa-usd"></i>Importe liq.: <Badge>$256.21</Badge>
                    </div>
                </div>  
            </div>                  
        )
    }
}

export default withAuth(Footer) 