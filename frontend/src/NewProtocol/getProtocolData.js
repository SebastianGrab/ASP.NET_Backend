
export const getData = (SchemaData) => {

    const inputValues = {};
    let helpers = 1;

    // Durchgehe alle Inputs in einer Kategorie aus JSON
    SchemaData.Schema.forEach((category) => {
        if (category.ID === "EINSATZHELFER") {
            const inputCategory = document.getElementById(category.ID);
            helpers = inputCategory.children.length - 2;
        }
        category.Inputs.forEach((input) => {
            //Prüfung ob es sich um KEIN Label handelt --> Labels haben kein .type
            if (input.Element !== "label" && input.Element !== "dropdownHelper") {
                const inputElement = document.getElementById(input.ID);

                // Füge den Wert des Inputs dem Objekt inputValues hinzu
                if ((inputElement.type === "checkbox" || inputElement.type === "radio") && inputElement.checked) {
                    input.Value = true;
                    inputValues[input.ID] = inputElement.checked
                } else if ((inputElement.type === "text" || inputElement.type === "number" || inputElement.type === "time" || inputElement.type === "date") && inputElement.value) {
                    input.Value = inputElement.value;
                    inputValues[input.ID] = inputElement.value
                } else if (inputElement.name === "dropdown" && inputElement.value !== "-") {
                    input.Value = inputElement.value;
                }
            }

            if (input.Element === "dropdownHelper") {
                input.HelperCollection = [];
                input.HelperNames = []

                for (let i = 1; i < helpers +1 ; i++) {
                    const inputHelper = document.getElementById(input.ID + i)
                    input.HelperCollection.push(inputHelper.id);
                    input.HelperNames.push(inputHelper.value);
                }
            }


        })

    });

    return SchemaData;

}