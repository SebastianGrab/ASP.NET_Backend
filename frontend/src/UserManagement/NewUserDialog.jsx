import React, { useContext, useState, useEffect } from "react";
import Button from "@mui/material/Button";
import Dialog from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import DialogContentText from "@mui/material/DialogContentText";
import DialogTitle from "@mui/material/DialogTitle";
import AuthContext from "../API/AuthProvider";
import { getRoles, postUser, findRoleIdByName, roleInformationAsArrays } from "./UserManagementService";
import { buildOrgaNameArray, getOrgaIdByName } from "../OrganizationManagement/OrganizationService";
import { DropDownWithData } from "../Components/DropDownWithData";
import { postCall } from "../API/postCall";
import { getCall } from "../API/getCall";

export const NewUserDialog = ({ open, handleDialog }) => {
  const { token, setToken, userID, orgaID, setRefreshHandler } = useContext(AuthContext);
  const [roles, setRoles] = useState(null);
  const [roleNames, setRoleNames] = useState(null);
  const [orgaNames, setOrgaNames] = useState(null);
  const [orgaData, setOrgaData] = useState([]);
  const [authData, setAuthData] = useState(null);
  const [errorMessage, setErrorMessage] = useState("");

  useEffect(() => {
    const storedloginData = JSON.parse(localStorage.getItem("loginData"));
    // if (storedloginData) {
    //   setToken(storedloginData.token);
    // }
    setErrorMessage("");

    const fetchRoles = async () => {
      try {
        const result = await getRoles(token);
        setRoles(result);
        if(roles !== null){
          const roleInformationAsArray = roleInformationAsArrays(roles);
          setRoleNames(roleInformationAsArray.roleNames);
        }
      } catch (error) {
        console.error("Error fetching roles:", error);
      }

      try {
        const result = await getCall("/api/organizations?pageIndex=1&pageSize=9999999", token, "Error getting Organizations");
        setOrgaData(result.items);
        setOrgaNames(buildOrgaNameArray(result.items));
      } catch (error) {
        console.error("Error fetching roles:", error);
      }
    };

    if (token) {
      fetchRoles();
    }
  }, [token, open, setToken]);

  const handleSubmit = async (data) => {
    const roleName = data.role;
    const roleId = findRoleIdByName(roleName, roles);
    const orgaName = data.orgaName;
    const orgaId = getOrgaIdByName(orgaData, orgaName);


    try{
      const response = await postUser(token, data);
      if (response) {
        setAuthData(response);
    }

    setRefreshHandler(response);

    if(response){
      const response2 = await postCall("", "/api/user/" + response.id + "/organization/" + orgaId + "/role/" + roleId, "Fehler beim anlegen des Users!", token);
      if(response2){
        handleDialog();
      };
    };
} catch (error) {
    setErrorMessage("Fehler beim anlegen des Nutzers");
}

  };


  const checkForEmptyValues = () => {
    if(document.getElementById("FIRSTNAME").value === null || document.getElementById("EMAIL").value  === null ){
        return "WRONG";
    } else return "RIGHT";


  }
  return (
    <Dialog open={open} onClose={handleDialog}
    PaperProps={{
      component: 'form',
      onSubmit: (event) => {
        event.preventDefault();
        const formData = new FormData(event.currentTarget);
        const formJson = Object.fromEntries(formData.entries());
        handleSubmit(formJson);

      },
    }}>
      <DialogTitle>Neuen Nutzer Anlegen</DialogTitle>
      <DialogContent sx={{ minWidth: "350px" }}>
        <DialogContentText></DialogContentText>
        <div className="row">
          <label htmlFor="firstName">Vorname:</label>
          <div>
            <input type="text" id="FIRSTNAME" name="firstName" required></input>
          </div>
        </div>
        <div className="row">
          <label htmlFor="firstName">Nachname:</label>
          <div>
            <input type="text" id="LASTNAME" name="lastName" required></input>
          </div>
        </div>
        <div className="row">
          <label htmlFor="firstName" >Email:</label>
          <div>
            <input type="text" id="EMAIL" name="email" pattern="^[^\s@]+@[^\s@]+\.[^\s@]+$" required></input>
          </div>
        </div>
        <div className="row">
          <label htmlFor="firstName">Passwort:</label>
          <div>
            <input type="password" id="PW" name="password" required></input>
          </div>
        </div>
        <div className="row">
        {orgaNames !== null ? (
            <DropDownWithData
            label="Organisation"
              name="orgaName"
              optionsArray={orgaNames}
            ></DropDownWithData>
          ) : null}
        </div>
        <div className="row">
          {roleNames !== null ? (
            <DropDownWithData
            label="Rolle zuweisen"
              name="role"
              optionsArray={roleNames}
            ></DropDownWithData>
          ) : null}
        </div>
      </DialogContent>
      {errorMessage === "Fehler beim anlegen des Nutzers" && <p style={{ color: 'red' }}>{errorMessage}</p>}
      <DialogActions>
        <Button onClick={handleDialog}>Abbruch</Button>
        <Button type="submit" >
          Nutzer anlegen
        </Button>
      </DialogActions>
    </Dialog>
  );
};
