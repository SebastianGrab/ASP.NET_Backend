import { useState, useEffect, useContext } from 'react';
import AuthContext from "../API/AuthProvider";
import { useNavigate } from 'react-router-dom';
import { getCall } from '../API/getCall';
import PdfDownload from "../Pdf/PdfDownload";

export default function TileArchive({ pagePath, icon, description, info, payload }) {
    const navigate = useNavigate();
    const { token, userID, orgaID, setUserID, setOrgaID, setToken } = useContext(AuthContext);
    const [protocolContent, setProtocolContent] = useState(null);

    useEffect(() => {
        const storedLoginData = JSON.parse(localStorage.getItem("loginData"));
        if (storedLoginData) {
            setToken(storedLoginData.token);
            setUserID(storedLoginData.userId);
        }

        const getProtocol = async () => {
            const response = await getCall(`/api/protocol/${payload.id}/content`, token, "Error getting templates")
            setProtocolContent(JSON.parse(response.content));
        }

        getProtocol();


    }, [ setToken, setOrgaID, setUserID, token]);




    function clickHandler() {
        if (payload === null) {
            navigate(pagePath);
        } else {
            navigate(pagePath, {state: {payload}});
        }
    }



        return (
        <>
            <div className="tile" onClick={clickHandler}>
                <div className="row">
                    <h3>Protokoll {description}</h3> {/* Beschreibung des Protokolls */}
                </div>
                <div className="archive-body">

                    {info} {/* Zusätzliche Info */}

                    {protocolContent !==null ? <PdfDownload protocol={protocolContent}></PdfDownload> :null}
                </div>

            </div>
        </>
    )
}
