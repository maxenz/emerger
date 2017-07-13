import React from 'react';
import { Link } from 'react-router-dom';
import './NotFound.css';

const NotFound = () =>  (    
    <div className="outer outer-notfound">
        <div className="middle">
            <div className="inner"> 
                <div id="content">        
                    <div className="clearfix"></div>
                    
                    <div id="main-body">
                        <p className="enormous-font bree-font"> 404 </p>
                        <p className="big-font"> Esto es vergonzoso; la página solicitada no existe... </p>
                        <hr />
                        <p className="big-font"> Volvamos a la <Link to="/" className="underline">página principal</Link></p>
                    </div>
                </div>
            </div>
        </div>
    </div>
)

export default NotFound;