import React from 'react';

export default function ReviewTextarea({ schemaObject }) {
    return (
        <div className="row">
            <label htmlFor={schemaObject.ID}>
                {schemaObject.Label} {schemaObject.Mandatory === 1 && <span style={{ color: 'red' }}>*</span>}
            </label>
            <textarea
                id={schemaObject.ID}
                defaultValue={schemaObject.Value}
                readOnly
                disabled
            />
        </div>
    );
}
