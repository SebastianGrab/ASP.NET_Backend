import { Routes, Route, Outlet, Link } from "react-router-dom";
import Tile from "../Components/Tile";
import { faPenToSquare } from '@fortawesome/free-solid-svg-icons';
import { getAllProtocolsInProgress } from '../API/archive/getAllProtocolsInProgress';
import AuthContext from "../API/AuthProvider";
import { useContext, useEffect, useState } from "react";
import { getCall } from "../API/getCall";



export default function ProtocolInProgress(){

    const [protocols, setProtocols] = useState(null);

    const { token, userID, orgaID, setUserID, setOrgaID, setToken} = useContext(AuthContext);

  
    useEffect(() => {
      const storedloginData = JSON.parse(localStorage.getItem('loginData'));
      console.log(storedloginData);
      if (storedloginData) {
        setToken(storedloginData.token);
        setOrgaID(storedloginData.organizationId);
        setUserID(storedloginData.userId);
      }
      getCall("/api/user/" + userID + "/all-protocols?pageIndex=1&pageSize=50", token, "Error getting templates")
        .then((response) => {
          setProtocols(response);
          console.log("Get Templates successfull!");
          console.log(response.items[0].id);
        })
        .catch((error) => {
          console.error(error);
        });
    }, [orgaID]);

    console.log(protocols);

    return(
        <>

        <h1>Protokolle in Bearbeitung!</h1>

        {protocols !== null
        ? protocols.items.map((protocol) => (
          
            <Tile
              pagePath={String(protocol.id)}
              icon={faPenToSquare}
              description={`Protokoll vom: ${protocol.createdDate.substring(0, 10)}`}
              payload={protocol}
              key={protocol.id}
            />
          ))
        : null}
        </>
    )
}