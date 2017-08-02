import React, { Component } from 'react';
import Nav from '../components/Nav';
import Filters from '../components/Prestaciones/Filters';
import Prestaciones from '../components/Prestaciones/Prestaciones';
import Footer from '../components/Footer';
import withAuth from  '../utils/withAuth';
import axios from 'axios';
import moment from 'moment';

class PrestacionesContainer extends Component {

    constructor(props) {
        super(props);

        this.state = {
        ftrCompany : "",
        ftrSimplePeriod: "",
        ftrState: "",
        companies: [],
        periods: [],
        states: [
            {value: 0, label: 'Todos'},
            {value: 1, label: 'Reclamados'},
            {value: 2, label: 'Pendientes'},
            {value: 3, label: 'Resueltos'}
        ],
        services: [],
        rangeStartDate: null,
        rangeEndDate: null,
        selectedDate: moment()
        }

        this.isOutsideRange = this.isOutsideRange.bind(this);
        this.handleSimplePeriodFilterChange = this.handleSimplePeriodFilterChange.bind(this);
        this.handleCompanyFilterChange = this.handleCompanyFilterChange.bind(this);
        this.handleStateFilterChange = this.handleStateFilterChange.bind(this);
        this.handleDatePickerChange = this.handleDatePickerChange.bind(this);
        this.onSubmitHandler = this.onSubmitHandler.bind(this);
    }

    onSubmitHandler = () => {
        axios.get('/services', {
            params: {
                companyId: this.state.ftrCompany.value,
                stateId: this.state.ftrState.value,
                periodId: this.state.ftrSimplePeriod.value
            }
        })
        .then(res => {  
            this.setState({services: res.data.services});
        })
        .catch(function(error){
            console.log(error);
        });
    }

    componentDidMount() {    
        axios
        .all([this.getCompanies(), this.getPeriods()])
        .then(res => {  
            this
            .setState({
                companies: res[0].data.companies.map(this.mapFilter),
                periods: res[1].data.periods.map(this.mapPeriodFilter)
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

  mapFilter = (obj) => {
      return {value: obj.id, label: obj.description};
  }

  mapPeriodFilter = (obj) => {
      return {value: obj.id, label: obj.description, dateFrom: moment(obj.dateFrom), dateTo: moment(obj.dateTo).add(1, 'days')};
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

  handleDatePickerChange = (val) => {
    this.setState({selectedDate: val})
  }

  getPropertyValueById = (arr, id, prop) => {
      return arr.filter(function(obj) {
          return obj.value === id;
      })[0][prop];
  }

  isOutsideRange = (_date) => {
      return _date < this.state.rangeStartDate || _date > this.state.rangeEndDate;
  }

  checkService = () => {
      console.log('check service');
  }

  selectService = () => {
      console.log('select servie');
  }

  render() {
    return (
      <div>
          <Nav properties={this.props}/>
          <Filters
              onSubmitHandler={this.onSubmitHandler}
              ftrCompany={this.state.ftrCompany}
              ftrSimplePeriod={this.state.ftrSimplePeriod}
              ftrState={this.state.ftrState}
              selectedDate={this.state.selectedDate}
              companies={this.state.companies}
              states={this.state.states}
              periods={this.state.periods}
              handleCompanyFilterChange={this.handleCompanyFilterChange}
              handleSimplePeriodFilterChange={this.handleSimplePeriodFilterChange}
              handleStateFilterChange={this.handleStateFilterChange}
              handleDatePickerChange={this.handleDatePickerChange}
              isOutsideRange={this.isOutsideRange}
           />
          <Prestaciones services={this.state.services} onCheckService={this.checkService} onSelectService={this.selectService} />
          <Footer />
      </div>
    )
  }
}

export default withAuth(PrestacionesContainer);