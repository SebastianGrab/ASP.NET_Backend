import { useState, useEffect } from "react";
import SchemaOff from "../Resources/Data/ProtocollInProgress/protocol3.json";

import { useNavigate } from 'react-router-dom';
import { Routes, Route, Outlet, Link } from "react-router-dom";
import Interpreter from '../Components/Interpreter/Interpreter';

export default function PiPdialog({ handelClick }) {
    const navigate = useNavigate();

    function send() {
        handelClick();
        navigate("..");
    }


    return (

        <>

            <div className="dialog">
                <h2>Protkoll absenden</h2>
                <p>Sie sind im Begriff, das Protokoll an Ihren Zugführer zu senden. Sie haben danach keien Möglochkeit mehr, Änderungen vorzunehmen.</p>
                <div className="row">
                    <button className="button" onClick={send}>Absenden</button>
                    <button className="button" onClick={handelClick}>Abbrechen</button>
                </div>

            </div>
        </>
    )
}