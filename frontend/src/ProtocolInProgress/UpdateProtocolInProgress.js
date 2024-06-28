import { useState, useEffect, useContext } from "react";
import { useLocation } from "react-router-dom";
import PiPdialog from "./PiPdialog";
import { getData } from "../NewProtocol/NewProtocolService";
import { putCall } from "../API/putCall";
import { getCall } from "../API/getCall";
import AuthContext from "../API/AuthProvider";
import Interpreter from "../Components/Interpreter/Interpreter";
import { UpdateProtocolButtons } from "./UpdateProtocolButtons";
import ConfirmationDialog from "./ConfirmationDialog";
import SuccesDialog from "../Components/SuccesDialog";
import { validateData } from "../NewProtocol/NewProtocolService";

export default function UpdateProtocolInProgress() {
  const { token, setUserID, setOrgaID, setToken } =
    useContext(AuthContext);
  const [protocolContent, setProtocolContent] = useState(null);

  const location = useLocation();
  const protocolData = location.state?.payload;
  const protocolID = protocolData.id;
  const isInReview = protocolData.isInReview;


  const [showDialog, setShowDialog] = useState(false);
  const [showConfirmation, setShowConfirmation] = useState(false);
  const [showSuccessDialog, setShowSuccessDialog] = useState(false);

  useEffect(() => {
    const storedloginData = JSON.parse(localStorage.getItem("loginData"));
    // if (storedloginData) {
    //   setToken(storedloginData.token);
    //   setUserID(storedloginData.userId);
    // }
    getCall(
      "/api/protocol/" + protocolID + "/content",
      token,
      "Error getting templates"
    )
      .then((response) => {
        setProtocolContent(JSON.parse(response.content));
        //console.log(JSON.parse(response.content));
        console.log("Get Templates successfull!");
      })
      .catch((error) => {
        console.error(error);
      });
  }, [protocolID]);

  const handleSave = () => {

      const data = {
        protocolId: protocolID,
        content: JSON.stringify(getData(protocolContent)),
      };
      console.log(data);
  
      putCall(
        data,
        "/api/protocol/" + protocolID + "/content",
        "Fehler beim Updaten",
        token
      )
        .then((response) => {
          setShowSuccessDialog(true);
        })
        .catch((error) => {
          console.error(error);
        });
  };

  const handleSend = () => {

    const update = async () => {
      const dataPut = {
        protocolId: protocolID,
        content: JSON.stringify(getData(protocolContent)),
      };
  
  
      await putCall(
        dataPut,
        "/api/protocol/" + protocolID + "/content",
        "Fehler beim Updaten",
        token
      );

    }

    update();

    if(validateData(protocolContent)){
    const data = {
        id: protocolID,
        name: "string",
        isDraft: false

      };
  
      putCall(
        data,
        "/api/protocol/" + protocolID,
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
  }

  const handleShowDialog = () => {
    setShowDialog(!showDialog)
  }
  const handleShowConfirmation = () => {
    setShowConfirmation(!showConfirmation);
  }
    const handleSuccessDialogClose = () => {
      setShowSuccessDialog(false);
  };
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
      <h1>Protokoll {protocolID}</h1>
      <div className={isHeaderFixed ? 'button-fixed' : 'button-notfixed'}>
        <div className="row">
          <UpdateProtocolButtons handleShowDialog={handleShowDialog} handleSave={handleSave} />
        </div>
      </div>

      {protocolContent !== null ? <Interpreter schema={protocolContent} /> : null}

      <PiPdialog open={showDialog} handleClose={handleShowDialog} handleSend={handleSend} />

      {showConfirmation && (
          <ConfirmationDialog
              handleShowConfirmation={handleShowConfirmation}
              protocolID={protocolID}
              isInReview={isInReview}
          />
      )}

      {showSuccessDialog && (
          <SuccesDialog
              open={showSuccessDialog}
              handleClose={handleSuccessDialogClose}
              header="Protokoll wurde erfolgreich gespeichert!"
              text="Protokoll wurde erfolgreich gespeichert!"
          />
      )}
    </>
  );
}