import { useState, useEffect, useContext } from "react";
import { useLocation, useNavigate } from "react-router-dom";
import CommentPopup from "./CommentPopup";
import { getData } from "../NewProtocol/NewProtocolService";
import { putCall } from "../API/putCall";
import { getCall } from "../API/getCall";
import AuthContext from "../API/AuthProvider";
import ReviewInterpreter from "./ReviewInterpreter/ReviewInterpreter";
import SuccesDialog from "../Components/SuccesDialog";
import PdfViewer from "../Pdf/PdfViewer";

export default function ReviewProtocol() {
    const { token, userID, orgaID, setUserID, setOrgaID, setToken } = useContext(AuthContext);
    const [protocolContent, setProtocolContent] = useState(null);
    const [showDialog, setShowDialog] = useState(false);
    const location = useLocation();
    const protocolData = location.state?.payload;
    const protocolID = protocolData?.id;


    const navigate = useNavigate();
    const [comment, setComment] = useState("");
    const [showSuccessDialog, setShowSuccessDialog] = useState(false);



    const handleSaveToArchive = () => {
        getCall(`/api/protocol/${protocolID}`, token, "Fehler beim Archivieren")
            .then((response) => {
                // Aktualisiere die Daten mit isClosed: true
                const updatedProtocolData = {
                    ...response,
                    isClosed: true,
                    isDraft: false
                };
                // Sende die aktualisierten Daten an den Server
                putCall(updatedProtocolData, `/api/protocol/${protocolID}`, "Fehler beim Archivieren", token)
                    .then(response => {
                        console.log("Erfolgreich im Archiv gespeichert");
                        setShowSuccessDialog(true);
                    })
                    .catch(error => {
                        console.error(error);
                    });
            })
            .catch((error) => {
                console.error(error);
            });
    };
    const handleSendBack = (comment) => {
        // Hole zuerst die aktuellen Daten des Protokolls vom Server
        getCall(`/api/protocol/${protocolID}`, token, "Fehler beim Zurückschicken")
            .then((response) => {
                // Aktualisiere die Daten mit isDraft: true und reviewComment
                const updatedProtocolData = {
                    ...response,
                    isDraft: true,
                    isInReview: true,
                    reviewComment: comment,
                    reviewCommentIsRead: false,
                };
                // Sende die aktualisierten Daten an den Server
                putCall(updatedProtocolData, `/api/protocol/${protocolID}`, "Fehler beim Zurückschicken", token)
                    .then(response => {
                        console.log("Erfolgreich zurückgesendet");
                        setShowDialog(false);// Schließe das Popup-Fenster nach dem Absenden
                        navigate(-1);
                    })
                    .catch(error => {
                        console.error(error);
                    });
            })
            .catch((error) => {
                console.error(error);
            });
    };

    const handleCloseDialog = () => {
        setShowDialog(false);
    };


    useEffect(() => {
        const storedLoginData = JSON.parse(localStorage.getItem("loginData"));
        if (storedLoginData) {
            setToken(storedLoginData.token);
            setOrgaID(storedLoginData.organizationId);
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

    const [isHeaderFixed, setIsHeaderFixed] = useState(false);
    useEffect(() => {
        const handleScroll = () => {
            const navBar = document.querySelector('.navbar');
            if (navBar) {
                const navBarBottom = navBar.getBoundingClientRect().bottom;
                setIsHeaderFixed(navBarBottom < 0);
            }
        };

        window.addEventListener('scroll', handleScroll);
        return () => {
            window.removeEventListener('scroll', handleScroll);
        };
    }, []);

    const handleSuccessDialogClose = () => {
        setShowSuccessDialog(false);
        navigate(-1); // Navigate to the previous page
    };
    return (
        <>
            <h1>Protokoll {protocolID}</h1> <PdfViewer protocol={protocolContent}></PdfViewer>
            <div className={isHeaderFixed ? 'button-fixed' : 'button-notfixed'}>
            <div className="row" style={{marginBottom: "10px"}}>
            <button className="button" style={{background: "green", marginRight: `8.333%`, display: "flex",
        justifyContent: "flex-start",}} onClick={handleSaveToArchive}>Protokoll genehmigen und archivieren!</button>
            <button className="button" style={{marginRight: `8.333%`}} onClick={() => setShowDialog(true)}>Protokoll an Ersteller zur Überarbeitung zurücksenden!</button>
            </div>
            </div>

            {protocolContent !== null ? (
                <ReviewInterpreter schema={protocolContent} />
            ) : null}


            {showDialog && <CommentPopup handleSendBack={handleSendBack} handleClose={handleCloseDialog}/>}

            {showSuccessDialog && ( // Zeige den SuccessDialog, wenn showSuccessDialog true ist
                <SuccesDialog
                    open={showSuccessDialog}
                    handleClose={handleSuccessDialogClose}
                    header="Protokoll wurde erfolgreich im Archiv gespeichert!"
                    text="Protokoll wurde erfolgreich im Archiv gespeichert!"
                />
            )}
        </>
    );
}
