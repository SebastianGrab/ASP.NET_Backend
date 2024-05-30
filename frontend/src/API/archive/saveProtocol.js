// api.js
import baseURL from '../baseURL';

export const saveProtocol = async (protocolData, handleSave) => {
  try {
    const response = await fetch(`${baseURL}/api/protocols`, {
      method: 'POST',
      mode: 'cors',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(protocolData),
    });
    if (!response.ok) {
      throw new Error('Fehler beim Speichern des Protokolls');
    }
    console.log('Erfolgreich');
    handleSave();
    return await response.json();
  } catch (error) {
    console.error('Fehler:', error);
    return null;
  }
};
