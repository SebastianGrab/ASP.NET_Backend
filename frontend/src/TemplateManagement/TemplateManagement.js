import Tile from "../Components/Tile";
import {
  faFileCirclePlus,
  faFile,
  faMagnifyingGlassPlus,
} from "@fortawesome/free-solid-svg-icons";

import AuthContext from "../API/AuthProvider";
import { useContext, useEffect, useState } from "react";
import { getCall } from "../API/getCall";
import { putCall } from "../API/putCall";
import { genericPath, organization } from "../API/endpoints";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { NewTemplateDialog } from "./NewTemplateDialog";
import { useNavigate } from "react-router-dom";

export const TemplateManagement = () => {
  const { token, orgaID, setToken, refreshHandler } = useContext(AuthContext);
  const [templates, setTemplates] = useState(null);
  const [editTemplate, setEditTemplate] = useState(null);
  const [openDialog, setOpenDialog] = useState(false);
  const navigate = useNavigate();

  useEffect(() => {
    const storedloginData = JSON.parse(localStorage.getItem("loginData"));
    if (storedloginData) {
      setToken(storedloginData.token);
    }
    getCall(
      "/api/organization/" + orgaID + "/templates?pageIndex=1&pageSize=999999",
      token,
      "Error getting templates"
    )
      .then((response) => {
        setTemplates(response);
        console.log("Get Templates successful!");
      })
      .catch((error) => {
        console.error(error);
      });
  }, [orgaID, token, refreshHandler]);

  const dialogHandler = () => {
    setOpenDialog(!openDialog);
  };

  const navigateSearch = () => {
    navigate("ListAllTemplates");
  };

  return (
    <>
      <div className="row">
        <h1>Template Management</h1>
        <FontAwesomeIcon
          icon={faMagnifyingGlassPlus}
          size="2x"
          onClick={navigateSearch}
          className="mgmt-icon"
        />
        <FontAwesomeIcon
          icon={faFileCirclePlus}
          size="2x"
          onClick={dialogHandler}
          className="mgmt-icon"
        />
      </div>
      <NewTemplateDialog
        open={openDialog}
        handleDialog={dialogHandler}
      ></NewTemplateDialog>

      <div className="row">
        <h3>Alle Templates Ihrer Organisation</h3>
      </div>

      {templates !== null
        ? templates.items.map((template) => (
            <div key={template.id}>
              <Tile
                pagePath={template.name}
                icon={faFile}
                description={template.name}
                payload={template}
              />
            </div>
          ))
        : null}
    </>
  );
};
