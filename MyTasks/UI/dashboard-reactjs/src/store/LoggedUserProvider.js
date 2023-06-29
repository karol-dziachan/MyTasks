/* eslint-disable */
import {React, useEffect, useState} from "react";
import ConfigProvider from "./ConfigProvider"
import axios from 'axios';
import Cookie from 'js-cookie'

const consumeIsUserAuthenticated = () => {
  var path = ConfigProvider("api_basepath") + '/user-authenticated';
  const [response, setResponse] = useState([]);
  useEffect(() => {
    const fetchAuthenticatedUser = async () => {
      try {

        const requestOptions = {
          method: "GET",
          withCredentials: true,
           headers: {
            'Content-Type': 'application/json',
            'Cookie': Cookie.get('IdToken') 
          },
        };

        const response = await axios.get(path, requestOptions);
        const data = response.data;
        console.log(data);
         setResponse(data);
      } catch (error) {
        console.log(error.message);
      }
    };

    fetchAuthenticatedUser();
  }, []);

  return response;
}
export default consumeIsUserAuthenticated;
