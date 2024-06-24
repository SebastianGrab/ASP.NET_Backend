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
   
      console.error('Fehlerhafte Antwort vom Server:', error.response.data);
      throw new Error(`${errorMessage}: ${error.response.data}`);
    } else if (error.request) {
  
      console.error('Keine Antwort vom Server:', error.request);
      throw new Error(`${errorMessage}: Keine Antwort vom Server`);
    } else {

      console.error('Fehler:', error.message);
      throw new Error(`${errorMessage}: ${error.message}`);
    }
  }
};
