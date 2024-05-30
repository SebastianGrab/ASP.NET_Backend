import Tile from "../Components/Tile";
import {
  faFileCirclePlus,
  faFilePen,
  faEnvelope,
  faBoxArchive,
  faChartSimple,
} from "@fortawesome/free-solid-svg-icons";
import AuthContext from "../API/AuthProvider";
import { useContext, useEffect, useState } from "react";
import { getCall } from "../API/getCall";
import { genericPath, organization } from "../API/endpoints";

export const TemplateChoice = () => {
  const { token, userID, orgaID, setUserID, setOrgaID, setToken} = useContext(AuthContext);
  const [templates, setTemplates] = useState(null);

  useEffect(() => {
    const storedloginData = JSON.parse(localStorage.getItem('loginData'));
    if (storedloginData) {
      setToken(storedloginData.token);
      setOrgaID(storedloginData.organizationId);
      setUserID(storedloginData.userId);
    }
    getCall(organization.ep + orgaID + genericPath.template, token, "Error getting templates")
      .then((response) => {
        setTemplates(response);
        console.log("Get Templates successfull!");
      })
      .catch((error) => {
        console.error(error);
      });
  }, [orgaID]);



  return (
    <>
      {templates !== null
        ? templates.items.map((template) => (
            <Tile
              pagePath={template.name}
              icon={faFileCirclePlus}
              description={template.name}
              payload={template}
              key={template.id}
            />
          ))
        : null}

    </>
  );
};
