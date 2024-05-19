import { useState, useEffect } from "react";
import SchemaOff from "../Resources/Data/ProtocollInProgress/protocol3.json";

import { useNavigate } from 'react-router-dom';
import { Routes, Route, Outlet, Link } from "react-router-dom";
import Interpreter from '../Components/Interpreter/Interpreter';

export default function PiPdialog({ header, text }) {
    const navigate = useNavigate();

    function Ok() {
        navigate("..");
    }



    return (

        <>

            <div className="dialog">
                <h2>{header}</h2>
                <p>{text}</p>
                <div className="row">
                    <button className="button" onClick={Ok}>Ok</button>
                </div>

            </div>
        </>
    )
}