import React, { Component } from 'react';
import withAuth from  '../../utils/withAuth';
import ReactTable from 'react-table';
import _ from 'lodash';
import namor from 'namor';
import moment from 'moment';
import 'moment/locale/es';
import {Modal, ModalHeader, ModalBody, ModalFooter, Button} from 'reactstrap';
import ModalServiceDetails from './ModalServiceDetails';

class Prestaciones extends Component {

    constructor(props) {
        super(props);

        this.state = {
            incModal: false,
            selectedService: null
        }

        this.toggleIncModal = this.toggleIncModal.bind(this);

    };

    toggleIncModal(serviceNumber) {
        
        var service = null;

        if (serviceNumber) {            
            service = this.props.services.filter(function(s){
                return s.number === serviceNumber;
            })[0];
        } 

        this.setState({
            incModal: !this.state.incModal,
            selectedService: service
        });
    }
 
    render() {

        const columns = [
            {
                Header: 'Servicios',
                columns: [
                    {
                        Header: 'Fecha',
                        accessor: 'date',
                        Cell: props => <span>{moment(props.value).format('DD - dddd')}</span>,
                        style: {textAlign: 'center'}
                    },
                    {
                        Header: 'Nro',
                        accessor: 'number',
                        Cell: props => <a onClick={() => this.toggleIncModal(props.value)} href="javascript:void(0)" >{props.value}</a>,
                        style: {textAlign: 'center'}
                    },
                    {
                        Header: 'Conc.',
                        accessor: 'concept',
                        style: {textAlign: 'center'}                          
                    },
                    {
                        Header: 'Cliente',
                        accessor: 'client'
                    },
                    {
                        Header: 'Paciente',
                        accessor: 'patient',
                        width: 200
                    },
                    {
                        Header: 'Origen',
                        accessor: 'origin',
                        width: 150
                    },
                    {
                        Header: 'Destino',
                        accessor: 'destiny',
                        width: 150                       
                    },
                    {
                        Header: 'Km.',
                        accessor: 'kilometers',
                        style: {textAlign: 'center'}                            
                    },
                    {
                        Header: 'Importe',
                        accessor: 'amount',
                        Cell: props => <span>${props.value.toFixed(2)}</span>,
                        style: {textAlign: 'right'}                                                     
                    },
                    {
                        Header: 'Conf.',
                        accessor: 'state',
                        style: {textAlign: 'center'},
                        Cell: props => 
                        <span>
                           {
                               props.value === 1 ? <i className='fa fa-check-circle color-green'></i>
                               : props.value === 2 ? <i className='fa fa-times-circle color-red'></i>
                               : props.value === 3 ? <i className='fa fa-times-circle color-yellow'></i>
                               : props.value === 4 ? <i className='fa fa-check-circle color-blue'></i>
                               : props.value === 5 ? <i className='fa fa-times-circle color-blue'></i>
                               : ''
                           }
                            
                            
                        </span>                
                    },
                    {
                        Header: 'Revisar',
                        Cell: props => <a onClick={this.props.onCheckService} href="javascript:void(0)" >Revisar</a>,
                        style: {textAlign: 'center'}
                    }
                ]
            }
        ]

            return (         
                <div> 
                    {
                        this.state.selectedService ? 
                            <ModalServiceDetails modalOpened={this.state.incModal} onModalToggle={this.toggleIncModal} service={this.state.selectedService} /> 
                            : null
                    }

                    <div className="table-wrap">
                    <ReactTable
                        className="-striped -highlight"
                        data={this.props.services}
                        columns={columns}
                        showPageSizeOptions={false}
                        defaultPageSize={10}
                        previousText="Anterior"
                        nextText="Siguiente"
                        loadingText="Cargando..."
                        noDataText="No se encontraron datos"
                        pageText="Página"
                        ofText="de"
                        rowsText="filas"
                    />
                    </div>
                </div>   
            )
    }
}

export default withAuth(Prestaciones) 