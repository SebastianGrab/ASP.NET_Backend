import { faCheckToSlot } from '@fortawesome/free-solid-svg-icons';
import AuthContext from "../API/AuthProvider";
import { useContext, useEffect, useState } from "react";
import { getCall } from "../API/getCall";
import Tile from "../Components/Tile";

export default function ProtocolsToReview() {

    const [protocols, setProtocols] = useState(null);

    const {token, userID, orgaID, setUserID, setOrgaID, setToken} = useContext(AuthContext);


    useEffect(() => {
        const storedloginData = JSON.parse(localStorage.getItem('loginData'));
        console.log(storedloginData);
        if (storedloginData) {
            setToken(storedloginData.token);
            setUserID(storedloginData.userId);
        }
        getCall("/api/protocols?pageIndex=1&pageSize=9999999&IsDraft=false&IsClosed=false", token, "Error getting templates")
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

    return (
        <>

            <h1>Protokolle zur Überprüfung!</h1>

            {protocols !== null
                ? protocols.items.map((protocol) => (

                    <Tile
                        pagePath={String(protocol.id)}
                        icon={faCheckToSlot}
                        description={`Protokoll ${protocol.id}`}
                        info={`Protokoll erstellt am: ${protocol.createdDate.substring(0, 10)}`}
                        payload={protocol}
                        key={protocol.id}
                    />
                ))
                : null}
        </>
    )

}


