/* eslint-disable */
import React from 'react';
import ConfigProvider from '../../store/ConfigProvider';

function LoginRedirect() {
  var loginPath = ConfigProvider("api_basepath") + '/account/login'

  window.location.href = loginPath;
  return null;
}

export default function Login(){
    return <>{LoginRedirect()}</>
}