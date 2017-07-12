import React from 'react';
import { Switch, Route } from 'react-router-dom';
import Login from './../components/Login';

const Main = () => (
  <main>
    <Switch>
      <Route exact path='/' component={Login}/>
      <Route path='/login' component={Login}/>
    </Switch>
  </main>
)

export default Main;