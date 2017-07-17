import React, { Component } from 'react';
import withAuth from  '../../utils/withAuth';
import Filter from '../Filter';
import axios from 'axios';

const options = [
    { value: 'one', label: 'One' },
    { value: 'two', label: 'Two' }
];

class PrestacionesFilters extends Component {

    state = {
        ftrCompanyValue : "",
        ftrSimplePeriod: "",
        ftrRangePeriod: "",
        ftrState: "",
        companies: [],
        periods: [],
        states: []
    }
    
    componentDidMount() {        
        axios.all([this.getCompanies(), this.getPeriods(), this.getStates()])
            .then(axios.spread(function(cmp, pds, st){
                console.log(cmp);
                console.log(pds);
                console.log(st);
            }))
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

    handleSimplePeriodFilterChange = (val) => {
        this.setState({ftrSimplePeriod: val});
    }

    handleRangePeriodFilterChange = (val) => {
        this.setState({ftrRangePeriod: val});
    }

    handleStateFilterChange = (val) => {
        this.setState({ftrState: val});
    }

    render() {
        return (
            <div className="container-fluid" style={{padding:10, backgroundColor: "#ecf0f1", borderBottom: "1px solid #95a5a6"}}>
                <div className="row">
                    <div className="col-2">
                        <Filter
                            name="ftr-company-select"
                            value={this.state.ftrCompany}
                            options={options}
                            onChangeHandler={this.handleCompanyFilterChange}
                            placeholder="Seleccione empresa..."
                        />
                    </div>
                    <div className="col-2">
                        <Filter
                            name="ftr-simple-period-select"
                            value={this.state.ftrSimplePeriod}
                            options={options}
                            onChangeHandler={this.handleSimplePeriodFilterChange}
                            placeholder="Seleccione período..."                          
                        />
                    </div>
                    <div className="col-2">
                        <Filter
                            name="ftr-range-period-select"
                            value={this.state.ftrRangePeriod}
                            options={options}
                            onChangeHandler={this.handleRangePeriodFilterChange}                      
                        />
                    </div>
                    <div className="col-2">                        
                        <Filter
                            name="ftr-state-select"
                            value={this.state.ftrState}
                            options={options}
                            onChangeHandler={this.handleStateFilterChange}
                            placeholder="Seleccione estado..."                          
                        />
                    </div>

                    <button className="btn btn-outline-success">
                        <i className="fa fa-search"></i> Consultar
                    </button>

                </div>
            </div>
        )
    }
}

export default withAuth(PrestacionesFilters) 