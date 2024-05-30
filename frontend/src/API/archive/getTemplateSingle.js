import baseURL from './baseURL';

export const getTemplateSingle = async (templateId) => {
    try {
      const response = await fetch(`${baseURL}/api/template/${templateId}`);
      if (!response.ok) {
        throw new Error('Fehler beim Laden des Templates');
      }
      return await response.json();
    } catch (error) {
      console.error('Fehler:', error);
      return null;
    }
  };
  