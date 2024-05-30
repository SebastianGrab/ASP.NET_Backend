import { useState, useEffect, useContext } from "react";
import schema from "../Resources/Data/protocol2.json";
import AuthContext from "../API/AuthProvider";
import { postCall } from "../API/postCall";
import { useParams, useLocation } from "react-router-dom";
import Interpreter from "../Components/Interpreter/Interpreter";
import SuccesDialog from "../Components/SuccesDialog";
import { getData } from "./getProtocolData";
import { protocol, genericQuery } from "../API/endpoints";

export default function NewProtocol() {
    const { token, userID, orgaID} = useContext(AuthContext);
  const { template } = useParams();
  const location = useLocation();
  const templateData = location.state?.payload;
  const templateContent = JSON.parse(templateData.templateContent);
  const templateID = templateData.id;

    const temp = JSON.parse(templateContent);

  const initialPostBody = {
    "name": "string",
    "isDraft": true,
    "reviewComment": "string",
    "isClosed": true
  };

  
  const [showDialog, setShowDialog] = useState(false);


  const handleSave = () => {

    //postCall(initialPostBody, protocol.ep + genericQuery.templateID + templateID + genericQuery.orgaID + orgaID + genericQuery.userID + userID, protocol.errMsgPostProtocol, token)
    postCall(initialPostBody, "/api/protocols?templateId=" + templateID + "&organizationId=" + orgaID + "&userId=" + userID, "Error posting a new protocol", token)
      .then((response) => {
        saveProtocolContent(response);
      })
      .catch((error) => {
        console.error(error);
      });

    //postCall(protocolData, endpoint2, errMsg2, token);
    //setShowDialog(true);
  };

  const saveProtocolContent = (protocolMetadata) => {
    const protocolData = getData(temp);
    const parsedProtocolData = JSON.stringify(protocolData);
    const body = {
        "protocolId": protocolMetadata.id,
        "content":  parsedProtocolData,
        "createdDate": "2024-05-24T15:39:41.807Z",
        "updatedDate": "2024-05-24T15:39:41.807Z"
    };

    postCall(body, "/api/protocol/" + protocolMetadata.id + "/content", protocol.errMsgPostProtocol, token)
    .then((response) => {
      console.log(response);
    })
    .catch((error) => {
      console.error(error);
    });

  }

  return (
    <>
      <h1>Neues Protokoll!</h1>
      <Interpreter schema={temp} />



      <div className="row">
        <input
          className="button"
          value="Protokoll speichern"
          type="button"
          onClick={handleSave}
        ></input>
      </div>

      {showDialog && (
        <SuccesDialog
          header="Protokoll wurde erfoglreich gespeichert!"
          text="Protokoll wurde erfoglreich gespeichert!"
        />
      )}
    </>
  );
}
