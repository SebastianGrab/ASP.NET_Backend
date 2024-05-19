import { useState } from "react"
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPen } from '@fortawesome/free-solid-svg-icons';




export default function InputElement({ schemaObject }) {

    const [editInput, setEditInput] = useState(true)

    const handelEditClick = () => {
        setEditInput(false)
    }




    return (
        <>
            <div className="row">
                <label htmlFor={schemaObject.ID}>{schemaObject.Label}</label>


                <div>
                    {/*Das value-attribut des Input Elements  & Edit-Icon wird gerendert, wenn: JSON-Attribut: Value existiert.
                    Falls Werte ausgefüllt worden sind, sind die Inputs gesperrt. Mit Klick auf das Edit Icon wird es wieder freigegeben
                     */}

                    {schemaObject.Value && editInput && <FontAwesomeIcon className="edit-icon" icon={faPen} onClick={handelEditClick} />}
                    <input id={schemaObject.ID} type={schemaObject.Type}
                    
                        {...(schemaObject.Value && editInput &&
                            (schemaObject.Type === "text" || schemaObject.Type === "number" || schemaObject.Type === "time" || schemaObject.Type === "date") &&
                            { defaultValue: schemaObject.Value, readOnly: true })}
                        {...(schemaObject.Value && editInput &&
                            (schemaObject.Type === "checkbox") &&
                            { checked: true, readOnly: true })}
                        {...(schemaObject.Value && editInput &&
                            (schemaObject.Type === "radio") &&
                            { checked: true, disabled: true })}
                        {...(schemaObject.RadioGroup && { name: schemaObject.RadioGroup })} />
                </div>


            </div>




        </>
    )





}