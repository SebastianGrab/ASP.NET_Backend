import axios from 'axios';
import baseURL from './baseURL';

export const putCall = async (data, endpoint, errorMsg, token) => {
  try {
    const response = await axios.put(`${baseURL}${endpoint}`, data, {
      headers: {
        'Authorization': `Bearer ${token}`,
        'Content-Type': 'application/json'
      }
    });
    return response.data;
  } catch (error) {
    console.error(errorMsg, error);
    throw new Error(errorMsg);
  }
};