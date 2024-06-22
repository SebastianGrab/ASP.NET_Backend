import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faPen } from '@fortawesome/free-solid-svg-icons';

export default function ReviewInputElement({ schemaObject }) {
    return (
        <>
            <div className="row">
                <label htmlFor={schemaObject.ID}>
                    {schemaObject.Label} {schemaObject.Mandatory === 1 && <span style={{ color: 'red' }}>*</span>}
                </label>
                <div>
                    <input
                        id={schemaObject.ID}
                        type={schemaObject.Type}
                        disabled
                        readOnly
                        {...(schemaObject.Value && (schemaObject.Type === "text" || schemaObject.Type === "number" || schemaObject.Type === "time" || schemaObject.Type === "date") && { defaultValue: schemaObject.Value })}
                        {...(schemaObject.Value && schemaObject.Type === "checkbox" && { checked: true })}
                        {...(schemaObject.Value && schemaObject.Type === "radio" && { checked: true })}
                        {...(schemaObject.RadioGroup && { name: schemaObject.RadioGroup })}
                    />
                </div>
            </div>
        </>
    )
}
