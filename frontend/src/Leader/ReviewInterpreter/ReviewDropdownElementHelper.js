import React, { useState, useEffect, useContext } from 'react';
import { getCall } from '../../API/getCall';
import AuthContext from '../../API/AuthProvider';

export default function ReviewDropdownElementHelper({ schemaObject }) {
    const { token, orgaID } = useContext(AuthContext);
    const [HelferList, setHelferList] = useState({});
    const [DropdownCollection, setDropdownCollection] = useState(schemaObject.HelperCollection);

    useEffect(() => {
        const getHelpers = async () => {
            const response = await getCall(`/api/organization/${orgaID}/users?pageIndex=1&pageSize=100`, token, "Error getting Users");
            setHelferList(response);
        };
        getHelpers();
    }, [token, orgaID]);

    return (
        <>
            {DropdownCollection.map((valD) => (
                <div className="row" key={valD}>
                    <label htmlFor={valD}>{schemaObject.Label}</label>
                    {HelferList.items ? (
                        <select id={valD} name={schemaObject.Element} disabled>
                            {HelferList.items.map((val) => (
                                <option key={val.id} value={val.username} selected={schemaObject.HelperNames && schemaObject.HelperNames[parseInt(valD.match(/\d+$/)[0]) - 1] === val.username}>
                                    {val.username}
                                </option>
                            ))}
                        </select>
                    ) : (
                        <select disabled>
                            <option value="">Lädt...</option>
                        </select>
                    )}
                    <select id={valD} name={schemaObject.Element} disabled defaultValue={schemaObject.Location || "vor Ort"}>
                        <option value="vor Ort">vor Ort</option>
                        <option value="beim Patient">beim Patient</option>
                    </select>
                </div>
            ))}
        </>
    );
}
