/* eslint-disable */
import {React, useEffect, useState} from "react";
import ConfigProvider from "./ConfigProvider"
import GetCookie from "./GetCookie";
import Cookies from 'js-cookie'

const consumeIsUserAuthenticated = () => {
    var path = ConfigProvider("api_basepath") + '/user-authenticated';
    const [response, setResponse] = useState([]);
    // const cookie = Cookies.get('AspNetCore.Cookies');
    // console.log('cookie ',  cookie)
   useEffect(() => {
      fetch(path, {
        method: "GET", 
      })
         .then((response) => response.json())
         .then((data) => {
           console.log(data);
         //   setResponse(data);
         })
         .catch((err) => {
            console.log(err.message);
         });
   }, []);

   return response;
}

export default consumeIsUserAuthenticated;
