import React from 'react';

function LoginRedirect() {
  window.location.href = 'https://localhost:7106/account/login';
  return null;
}
export default function Login(){
    return <>{LoginRedirect()}</>
}