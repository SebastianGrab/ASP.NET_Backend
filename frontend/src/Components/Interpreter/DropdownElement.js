import { useState} from "react";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPen } from '@fortawesome/free-solid-svg-icons';



export default function DropdownElement({ schemaObject }) {

    const [editInput, setEditInput] = useState(true)

    const handelEditClick = () => {
        setEditInput(false)
    }


    return (
        <>
            <div className="row">
                <label htmlFor={schemaObject.ID}>{schemaObject.Label}</label>
                <div>


                    {schemaObject.Value && editInput && <FontAwesomeIcon className="edit-icon" icon={faPen} onClick={handelEditClick} />}

                    <select id={schemaObject.ID} name={schemaObject.Element} {...(schemaObject.Value && editInput && { disabled: true, value: schemaObject.Value })}>
                        {schemaObject.Options.map((val) => (
                            <option key={val} value={val} >{val}</option>
                        ))}
                    </select>
                </div>


            </div>




        </>
    )




}