import { useState, useEffect } from "react";
import SchemaOff from "../Resources/Data/ProtocollInProgress/protocol3.json";
import PiPdialog from "./PiPdialog";


import { Routes, Route, Outlet, Link } from "react-router-dom";
import Interpreter from '../Components/Interpreter/Interpreter';

export default function PiP4() {


    const [Schema, setSchema] = useState({});
    const [serverUrl, setServerUrl] = useState("/Data/protocol.json");
    const [showDialog, setShowDialog] = useState(false);

    useEffect(() => {
        fetch(serverUrl)
            .then((response) => response.json())
            .then((data) => setSchema(data))
            .catch((error) => console.error('Error fetching data: ', error));
    }, [serverUrl]);

    const handleSend = () => {
        setShowDialog(!showDialog);

    }

    return (

        <>
            <div className="row">
                <input className="button" value="Protokoll absenden!" type="button" onClick={handleSend}></input>

            </div>

            <h1>Protokoll 4</h1>

            <Interpreter schema={SchemaOff} />

            <div className="row">
                <input className="button" value="Protokoll absenden!" type="button" onClick={handleSend}></input>

            </div>



            {showDialog && <PiPdialog handelClick={handleSend}/>}
        </>
    )
}