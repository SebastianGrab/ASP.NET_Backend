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
      // Server hat mit einem Statuscode geantwortet, der außerhalb des Bereichs 2xx liegt
      console.error('Fehlerhafte Antwort vom Server:', error.response.data);
      throw new Error(`${errorMessage}: ${error.response.data}`);
    } else if (error.request) {
      // Anfrage wurde gesendet, aber es kam keine Antwort zurück
      console.error('Keine Antwort vom Server:', error.request);
      throw new Error(`${errorMessage}: Keine Antwort vom Server`);
    } else {
      // Etwas anderes hat den Fehler verursacht
      console.error('Fehler:', error.message);
      throw new Error(`${errorMessage}: ${error.message}`);
    }
  }
};
