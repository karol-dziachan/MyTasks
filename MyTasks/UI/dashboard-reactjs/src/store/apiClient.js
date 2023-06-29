import { useState, useEffect } from 'react';
import axios from 'axios';
import Cookie from 'js-cookie';
import ConfigProvider from './ConfigProvider';

const useApiClient = (relativePath, method, requestData) => {
  const basePath = ConfigProvider('api_basepath');
  const path = `${basePath}/${relativePath}`;

  const [response, setResponse] = useState([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const fetchData = async () => {
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
        console.log(data);
        setResponse(data);
      } catch (error) {
        console.log(error.message);
      } finally {
        setLoading(false);
      }
    };

    fetchData();
  }, [path, method, requestData]);

  return { response, loading };
};

export default useApiClient;