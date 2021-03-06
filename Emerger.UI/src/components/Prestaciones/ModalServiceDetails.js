import React, { Component } from 'react';
import withAuth from  '../../utils/withAuth';
import ReactTable from 'react-table';
import _ from 'lodash';
import namor from 'namor';
import moment from 'moment';
import 'moment/locale/es';
import {Modal, ModalHeader, ModalBody, ModalFooter, Button} from 'reactstrap';

class ModalServiceDetails extends Component {

     render() {

        //harcodeado, sacar cuando funcione el backend
        const details = [
            {
                id: 1,
                concept: 'Concepto 1',
                amount: 234,
                quantity: 2,
                total: 468
            },
            {
                id: 2,
                concept: 'Concepto 2',
                amount: 200,
                quantity: 1,
                total: 200
            },
            {
                id: 3,
                concept: 'Concepto 3',
                amount: 300,
                quantity: 5,
                total: 1500
            },
            {
                id: 4,
                concept: 'Concepto 4',
                amount: 150,
                quantity: 2,
                total: 300
            }
        ];

        const sumTotal = (total, num) => {
            return total + num.total;
        }

        this.props.service.totalServiceDetails = details.reduce(sumTotal,0);

        const lines = () => {
            return details.map((item, i) => {
                return (
                    <tr key={item.id}>
                        <td className="bold">{item.concept}</td>
                        <td className="text-xs-center">${item.amount.toFixed(2)}</td>
                        <td className="text-xs-center">{item.quantity}</td>
                        <td className="text-xs-right">${item.total.toFixed(2)}</td>
                    </tr>
                )
            })
        }

        return (         
            <Modal isOpen={this.props.modalOpened} toggle={this.props.onModalToggle} size="lg" className="modal-service-details">
                <ModalHeader toggle={this.props.onModalToggle}>
                    Detalles del incidente
                     <span className="italic"> {this.props.service.number} </span>
                      del paciente 
                      <span className="italic"> {this.props.service.patient}</span>
                </ModalHeader>
                <ModalBody>
                    <div className="row">
                        <div className="col-md-12">
                            <div className="card">
                                <div className="card-block">
                                    <div className="table-responsive">
                                        <table className="table table-sm">
                                            <thead>
                                                <tr>
                                                    <td><strong>Concepto</strong></td>
                                                    <td className="text-xs-center"><strong>Importe</strong></td>
                                                    <td className="text-xs-center"><strong>Cantidad</strong></td>
                                                    <td className="text-xs-right"><strong>Total</strong></td>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                { lines() }
                                                <tr className="service-total-border">
                                                    <td className="emptyrow"></td>
                                                    <td className="emptyrow"></td>
                                                    <td className="emptyrow text-xs-center"><strong>Total</strong></td>
                                                    <td className="emptyrow text-xs-right bold">${this.props.service.totalServiceDetails.toFixed(2)}</td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </ModalBody>
            </Modal>  
        )
    }
}

export default withAuth(ModalServiceDetails) 