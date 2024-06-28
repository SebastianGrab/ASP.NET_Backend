import { useState, useEffect, useContext } from "react";
import AuthContext from "../../API/AuthProvider";
import { getCall } from "../../API/getCall";

export const DropdownElementHelper = ({ schemaObject }) => {
    const { token, orgaID} = useContext(AuthContext);

    const [HelferList, setHelferList] = useState({});
    const [AnzahlHelfer, setAnzahlHelfer] = useState(schemaObject.HelperCollection.length);
    const [DropdownCollection, setDropdwonCollection] = useState(schemaObject.HelperCollection);
    const [selectedHelpers, setSelectedHelpers] = useState(schemaObject.HelperNames || []);

    useEffect(() => {
        const getHelpers = async () => {

            try{
                const response = await getCall(`/api/organization/${orgaID}/users?pageIndex=1&pageSize=100`, token, "Error getting Users");
                setHelferList(response);

            } catch(error) {
                console.log(error)

            }

        };

        getHelpers();
    }, [token, orgaID]);

    function handelAddHelper() {
        setAnzahlHelfer(AnzahlHelfer + 1);
        setDropdwonCollection([
            ...DropdownCollection,
            schemaObject.ID + (AnzahlHelfer + 1)
        ]);
        setSelectedHelpers([
            ...selectedHelpers,
            ""
        ]);
    }

    function handeDeleteHelper() {
        if (AnzahlHelfer > 1) {
            setAnzahlHelfer(AnzahlHelfer - 1);
            setDropdwonCollection(
                DropdownCollection.filter((_, index) => index !== DropdownCollection.length - 1)
            );
            setSelectedHelpers(
                selectedHelpers.slice(0, -1)
            );
        }
    }

    function handleHelperChange(index, value) {
        const newSelectedHelpers = [...selectedHelpers];
        newSelectedHelpers[index] = value;
        setSelectedHelpers(newSelectedHelpers);
    }

    return (
        <>
            {DropdownCollection.map((valD, index) => (
                <div className="row" key={valD}>
                    <label htmlFor={valD}>{schemaObject.Label}</label>
                    {HelferList.items ? (
                        <select
                            id={valD}
                            name={schemaObject.Element}
                            value={selectedHelpers[index] || ""}
                            onChange={(e) => handleHelperChange(index, e.target.value)}
                        >
                            {HelferList.items.map((val) => (
                                <option key={val.id} value={val.username}>{val.username}</option>
                            ))}
                        </select>
                    ) : (
                        <select disabled>
                            <option value="">Lädt...</option>
                        </select>
                    )}
                    <select id={valD} name={schemaObject.Element} defaultValue={schemaObject.Location}>
                        <option value="vor Ort">vor Ort</option>
                        <option value="beim Patient">beim Patient</option>
                    </select>
                </div>
            ))}
            <p></p>
            <div className="row">
                <input className="button-scnd" value="Helfer hinzufügen" type="button" onClick={handelAddHelper} />
                <input className="button-scnd" value="Helfer löschen" type="button" onClick={handeDeleteHelper} />
            </div>
        </>
    );
};
