import { Routes, Route, Outlet, Link } from "react-router-dom";
import Tile from "../Components/Tile";
import { faPenToSquare } from '@fortawesome/free-solid-svg-icons';
import { getAllProtocolsInProgress } from '../API/getAllProtocolsInProgress';
import { useEffect, useState } from "react";

export default function ProtocolInProgress(){

    const [protocols, setProtocols] = useState([]);

    useEffect(() => {
        const fetchData = async () => {
          const data = await getAllProtocolsInProgress();
          if (data) {
            setProtocols(data);
            console.log(data);
          }
        };
        fetchData();
      }, []);

    return(
        <>

        <h1>Protokolle in Bearbeitung!</h1>

        <Tile pagePath='3' icon={ faPenToSquare } description="Protokoll vom 12.04.2024" />
        <Tile pagePath='4' icon={ faPenToSquare } description="Protokoll vom 18.04.2024" />
        </>
    )
}