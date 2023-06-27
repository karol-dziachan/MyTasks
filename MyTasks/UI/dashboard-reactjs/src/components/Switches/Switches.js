/* eslint-disable */
import React from 'react'
import {Switch, Route } from "react-router-dom";
import Settings from '../../Views/Settings/Settings';
import Profile from '../../Views/Profile/Profile';
import Dashboard from '../../Views/Dashboard/Dashboard';
import Login from '../Login/Login';

export default function Switches() {
    return (
        <Switch>
          <Route path="/admin/profile"   component={Profile} />
          <Route path="/admin/dashboard" component={Dashboard}/>
          <Route path="/login/" component={Login}/>
       </Switch>
    )
}
