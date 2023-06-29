/* eslint-disable */
import React from 'react'
import {Switch, Route } from "react-router-dom";
import Settings from '../../Views/Settings/Settings';
import Profile from '../../Views/Profile/Profile';
import Dashboard from '../../Views/Dashboard/Dashboard';
import Login from '../Login/Login';
import Logout from '../Logout/Logout';
import Calendar from '../../Views/Calendar/Calendar';


export default function Switches() {
    return (
        <Switch>
          <Route path="/admin/profile"   component={Profile} />
          <Route path="/admin/dashboard" component={Dashboard}/>
          <Route path="/actions/calendar" component={Calendar}/>
          <Route path="/login/" component={Login}/>
          <Route path="/logout/" component={Logout}/>
       </Switch>
    )
}
