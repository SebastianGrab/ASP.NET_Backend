import Tile from "../Components/Tile";
import { useNavigate, Navigate, Link, redirect } from "react-router-dom";
import { useEffect, useState } from "react";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faFileCirclePlus, faFilePen, faEnvelope, faBoxArchive,faChartSimple } from '@fortawesome/free-solid-svg-icons';


export default function Dashboard() {


    return (
        <>

            <div className='container'>
                <h1>Dashboard</h1>

                <Tile pagePath='newProtocol' icon={faFileCirclePlus} description="Neues Protokoll" />
                <Tile pagePath='protocolInProgress' icon={faFilePen} description="Protokoll in Bearbeitung" />
                <Tile pagePath='messages' icon={faEnvelope} description="Benachrichtigungen" />
                <Tile pagePath='archive' icon={faBoxArchive} description="Archiv" />
                <Tile pagePath='stats' icon={faChartSimple} description="Statistiken" />

            </div>

        </>
    )
}