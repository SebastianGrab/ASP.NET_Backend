import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPen } from '@fortawesome/free-solid-svg-icons';
import { useState } from 'react';


export default function Textarea({ schemaObject }) {

    const [editInput, setEditInput] = useState(true)

    const handelEditClick = () => {
        setEditInput(false)
    }

    return (
        <>
            <div className="row">
                <label htmlFor={schemaObject.ID}>{schemaObject.Label}</label>

                {schemaObject.Value && editInput && <FontAwesomeIcon className="edit-icon" icon={faPen} onClick={handelEditClick} />}
                <textarea id={schemaObject.ID}
                    {...(schemaObject.Value && editInput &&
                        { value: schemaObject.Value })}
                    name={schemaObject.Name} rows={schemaObject.Rows} cols={schemaObject.Cols}>

                </textarea>
                <br />

            </div>




        </>
    )





}