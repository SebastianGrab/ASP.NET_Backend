// api.js
import baseURL from '../baseURL';



export const sendLogIn = (logInData) => {
  return fetch(`${baseURL}/api/login`, {
    method: 'POST',
    mode: 'cors',
    headers: {
      'Content-Type': 'application/json'
    },
    body: JSON.stringify(logInData),
  })
  .then((response) => {
    if (!response.ok) {
      throw new Error('Fehler beim LogIn');
    }
    
    return response.json();
  })
  .catch((error) => {
    console.error('Fehler:', error);
    return null;
  });
};
