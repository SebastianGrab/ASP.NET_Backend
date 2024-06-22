import React from 'react';
import {FontAwesomeIcon} from "@fortawesome/react-fontawesome";
import {faPen} from "@fortawesome/free-solid-svg-icons";

export default function ReviewDropdownElement({ schemaObject }) {
    return (
        <div className="row">
            <label htmlFor={schemaObject.ID}>
                {schemaObject.Label}
            </label>


            <select id={schemaObject.ID} name={schemaObject.Element} {...(schemaObject.Value && { disabled: true, value: schemaObject.Value })}>
                {schemaObject.Options.map((val) => (
                    <option key={val} value={val} >{val}</option>
                ))}
            </select>
        </div>
    );
}