import { useState, useEffect } from 'react';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faCircle } from '@fortawesome/free-solid-svg-icons';
import InputElement from './InputElement';
import DropdownElementHelper from './DropdownElementHelper';
import DropdownElement from './DropdownElement';
import InTileLabel from './InTileLabel';
import Textarea from './Textarea';

export default function TileCategory({  category }) {
    let message = 'Lorem ipsum dolor sit amet, consetetur sadipscing elitr, sed diam nonumy eirmod tempor invidunt ut labore et dolore magna aliquyam erat, sed diam voluptua. At vero eos et accusam et justo duo dolores et ea rebum. Stet clita kasd gubergren';

    const inputIDs = [];
    const [showInputs, setShowMessageBody] = useState(false);
    const [numberOfInputs, setNumberOfInputs] = useState(0);
    const [numberOfValues, setNumberOfValues] = useState(0);

    function showBody() {
        setShowMessageBody(true);
    }

    function hideBody() {
        setShowMessageBody(false);
    }

    const getInputs = () => {
            setNumberOfInputs(category.Inputs.length);

            category.Inputs.forEach((input) => {
                if(input.Value){
                    setNumberOfValues(numberOfValues+1);
                    console.log(input);
                }
            });

    }

    useEffect(() => {
        getInputs();
    }, []);

    function handleSaveSection() {
        const inputValues = {};

        // Durchgehe alle Inputs in einer Kategorie aus JSON
        category.Inputs.forEach((input) => {
            //Prüfung ob es sich um KEIN Label handelt --> Labels haben kein .type
            if(input.Element !== "label"){
                const inputElement = document.getElementById(input.ID);
                console.log(inputElement.type);
    
                // Füge den Wert des Inputs dem Objekt inputValues hinzu
                if(inputElement.type === "checkbox" || inputElement.type === "radio"){
                    inputValues[input.ID] = inputElement.checked
                } else {
                    inputValues[input.ID] = inputElement.value
    
                }
            }

        });
        console.log(inputValues);
    }


    return (
        <>
        
            <div className="tile-message">
                <div className="message-head" onClick={showBody}>
                    <h2>{category.Kategorie}</h2>
                    <p>{numberOfValues}/{numberOfInputs}</p>
                </div>

                <div className={`category-body ${showInputs ? 'open' : ''}`} id={category.ID}>
                    {
                        category.Inputs.map((input) => (
                            inputIDs.push(input.ID), 
                            input.Element === "input" ? (
                                <InputElement key={input.ID} schemaObject={input} />
                            ) : input.Element === "dropdownHelper" ? (
                                <DropdownElementHelper key={input.ID} schemaObject={input} />
                            ) : input.Element === "dropdown" ? (
                                <DropdownElement key={input.ID} schemaObject={input} />
                            ) : input.Element === "label" ? (
                                <InTileLabel key={input.ID} schemaObject={input} />
                            ) : input.Element === "textarea" ? (
                                <Textarea key={input.ID} schemaObject={input} />
                            ): null
                        ))}

                </div>
                <p></p>

                <div className='row'>
                    {/* 
                                    <input className="button" value="Abschnitt speichern" type="button" onClick={handleSaveSection}></input>
                    */}


                    <div className={`category-buttons ${showInputs ? '' : 'hide'}`} onClick={hideBody}>Einklappen</div>
                    <div className={`category-buttons ${showInputs ? 'hide' : ''}`} onClick={showBody}>Ausklappen</div>
                </div>




            </div>


        </>
    )
}