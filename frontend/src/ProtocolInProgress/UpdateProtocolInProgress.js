import { useState, useEffect, useContext } from "react";
import { useLocation } from "react-router-dom";
import SchemaOff from "../Resources/Data/protocol2.json";
import PiPdialog from "./PiPdialog";
import { getData } from "../NewProtocol/getProtocolData";
import { putCall } from "../API/putCall";
import AuthContext from "../API/AuthProvider";
import { Routes, Route, Outlet, Link } from "react-router-dom";
import Interpreter from '../Components/Interpreter/Interpreter';


export default function UpdateProtocolInProgress() {
    const { token, userID, orgaID, setUserID, setOrgaID, setToken} = useContext(AuthContext);
    const temp = SchemaOff;
    const protData = null;

    
const location = useLocation();
const protocolData = location.state?.payload;
const protocolID = protocolData.id;


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

            const data = {
                "protocolId": protocolID,
                "content": getData(temp)
            };
        
            putCall(data, "/api/protocol/" + protocolID + "/content'", "Fehler beim Updaten", token)
            .then((response) => {
                console.log(response);
              })
              .catch((error) => {
                console.error(error);
              });
              setShowDialog(!showDialog);

        };

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