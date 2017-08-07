import React, { Component } from 'react';
import withAuth from  '../../utils/withAuth';
import {Badge} from 'reactstrap';

class PrestacionesFooter extends Component {

    render() {
        return (
            <div className="row no-margins footer-container">
                <div className="col">
                    <div>
                        <i className="fa fa-calendar"></i> Período: 
                        <Badge>
                            {this.props.footer.period || 'N/A'}
                        </Badge>
                    </div>                            
                </div>
                <div className="col">
                    <div>
                        <i className="fa fa-list"></i>Servicios: 
                        <Badge>
                            {this.props.footer.servicesQuantity || 'N/A'}
                        </Badge>
                    </div>
                </div>
                        
                <div className="col">
                    <div>
                        <i className="fa fa-ambulance"></i>Prestador:
                         <Badge>
                             {this.props.footer.company || 'N/A'}
                        </Badge>
                    </div>
                </div>
                <div className="col">
                    <div>
                        <i className="fa fa-usd"></i>Importe liq.: 
                        <Badge>
                            {this.props.footer.totalAmount ? this.props.footer.totalAmount.toFixed(2) : 'N/A'}
                        </Badge>
                    </div>
                </div>  
            </div>                  
        )
    }
}

export default withAuth(PrestacionesFooter) 