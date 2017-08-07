import React, { Component } from 'react';
import withAuth from  '../../utils/withAuth';
import ReactTable from 'react-table';
import _ from 'lodash';
import namor from 'namor';
import moment from 'moment';
import 'moment/locale/es';
import {Modal, ModalHeader, ModalBody, ModalFooter, Button} from 'reactstrap';

class ModalRevision extends Component {

     render() {

        return (         
            <Modal isOpen={this.props.modalOpened} toggle={this.props.onModalToggle} size="lg" className="modal-revision">
                <ModalHeader toggle={this.props.onModalToggle}>
                    Detalles del incidente
                     <span className="italic"> sadasd </span>
                      del paciente 
                      <span className="italic"> asdfsd</span>
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
                                                <tr className="service-total-border">
                                                    <td className="emptyrow"></td>
                                                    <td className="emptyrow"></td>
                                                    <td className="emptyrow text-xs-center"><strong>Total</strong></td>
                                                    <td className="emptyrow text-xs-right bold">sarasa</td>
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

export default withAuth(ModalRevision) 