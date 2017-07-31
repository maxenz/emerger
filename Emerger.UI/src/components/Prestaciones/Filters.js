import React, { Component } from 'react';
import withAuth from  '../../utils/withAuth';
import Filter from '../Filter';
import InformationButton from '../InformationButton';
import axios from 'axios';
import {SingleDatePicker} from 'react-dates';
import moment from 'moment';
import {Badge} from 'reactstrap';

class PrestacionesFilters extends Component {
    constructor(props) {
        super(props);

        this.state = {
            focused: false
        }
    };

    render() {
        return (
            <div className="container-fluid" style={{padding:10, backgroundColor: "#ecf0f1", borderBottom: "1px solid #95a5a6"}}>
                <div className="container-filters">
                    <Filter
                        name="ftr-company-select"
                        value={this.props.ftrCompany}
                        options={this.props.companies}
                        onChangeHandler={this.props.handleCompanyFilterChange}
                        placeholder="Seleccione empresa..."
                    />                      
                    <Filter
                        name="ftr-state-select"
                        value={this.props.ftrState}
                        options={this.props.states}
                        onChangeHandler={this.props.handleStateFilterChange}
                        placeholder="Seleccione estado..."                          
                    />
                    <Filter
                        name="ftr-simple-period-select"
                        value={this.props.ftrSimplePeriod}
                        options={this.props.periods}
                        onChangeHandler={this.props.handleSimplePeriodFilterChange}
                        placeholder="Seleccione período..."                          
                    />

                    {   
                        this.props.ftrSimplePeriod ?                   
                                <SingleDatePicker
                                    date={this.props.selectedDate}
                                    onDateChange={this.props.handleDatePickerChange} 
                                    focused={this.state.focused}
                                    onFocusChange={({ focused }) => this.setState({ focused })} 
                                    hideKeyboardShortcutsPanel = {true}
                                    isOutsideRange = {this.props.isOutsideRange}
                                    displayFormat="DD/MM/YYYY"
                                />
                            : null
                    }

                    <button className="btn btn-outline-success" onClick={this.props.onSubmitHandler}>
                        <i className="fa fa-search"></i> Consultar
                    </button>
                    <div style={{marginLeft:'auto', marginRight:0, textAlign: 'right'}}>
                        <InformationButton 
                            buttonTitle="Instrucciones de uso"
                            popoverTitle="Sección prestaciones"
                            popoverContent={
                                <div>
                                    <p>
                                        <Badge>1</Badge> Utilice el link "Revisar" para cambiar la conformidad. Luego, en caso de tener diferencias,
                                                        fundamente el motivo y el importe total de su resultado.
                                    </p>                                   
                                    <p>
                                        <Badge>2</Badge> En caso de que desee conocer la apertura de los valores y más datos del servicio, 
                                                        utilice el link con el número de incidente.
                                    </p>
                                    <hr />
                                    <h6>Referencias de conformidad</h6>
                                    <div>
                                        <i className="fa fa-check-circle color-green"></i> Estoy conforme con los valores del servicio.
                                    </div>
                                    <div>
                                        <i className="fa fa-times-circle color-red"></i> No estoy conforme con los valores del servicio.
                                    </div>
                                    <div>
                                        <i className="fa fa-times-circle color-yellow"></i> He recibido una respuesta a mi reclamo.
                                    </div>
                                    <div>
                                        <i className="fa fa-check-circle color-blue"></i> Mi reclamo ha sido aceptado y se aplicó la diferencia.
                                    </div>
                                    <div>
                                        <i className="fa fa-times-circle color-blue"></i> Mi reclamo no ha sido aceptado y el importe no se ha alterado.
                                    </div>
                                </div>
                                } />
                        </div>                       
                </div>
            </div>
        )
    }
}

export default withAuth(PrestacionesFilters) 