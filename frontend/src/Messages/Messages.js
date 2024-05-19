import { Routes, Route, Outlet, Link } from "react-router-dom";
import TileMessages from '../Components/TileMessages'
import {faPenToSquare} from "@fortawesome/free-solid-svg-icons";

export default function Messages() {

    return (

        <>

            <h1>Benachrichtigungen</h1>

            <TileMessages heading = {"Protokoll vom 18.04.2024 fehlerhaft"} status ={"status-bad"} pagePath='../protocolInProgress/3' />
            <TileMessages heading = {"Protokoll vom 30.03.2024 wurde bestätigt"} status ={"status-good"} pagePath='../protocolInProgress/4'/>


        </>



    )
}