
import AuthContext from "../API/AuthProvider";
import { useContext, useEffect, useState } from "react";
import { getCall } from "../API/getCall";
import TileMessages from '../Components/TileMessages'
import { putCall } from "../API/putCall"


export default function Messages() {
    const [protocols, setProtocols] = useState(null);
    const [clickedProtocols, setClickedProtocols] = useState([]);

    const {token, userID, orgaID, setUserID, setOrgaID, setToken} = useContext(AuthContext);

    useEffect(() => {
        const storedloginData = JSON.parse(localStorage.getItem('loginData'));

        console.log(storedloginData);
        if (storedloginData) {
            setToken(storedloginData.token);
            setUserID(storedloginData.userId);
        }
        if (token && userID) {
            getCall("/api/user/" + userID + "/all-protocols?pageIndex=1&pageSize=9999999", token, "Error getting templates")
                .then((response) => {
                    const filteredProtocols = response.items.filter(protocol =>
                        protocol.isClosed || (protocol.isDraft && (protocol.reviewComment !== "string"))
                    );
                    setProtocols(filteredProtocols);
                    console.log("Get Protocols successful!");
                    console.log(filteredProtocols);
                })
                .catch((error) => {
                    console.error(error);
                });
        }
    }, [orgaID]);

    console.log(protocols);

    const handleTileClick = (protocolId) => {
        const protocol = protocols.find(p => p.id === protocolId);
        if (!protocol || protocol.reviewCommentIsRead || protocol.isClosed) {
            return; // Wenn das Protokoll geschlossen ist oder bereits gelesen wurde, beende die Funktion
        }
        if (protocol && !protocol.reviewCommentIsRead) {
            getCall(`/api/protocol/${protocolId}`, token, "Fehler beim Markieren des Protokolls als gelesen")
                .then((response) => {
                    const updatedProtocolData = {
                        ...response,
                        reviewCommentIsRead: true
                    };
                    putCall(updatedProtocolData, `/api/protocol/${protocolId}`, "Fehler beim Markieren des Protokolls als gelesen", token)
                        .then(() => {
                            setProtocols(protocols.map(p =>
                                p.id === protocolId ? { ...p, reviewCommentIsRead: true } : p
                            ));
                        })
                        .catch(error => {
                            console.error("Fehler beim Markieren des Protokolls als gelesen", error);
                        });
                })
                .catch((error) => {
                    console.error("Fehler beim Markieren des Protokolls als gelesen", error);
                });
        }
    };

    return (
        <>
        <h1>Benachrichtigungen</h1>
        <div>
            {protocols ? (
                protocols.map(protocol => (
                    <TileMessages
                        key={protocol.id}
                        heading={`Protokoll ${protocol.id}`}
                        statusClass={protocol.isClosed ? 'status-good' : 'status-bad'}
                        pagePath={protocol.isClosed ? `/${userID}/${orgaID}/archive/${protocol.id}` : `/${userID}/${orgaID}/protocolInProgress/${protocol.id}`}
                        payload={protocol}
                        reviewComment={protocol.isClosed ? "Das Protokoll wurde vom Zugführer genehmigt." : protocol.reviewComment}
                        isClicked={protocol.reviewCommentIsRead}
                        onClick={() => handleTileClick(protocol.id)}

                    />
                ))
            ) : (
                <p>Loading...</p>
            )}
        </div>
        </>
    );

}