// api.js
import baseURL from './baseURL';



export const sendLogIn = async (logInData) => {
    
  try {
    const response = await fetch(`${baseURL}/api/login`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(logInData),
    });
    if (!response.ok) {
      throw new Error('Fehler beim LogIn');
    }
    console.log('Erfolgreich');

    return await response.json();
  } catch (error) {

    console.error('Fehler:', error);
    return null;
  }
};
