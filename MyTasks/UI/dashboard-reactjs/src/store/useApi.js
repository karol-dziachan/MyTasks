import { useState, useEffect } from 'react';
import axios from 'axios';
import Cookie from 'js-cookie';
import ConfigProvider from './ConfigProvider';

const useApiClient = () => {
  const [response, setResponse] = useState([]);
  const [loading, setLoading] = useState(true);

  const sendRequest = async (relativePath, method, requestData) => {
    const basePath = ConfigProvider('api_basepath');
    const path = `${basePath}/${relativePath}`;

    try {
      const requestOptions = {
        method: method,
        withCredentials: true,
        headers: {
          'Content-Type': 'application/json',
          Cookie: Cookie.get('IdToken'),
        },
      };

      if (requestData) {
        requestOptions.data = requestData;
      }

      setLoading(true);

      const response = await axios(path, requestOptions);
      const data = response.data;
      setResponse(data);

      return { response: data, loading: false };
    } catch (error) {
      console.log(error.message);
      setLoading(false);
      throw error;
    }
  };

  return { response, loading, sendRequest };
};

export default useApiClient;