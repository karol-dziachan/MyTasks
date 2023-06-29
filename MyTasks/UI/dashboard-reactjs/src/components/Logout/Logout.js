/* eslint-disable */
import React from 'react';
import ConfigProvider from '../../store/ConfigProvider';

function LogoutRedirect() {
  var logoutPath = ConfigProvider("api_basepath") + '/account/logout'

  console.log('wyloguj ', logoutPath)
  window.location.href = logoutPath;
  return null;
}

export default function Logout(){
    return <>{LogoutRedirect()}</>
}