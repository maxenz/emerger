import React, { Component } from 'react';
import { Link } from 'react-router-dom';
import './Unauthorized.css';

const Unauthorized = () =>  (
     <div className="outer outer-unauthorized">
        <div className="middle">
            <div className="inner"> 
                <div id="content">        
                    <div className="clearfix"></div>
                    
                    <div id="main-body">
                        <p className="enormous-font bree-font"> 401 </p>
                        <p className="big-font"> Ups! Usted no se encuentra autorizado para ingresar a esta página... </p>
                        <hr />
                        <p className="big-font"> Volvamos a la <Link to="/" className="underline">página principal</Link></p>
                    </div>
                </div>
            </div>
        </div>
    </div>
)

export default Unauthorized;
