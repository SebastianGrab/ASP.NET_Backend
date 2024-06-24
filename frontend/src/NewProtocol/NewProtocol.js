import React, { useState, useEffect, useContext } from "react";
import { useParams, useLocation, useNavigate } from "react-router-dom";
import AuthContext from "../API/AuthProvider";
import { postCall } from "../API/postCall";
import Interpreter from "../Components/Interpreter/Interpreter";
import ConfirmationDialog from "../ProtocolInProgress/ConfirmationDialog";
import PiPdialog from "../ProtocolInProgress/PiPdialog";
import { getData, validateData, buildProtocolContentData, buildProtocolInitData } from "./NewProtocolService";
import SuccessDialog from "../Components/SuccesDialog";

export default function NewProtocol() {
  const navigate = useNavigate();
  const { token, userID, orgaID, setRefreshHandler } = useContext(AuthContext);
  const { template } = useParams();
  const location = useLocation();
  const templateData = location.state?.payload;
  const templateContent = JSON.parse(templateData.templateContent);
  const templateID = templateData.id;

  const [showSuccessDialog, setShowSuccessDialog] = useState(false);
  const [protocol, setProtocol] = useState(templateContent);
  const [showConfirmation, setShowConfirmation] = useState(false);
  const [showDialog, setShowDialog] = useState(false);

  const handleDialog = () => {
    setShowSuccessDialog(!showSuccessDialog);
  };

  const handleShowDialog = () => {
    setShowDialog(!showDialog);
  };

  const handleShowConfirmation = (show) => {
    console.log("Setting showConfirmation to:", show);
    setShowConfirmation(show);
  };

  const handleSave = async () => {
    const initializeProtocol = async () => {
      const response = await postCall(
          buildProtocolInitData(),
          `/api/protocols?templateId=${templateID}&organizationId=${orgaID}&userId=${userID}`,
          "Error posting a new protocol",
          token
      );
      saveProtocolContent(response, true);
    };

    initializeProtocol();
  };

  const handleSend = async () => {
    const initialPostBody = {
      name: "string",
      isDraft: false,
      reviewComment: "string",
      isClosed: false,
      isInReview: false,
    };

    if (validateData(getData(protocol))) {
      try {
        const response = await postCall(
            initialPostBody,
            `/api/protocols?templateId=${templateID}&organizationId=${orgaID}&userId=${userID}`,
            "Error posting a new protocol",
            token
        );

        if (response) {
          console.log("Post call successful, response:", response);
          const responseContent = await saveProtocolContent(response, false);

          if (responseContent) {
            console.log("Protocol content saved successfully, showing confirmation dialog.");
            handleShowConfirmation(true); // Show confirmation dialog after sending
            setShowDialog(false); // Close the initial dialog
          } else {
            console.log("Protocol content not saved, not showing confirmation dialog.");
          }
        } else {
          console.log("Post call response is null or undefined.");
        }
      } catch (error) {
        console.error("Error while sending protocol:", error);
      }
    } else {
      console.log("Protocol validation failed.");
    }
  };


  const saveProtocolContent = async (protocolMetadata, showDialog) => {
    const protocolData = getData(protocol);
    const data = buildProtocolContentData(protocolMetadata.id, protocolData);

    try {
      const response = await postCall(
          data,
          `/api/protocol/${protocolMetadata.id}/content`,
          "Error posting protocol content",
          token
      );

      setRefreshHandler(response);

      if (response) {
        console.log("saveProtocolContent response:", response);

        if (showDialog) {
          handleDialog();
          setProtocol(templateContent);
          setShowSuccessDialog(true);
        }
      } else {
        console.log("saveProtocolContent: No response received");
      }

      return response;
    } catch (error) {
      console.error("Error while saving protocol content:", error);
      return null;
    }
  };

  const handleSuccessDialogClose = () => {
    setShowSuccessDialog(false);
  };

  const [isHeaderFixed, setIsHeaderFixed] = useState(false);

  useEffect(() => {
    const handleScroll = () => {
      const navBar = document.querySelector(".navbar");
      if (navBar) {
        const navBarBottom = navBar.getBoundingClientRect().bottom;
        setIsHeaderFixed(navBarBottom < 0);
      }
    };

    window.addEventListener("scroll", handleScroll);
    return () => {
      window.removeEventListener("scroll", handleScroll);
    };
  }, []);

  return (
      <>
        <h1>Neues Protokoll!</h1>
        <div className={isHeaderFixed ? "button-fixed" : "button-notfixed"}>
          <div className="row" style={{ marginBottom: "10px" }}>
            <input
                className="button"
                value="Protokoll senden"
                type="button"
                onClick={handleShowDialog}
                style={{
                  display: "flex",
                  justifyContent: "flex-start",
                  marginRight: `8.333%`,
                }}
            />
            <input
                className="button-scnd"
                value="Protokoll speichern"
                type="button"
                onClick={handleSave}
                style={{
                  display: "flex",
                  justifyContent: "flex-start",
                  marginRight: `8.333%`,
                }}
            />
          </div>
        </div>
        <Interpreter schema={protocol} />
        {showConfirmation && (
            <ConfirmationDialog
                handleShowConfirmation={handleShowConfirmation}
                protocolID=""
                isInReview=""
            />
        )}
        <PiPdialog open={showDialog} handleClose={handleShowDialog} handleSend={handleSend} />
        {showSuccessDialog && (
            <SuccessDialog
                open={showSuccessDialog}
                handleClose={handleSuccessDialogClose}
                header="Protokoll wurde erfolgreich gespeichert!"
                text="Protokoll wurde erfolgreich gespeichert!"
            />
        )}
      </>
  );
}
