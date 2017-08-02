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
                                                    <td><strong>Item Name</strong></td>
                                                    <td className="text-xs-center"><strong>Item Price</strong></td>
                                                    <td className="text-xs-center"><strong>Item Quantity</strong></td>
                                                    <td className="text-xs-right"><strong>Total</strong></td>
                                                </tr>
                                            </thead>
                                            <tbody>
                                                <tr>
                                                    <td>Samsung Galaxy S5</td>
                                                    <td className="text-xs-center">$900</td>
                                                    <td className="text-xs-center">1</td>
                                                    <td className="text-xs-right">$900</td>
                                                </tr>
                                                <tr>
                                                    <td>Samsung Galaxy S5 Extra Battery</td>
                                                    <td className="text-xs-center">$30.00</td>
                                                    <td className="text-xs-center">1</td>
                                                    <td className="text-xs-right">$30.00</td>
                                                </tr>
                                                <tr>
                                                    <td>Screen protector</td>
                                                    <td className="text-xs-center">$7</td>
                                                    <td className="text-xs-center">4</td>
                                                    <td className="text-xs-right">$28</td>
                                                </tr>
                                                <tr>
                                                    <td className="highrow"></td>
                                                    <td className="highrow"></td>
                                                    <td className="highrow text-xs-center"><strong>Subtotal</strong></td>
                                                    <td className="highrow text-xs-right">$958.00</td>
                                                </tr>
                                                <tr>
                                                    <td className="emptyrow"></td>
                                                    <td className="emptyrow"></td>
                                                    <td className="emptyrow text-xs-center"><strong>Shipping</strong></td>
                                                    <td className="emptyrow text-xs-right">$20</td>
                                                </tr>
                                                <tr>
                                                    <td className="emptyrow"></td>
                                                    <td className="emptyrow"></td>
                                                    <td className="emptyrow text-xs-center"><strong>Total</strong></td>
                                                    <td className="emptyrow text-xs-right">$978.00</td>
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