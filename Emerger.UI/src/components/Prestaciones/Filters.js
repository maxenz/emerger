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
                ftrCompanyValue : "",
                ftrSimplePeriod: "",
                ftrState: "",
                companies: [],
                periods: [],
                states: [],
                selectedDate: moment(),
                rangeStartDate: null,
                rangeEndDate: null,
                focused: false,
                popoverOpen: false
        }

        this.isOutsideRange = this.isOutsideRange.bind(this);
        this.handleSimplePeriodFilterChange = this.handleSimplePeriodFilterChange.bind(this);
        this.handleCompanyFilterChange = this.handleCompanyFilterChange.bind(this);
        this.handleStateFilterChange = this.handleStateFilterChange.bind(this);
    };
 
    componentDidMount() {    
        axios.all([this.getCompanies(), this.getPeriods(), this.getStates()])
            .then(res => {  
                this.setState({
                    companies: res[0].data.companies.map(this.mapFilter),
                    periods: res[1].data.periods.map(this.mapPeriodFilter),
                    states: res[2].data.states.map(this.mapFilter)
                });                
            })
            .catch(function(error){
                console.log(error);
            });
    }

    getCompanies = () => {
        return axios.get('/filters/companies');
    }

    getPeriods = () => {
        return axios.get('/filters/periods');
    }

    getStates = () => {
        return axios.get('/filters/states');
    }

    handleCompanyFilterChange = (val) => {
        this.setState({ftrCompany: val});
    }

    handleSimplePeriodFilterChange = (obj) => {
        this.setState({ftrSimplePeriod: obj});
        if (obj !== null) {
            this.setState({rangeStartDate: this.getPropertyValueById(this.state.periods, obj.value, 'dateFrom')});
            this.setState({rangeEndDate: this.getPropertyValueById(this.state.periods, obj.value, 'dateTo')});
            this.setState({selectedDate: this.getPropertyValueById(this.state.periods, obj.value, 'dateFrom')});
        }
    }

    handleStateFilterChange = (val) => {
        this.setState({ftrState: val});
    }

    mapFilter = (obj) => {
        return {value: obj.id, label: obj.description};
    }

    mapPeriodFilter = (obj) => {
        return {value: obj.id, label: obj.description, dateFrom: moment(obj.dateFrom), dateTo: moment(obj.dateTo).add(1, 'days')};
    }

    isOutsideRange = (_date) => {
        return _date < this.state.rangeStartDate || _date > this.state.rangeEndDate;
    }

    getPropertyValueById = (arr, id, prop) => {
        return arr.filter(function(obj) {
            return obj.value === id;
        })[0][prop];
    }

    render() {

        

        return (
            <div className="container-fluid" style={{padding:10, backgroundColor: "#ecf0f1", borderBottom: "1px solid #95a5a6"}}>
                <div className="container-filters">
                    <Filter
                        name="ftr-company-select"
                        value={this.state.ftrCompany}
                        options={this.state.companies}
                        onChangeHandler={this.handleCompanyFilterChange}
                        placeholder="Seleccione empresa..."
                    />                      
                    <Filter
                        name="ftr-state-select"
                        value={this.state.ftrState}
                        options={this.state.states}
                        onChangeHandler={this.handleStateFilterChange}
                        placeholder="Seleccione estado..."                          
                    />
                    <Filter
                        name="ftr-simple-period-select"
                        value={this.state.ftrSimplePeriod}
                        options={this.state.periods}
                        onChangeHandler={this.handleSimplePeriodFilterChange}
                        placeholder="Seleccione período..."                          
                    />

                    {   
                        this.state.ftrSimplePeriod ?                   
                                <SingleDatePicker
                                    date={this.state.selectedDate}
                                    onDateChange={selectedDate => this.setState({ selectedDate })} 
                                    focused={this.state.focused}
                                    onFocusChange={({ focused }) => this.setState({ focused })} 
                                    hideKeyboardShortcutsPanel = {true}
                                    isOutsideRange = {this.isOutsideRange}
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