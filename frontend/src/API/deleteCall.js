// api.js
import axios from 'axios';
import baseURL from './baseURL';

export const deleteCall = async (endpoint, errorMessage, token) => {
  try {
    const config = {
      headers: {
        'Authorization': `Bearer ${token}`,
      }
    };

    const response = await axios.delete(`${baseURL}${endpoint}`, config);

    console.log('Erfolgreich');
    return response;

  } catch (error) {
    if (error.response) {
      // Server responded with a status code that falls out of the range of 2xx
      console.error('Fehlerhafte Antwort vom Server:', error.response.data);
      throw new Error(`${errorMessage}: ${error.response.data}`);
    } else if (error.request) {
      // The request was made but no response was received
      console.error('Keine Antwort vom Server:', error.request);
      throw new Error(`${errorMessage}: Keine Antwort vom Server`);
    } else {
      // Something happened in setting up the request that triggered an Error
      console.error('Fehler:', error.message);
      throw new Error(`${errorMessage}: ${error.message}`);
    }
  }
};
