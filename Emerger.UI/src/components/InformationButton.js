import React, { Component } from 'react';
import withAuth from  '../utils/withAuth';
import {Button, Popover, PopoverTitle, PopoverContent} from 'reactstrap';
import namor from 'namor';

class InformationButton extends Component {

    constructor(props) {
        super(props);

        this.state = {
            popoverOpen: false
        }

        this.toggle = this.toggle.bind(this);

    }

    toggle() {
        this.setState({
            popoverOpen: !this.state.popoverOpen
        });
    }

    render() {

        const tether = {
            classPrefix : 'emerger-information'
        };
        const buttonId = namor.generate({ words: 1, numbers: 0 });

        return (
            <div>
                <button className="btn btn-warning" id={buttonId} onMouseOver={this.toggle} onMouseLeave={this.toggle} >
                    <i className="fa fa-book"></i> {this.props.buttonTitle}
                </button>
                <Popover placement="left bottom" isOpen={this.state.popoverOpen} target={buttonId} toggle={this.toggle} 
                    tether={tether}>
                    <PopoverTitle>{this.props.popoverTitle}</PopoverTitle>
                    <PopoverContent>{this.props.popoverContent}</PopoverContent>
                </Popover> 
            </div>
            
        )
    }
}

export default withAuth(InformationButton) 