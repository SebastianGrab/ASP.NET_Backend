import { useState, useEffect } from "react";
import schema from "../Resources/Data/protocol2.json";

import { saveProtocol } from '../API/saveProtocol';
import { Routes, Route, Outlet, Link } from "react-router-dom";
import Interpreter from '../Components/Interpreter/Interpreter';
import SuccesDialog from '../Components/SuccesDialog';


export default function NewProtocol() {

    const [showDialog, setShowDialog] = useState(false);

    const [Schema, setSchema] = useState({});
    const [serverUrl, setServerUrl] = useState("/Data/protocol.json");
    let SchemaData = schema;

    useEffect(() => {
        fetch(serverUrl)
            .then((response) => response.json())
            .then((data) => setSchema(data))
            .catch((error) => console.error('Error fetching data: ', error));
    }, [serverUrl]);


    const handleSave = () => {
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

        console.log(inputValues);
        console.log(SchemaData);
        saveProtocol(SchemaData);
        setShowDialog(true);
    }




    return (

        <>

            <h1>Neues Protokoll!</h1>

                <Interpreter schema={schema} />

                <div className="row">
                <input className="button" value="Protokoll speichern" type="button" onClick={handleSave}></input>

            </div>

                {showDialog && <SuccesDialog header="Protokoll wurde erfoglreich gespeichert!" text="Protokoll wurde erfoglreich gespeichert!"/>}

                
        </>
    )
}