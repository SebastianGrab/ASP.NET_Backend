import axios from 'axios';
import baseURL from './baseURL';

export const getCall = async (endpoint, token, errorMessage) => {
  try {
    const response = await axios.get(`${baseURL}${endpoint}`, {
      headers: {
        'Authorization': `Bearer ${token}`,
      },
    });

    return response.data;

  } catch (error) {
    if (error.response) {

      console.error('Fehlerhafte Antwort vom Server:', error.response.data);
      throw new Error(errorMessage || `Failed to fetch data: ${error.response.data}`);
    } else if (error.request) {

      console.error('Keine Antwort vom Server:', error.request);
      throw new Error(errorMessage || 'Failed to fetch data: No response from server');
    } else {

      console.error('Fehler:', error.message);
      throw new Error(errorMessage || `Failed to fetch data: ${error.message}`);
    }
  }
};
