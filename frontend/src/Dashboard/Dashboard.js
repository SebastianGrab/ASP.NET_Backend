import Tile from "../Components/Tile";
import { useNavigate, Navigate, Link, redirect } from "react-router-dom";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faFileCirclePlus, faFilePen, faEnvelope, faBoxArchive,faChartSimple, faUser, faShop, faCheckToSlot, faFileShield } from '@fortawesome/free-solid-svg-icons';
import StoreIcon from '@mui/icons-material/Store';
import React, { useState, useEffect, useContext } from 'react';
import AuthContext from "../API/AuthProvider";

export default function Dashboard() {
    const { token, setToken, userID, orgaID, setRefreshHandler, role } = useContext(AuthContext);

    const adminTiles = () => {
        return(
            <>
            <Tile pagePath='protocolsToReview' icon={faCheckToSlot} description="Protokolle zur Überprüfung" />
            <Tile pagePath='userOverview' icon={faUser} description="Nutzer Übersicht" />
            <Tile pagePath='organizationsOverview' icon={faShop} description="Organisations Übersicht" />
            <Tile pagePath='templateManager' icon={faFileShield} description="Template Übersicht" />
            <Tile pagePath='stats' icon={faChartSimple} description="Gesamtstatistik" />
            </>

        )
        
    }

    return (
        <>

            <div className='container'>
                <h1>Dashboard</h1>

                <Tile pagePath='newProtocol' icon={faFileCirclePlus} description="Neues Protokoll" />
                <Tile pagePath='protocolInProgress' icon={faFilePen} description="Protokoll in Bearbeitung" />
                <Tile pagePath='messages' icon={faEnvelope} description="Benachrichtigungen" />
                <Tile pagePath='archive' icon={faBoxArchive} description="Archiv" />

                {role === "Helfer"
                ?             <Tile pagePath='userStats' icon={faChartSimple} description="Statistiken" />
            : null}
    

                {role === "Admin" || role === "Leiter"
                ? adminTiles()
            : null}




            </div>

        </>
    )
}