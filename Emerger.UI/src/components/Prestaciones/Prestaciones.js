import React, { Component } from 'react';
import withAuth from  '../../utils/withAuth';
import ReactTable from 'react-table';
import _ from 'lodash';
import namor from 'namor';

class Prestaciones extends Component {

    constructor(props) {
        super(props);

        this.state = {

        }

    };
 
        render() {

            const data = _.map(_.range(5553), d => {
            return {
                firstName: namor.generate({ words: 1, numbers: 0 }),
                lastName: namor.generate({ words: 1, numbers: 0 }),
                age: Math.floor(Math.random() * 30),
                children: _.map(_.range(10), d => {
                return {
                    firstName: namor.generate({ words: 1, numbers: 0 }),
                    lastName: namor.generate({ words: 1, numbers: 0 }),
                    age: Math.floor(Math.random() * 30)
                }
                })
            }
            })

            const columns = [
                {
                    Header: 'Name',
                    columns: [
                    {
                        Header: 'First Name',
                        accessor: 'firstName'
                    },
                    {
                        Header: 'Last Name',
                        id: 'lastName',
                        accessor: d => d.lastName
                    }
                    ]
                },
                {
                    Header: 'Info',
                    columns: [
                    {
                        Header: 'Age',
                        accessor: 'age'
                    }
                    ]
                }
            ]

            return (         
                <div>   
                    <div className="table-wrap">
                    <ReactTable
                        className="-striped -highlight"
                        data={data}
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