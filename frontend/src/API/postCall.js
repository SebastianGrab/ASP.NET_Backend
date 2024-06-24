// api.js
import axios from 'axios';
import baseURL from './baseURL';

export const postCall = async (data, endpoint, errorMessage, token) => {
  try {
    const response = await axios.post(`${baseURL}${endpoint}`, data, {
      headers: {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${token}`,
      }
    });

    console.log('Erfolgreich');
    return response.data;


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
