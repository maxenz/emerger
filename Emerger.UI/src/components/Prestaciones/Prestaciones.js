import React, { Component } from 'react';
import withAuth from  '../../utils/withAuth';
import ReactTable from 'react-table';
import _ from 'lodash';
import namor from 'namor';
import moment from 'moment';
import 'moment/locale/es';

class Prestaciones extends Component {

    constructor(props) {
        super(props);

        this.state = {

        }

    };
 
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
                        style: {textAlign: 'center'}
                    },
                    {
                        Header: 'Conc.',
                        accessor: 'concept'
                    },
                    {
                        Header: 'Cliente',
                        accessor: 'client'
                    },
                    {
                        Header: 'Paciente',
                        accessor: 'patient'
                    },
                    {
                        Header: 'Origen',
                        accessor: 'origin',
                        style: {textAlign: 'center'}
                    },
                    {
                        Header: 'Destino',
                        accessor: 'destiny',
                        style: {textAlign: 'center'}                        
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