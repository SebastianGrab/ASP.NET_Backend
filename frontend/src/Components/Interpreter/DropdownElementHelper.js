import { useState, useEffect } from "react";


export default function DropdownElementHelper({ schemaObject }) {

    const [HelferList, setHelferList] = useState({});
    const [AnzahlHelfer, setAnzahlHelfer] = useState(schemaObject.HelperCollection.length);
    const [DropdownCollection, setDropdwonCollection] = useState(schemaObject.HelperCollection);

    useEffect(() => {
        fetch(schemaObject.Options)
            .then((response) => response.json())
            .then((data) => setHelferList(data))
            .catch((error) => console.error('Error fetching data: ', error));
    }, []);


    function handelAddHelper() {
        setAnzahlHelfer(AnzahlHelfer + 1);

            setDropdwonCollection( [
                ...DropdownCollection,
                 (schemaObject.ID + (AnzahlHelfer + 1 ))
              ]);

    }


    function handeDeleteHelper() {

        if (AnzahlHelfer > 1){
            setAnzahlHelfer(AnzahlHelfer  -  1);

            setDropdwonCollection( 
                DropdownCollection.filter(lastHelper => lastHelper !== (schemaObject.ID + (AnzahlHelfer)))
            );

        }


    }




    return (
        <>
            {DropdownCollection.map((valD) => (

                <div className="row" key={valD}>
                    <label htmlFor={valD}>{schemaObject.Label}</label>
                    {HelferList.Helfer ? (

                        <select id={valD} name={schemaObject.Element} {...(schemaObject.HelperNames && {value: schemaObject.HelperNames[ parseInt(valD.match(/\d+$/)[0])-1]})}>
                            {HelferList.Helfer.map((val) => (
                                <option key={val} value={val}>{val}</option>
                            ))}
                        </select>
                    ) : (
                        <select disabled>
                            <option value="">Lädt...</option>
                        </select>
                    )}

                    <select id={valD} name={schemaObject.Element} {...(schemaObject.Location  && {defaultValue: schemaObject.Location})}>
                        <option value="vor Ort" >vor Ort</option>
                        <option value="beim Patient" >beim Patient</option>
                    </select>
                </div>

            ))}
            <p></p>

            <div className="row">
            <input className="button-scnd" value="Helfer hinzufügen" type="button" onClick={handelAddHelper}></input>
            <input className="button-scnd" value="Helfer löschen" type="button" onClick={handeDeleteHelper}></input>

            </div>



        </>


    )





}