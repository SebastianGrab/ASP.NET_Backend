export const handleFileUpload = (e, setTemplate) => {
    const file = e.target.files[0];
    if (!file) return;  // Check if a file is actually selected

    const reader = new FileReader();
    reader.onload = (event) => {
        try {
            const json = event.target.result;
            setTemplate(json)
        } catch (error) {
            console.error("Invalid JSON file");
        }
    };
    reader.readAsText(file);
};


const handleCreateTemplate = async (template) => {

 
};

export const buildTemplateData = (template, name, description) => {
    const data = {
        name: name,
        description: description
    };
    const stringData = JSON.stringify(data);
    return data;

}
