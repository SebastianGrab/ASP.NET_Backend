import { useState, useEffect, useContext } from "react";
import { useLocation } from "react-router-dom";
import { getCall } from "../API/getCall";
import AuthContext from "../API/AuthProvider";
import ReviewInterpreter from "../Leader/ReviewInterpreter/ReviewInterpreter"; 

export default function ArchiveProtocol() {
    const { token, userID, orgaID, setUserID, setOrgaID, setToken } = useContext(AuthContext);
    const [protocolContent, setProtocolContent] = useState(null);
    const location = useLocation();
    const protocolData = location.state?.payload;
    const protocolID = protocolData?.id;


    useEffect(() => {
        const storedLoginData = JSON.parse(localStorage.getItem("loginData"));
        if (storedLoginData) {
            setToken(storedLoginData.token);
            setUserID(storedLoginData.userId);
        }

        getCall(`/api/protocol/${protocolID}/content`, token, "Error getting templates")
            .then((response) => {
                setProtocolContent(JSON.parse(response.content));
                console.log("Get Templates successful!");
            })
            .catch((error) => {
                console.error(error);
            });
    }, [protocolID, setToken, setOrgaID, setUserID, token]);


    return (
        <>
            <h1>Protokoll {protocolID}</h1>
            {protocolContent !== null ? (
                <ReviewInterpreter schema={protocolContent} />
            ) : null}

        </>
    );
}
