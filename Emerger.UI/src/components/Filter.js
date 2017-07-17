import React, { Component } from 'react';
import withAuth from  '../utils/withAuth';
import Select from 'react-select';

class Filter extends Component {

    render() {
        return (
            <Select
                name={this.props.name}
                value={this.props.value}
                options={this.props.options}
                onChange={this.props.onChangeHandler}
                searchable={false}
                placeholder={this.props.placeholder}
            />      
        )
    }
}

export default withAuth(Filter) 