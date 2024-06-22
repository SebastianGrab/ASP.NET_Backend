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

export const TemplateChoice = () => {
  const { token, userID, orgaID, setUserID, setOrgaID, setToken, refreshHandler} = useContext(AuthContext);
  const [templates, setTemplates] = useState(null);

  useEffect(() => {
    const storedloginData = JSON.parse(localStorage.getItem('loginData'));
    if (storedloginData) {
      setToken(storedloginData.token);
      setOrgaID(storedloginData.organizationId);
      setUserID(storedloginData.userId);
    }

    // const getOrga = async () => {
    //   const response = await getCall("/api/organization/1", token, "Errog orga")
    //   console.log(response);
    // }

    // getOrga();

    const getTemplates = async () => {
      const response = await getCall("/api/organization/ "+ orgaID + "/templates?pageIndex=1&pageSize=9999999", token, "Errog orga")
      console.log(response);
      setTemplates(response);
    }

    getTemplates();

  }, [orgaID, refreshHandler]);



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
