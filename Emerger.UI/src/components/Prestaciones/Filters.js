import React, { Component } from 'react';
import withAuth from  '../../utils/withAuth';
import Filter from '../Filter';
import axios from 'axios';
import {SingleDatePicker} from 'react-dates';
import moment from 'moment';

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
                focused: false
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
        return {value: obj.id, label: obj.description, dateFrom: moment(obj.dateFrom), dateTo: moment(obj.dateTo)};
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
                <div className="row no-margins">
                    <div className="col-2">
                        <Filter
                            name="ftr-company-select"
                            value={this.state.ftrCompany}
                            options={this.state.companies}
                            onChangeHandler={this.handleCompanyFilterChange}
                            placeholder="Seleccione empresa..."
                        />
                    </div>
                    <div className="col-2">                        
                        <Filter
                            name="ftr-state-select"
                            value={this.state.ftrState}
                            options={this.state.states}
                            onChangeHandler={this.handleStateFilterChange}
                            placeholder="Seleccione estado..."                          
                        />
                    </div>
                    <div className="col-2">
                        <Filter
                            name="ftr-simple-period-select"
                            value={this.state.ftrSimplePeriod}
                            options={this.state.periods}
                            onChangeHandler={this.handleSimplePeriodFilterChange}
                            placeholder="Seleccione período..."                          
                        />
                    </div>

                        {   
                            this.state.ftrSimplePeriod ? 
                                <div className="col-1" style={{marginRight: '3.5%'}}>                  
                                    <SingleDatePicker
                                        date={this.state.selectedDate}
                                        onDateChange={selectedDate => this.setState({ selectedDate })} 
                                        focused={this.state.focused}
                                        onFocusChange={({ focused }) => this.setState({ focused })} 
                                        hideKeyboardShortcutsPanel = {true}
                                        isOutsideRange = {this.isOutsideRange}
                                        displayFormat="DD/MM/YYYY"
                                    />
                                </div>
                             : null
                        }

                    <button className="btn btn-outline-success" onClick={this.props.onSubmitHandler}>
                        <i className="fa fa-search"></i> Consultar
                    </button>

                </div>
            </div>
        )
    }
}

export default withAuth(PrestacionesFilters) 