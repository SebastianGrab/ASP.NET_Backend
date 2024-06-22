import baseURL from '../baseURL';

export const getAllProtocolsInProgress = async () => {
    try {
      const response = await fetch(`${baseURL}/api/protocols`);
      if (!response.ok) {
        throw new Error('Fehler beim Laden der Protokolle');
      }
      return await response.json();
    } catch (error) {
      console.error('Fehler:', error);
      return null;
    }
  };