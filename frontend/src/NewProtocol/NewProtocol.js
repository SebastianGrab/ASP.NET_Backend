import { useState, useEffect, useContext } from "react";
import schema from "../Resources/Data/protocol2.json";
import AuthContext from "../API/AuthProvider";
import { postCall } from "../API/postCall";
import { useParams, useLocation } from "react-router-dom";
import Interpreter from "../Components/Interpreter/Interpreter";
import SuccesDialog from "../Components/SuccesDialog";
import { getData, validateData, checkPattern, buildProtocolContentData, buildProtocolInitData } from "./NewProtocolService";
import { protocol, genericQuery } from "../API/endpoints";
import { FormControl } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { green } from "@mui/material/colors";
import ConfirmationDialog from "../ProtocolInProgress/ConfirmationDialog";
import { putCall } from "../API/putCall";
import PiPdialog from "../ProtocolInProgress/PiPdialog";

export default function NewProtocol() {

  const navigate = useNavigate();
    const { token, userID, orgaID, setRefreshHandler} = useContext(AuthContext);
  const { template } = useParams();
  const location = useLocation();
  const templateData = location.state?.payload;
  const templateContent = JSON.parse(templateData.templateContent);
  const templateID = templateData.id;
  console.log(templateContent);

  const [showSuccessDialog, setShowSuccessDialog] = useState(false);
  const [protocol, setProtocol] = useState(templateContent);
  const [showConfirmation, setShowConfirmation] = useState(false);

  const [showDialog, setShowDialog] = useState(false);
  const handleDialog = () => {
    setShowSuccessDialog(!showSuccessDialog);
  }


  const handleShowDialog = () => {
    setShowDialog(!showDialog)
  }

  const handleSave = () => {
   
  
    const initializeProtocol = async () => {
      const response = await postCall(buildProtocolInitData(), "/api/protocols?templateId=" + templateID + "&organizationId=" + orgaID + "&userId=" + userID, "Error posting a new protocol", token);
      saveProtocolContent(response, true);
    }
    //postCall(protocolData, endpoint2, errMsg2, token);

      initializeProtocol();
  

  };

  
const handleSend = () => {

  const initialPostBody = {
    name: "string",
    isDraft: false,
    reviewComment: "string",
    isClosed: false,
    isInReview: false,
  };

  const initProtocol = async () => {
    if(validateData(getData(protocol))){
    const response = await postCall(initialPostBody, "/api/protocols?templateId=" + templateID + "&organizationId=" + orgaID + "&userId=" + userID, "Error posting a new protocol", token);
    const responseContent = await saveProtocolContent(response, false);

    setShowConfirmation(true); // Show confirmation dialog after sending
    handleShowDialog(); // Close the initial dialog

    if(responseContent){
    
    }
    console.log(response);

  }
  setShowConfirmation(true); // Show confirmation dialog after sending
  handleShowDialog(); // Close the initial dialog


  }

  initProtocol();

  const changeStatus = async (response) => {

    if(validateData(getData(protocol))){
      const data = {
          id: response.id,
          name: "string",
          isDraft: false
    
        };
    
        putCall(
          data,
          "/api/protocol/" + response.id +
          "Fehler beim Updaten",
          token
        )
          .then((response) => {
            console.log(response);
            setShowConfirmation(true); // Show confirmation dialog after sending
            setShowDialog(false); // Close the initial dialog
          })
          .catch((error) => {
            console.error(error);
          });
        }


    // const resPut = await putCall(
    //   dataPut,
    //   "/api/protocol/" + response.id + "/content",
    //   "Fehler beim Updaten",
    //   token
    // );


  }

 
}

const handleShowConfirmation = () => {
  setShowConfirmation(!showConfirmation);
}


  const saveProtocolContent = async (protocolMetadata, showDialog) => {
    console.log(showDialog);
    console.log(protocolMetadata);
    const protocolData = getData(protocol);

      const data = buildProtocolContentData(protocolMetadata.id, protocolData);
      const response = await postCall(data, "/api/protocol/" + protocolMetadata.id + "/content", protocol.errMsgPostProtocol, token);
      setRefreshHandler(response);
      if(showDialog){
      if(response){
        handleDialog();
      }


        setProtocol(templateContent);
        setShowSuccessDialog(showDialog);

      }



  }
  const handleSuccessDialogClose = () => {
    setShowSuccessDialog(false);
  }


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

  return (
    <>
      <h1>Neues Protokoll!</h1>
      <div className={isHeaderFixed ? 'button-fixed' : 'button-notfixed'}>
      <div className="row" style={{marginBottom: "10px"}}>

      <input
            className="button"
            value="Protokoll senden"
            type="button"
            onClick={handleShowDialog}
            style={{display: "flex",
              justifyContent: "flex-start",
              marginRight: `8.333%`,
            }}
        />

        <input
            className="button-scnd"
            value="Protokoll speichern"
            type="button"
            onClick={handleSave}
            style={{display: "flex",
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


    </>
  );
}
