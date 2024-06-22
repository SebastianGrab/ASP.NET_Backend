import React, { useContext, useState, useEffect } from "react";
import Button from "@mui/material/Button";
import TextField from "@mui/material/TextField";
import Dialog from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import DialogContentText from "@mui/material/DialogContentText";
import DialogTitle from "@mui/material/DialogTitle";
import AuthContext from "../API/AuthProvider";
import EditIcon from "@mui/icons-material/Edit";
import { buildOrgaNameArray } from "../OrganizationManagement/OrganizationService";
import { DropDownWithData } from "../Components/DropDownWithData";
import { Box, Divider } from "@mui/material";
import { findIdByName } from "./UserManagementService";
import { postCall } from "../API/postCall";
import { AdditionalOrgasForUserList } from "./AdditionalOrgasForUserList";

import {
  getRoles,
  postUser,
  buildUserData,
  findRoleIdByName,
  roleInformationAsArrays,
  updateUser,
} from "./UserManagementService";
import { deleteCall } from "../API/deleteCall";
import { getCall } from "../API/getCall";

export const EditUserDialog = ({ open, handleDialog, user }) => {
  const { token, setToken, userID, orgaID, setRefreshHandler, refreshHander } =
    useContext(AuthContext);
  const [roles, setRoles] = useState(null);
  const [roleNames, setRoleNames] = useState(null);
  const [editFirstName, setEditFirstName] = useState(true);
  const [editLastName, setEditLastName] = useState(true);
  const [editEmail, setEditEmail] = useState(true);
  const [orgas, setOrgas] = useState([]);
  const [orgaNames, setOrgaNames] = useState([]);
  const [orgaError, setOrgaError] = useState(false);
  const [userOrgas, setUserOrgas] = useState(null);
  const [showAddOrga, setShowAddOrga] = useState(false);
  const [showAdditionalOrgas, setShowAdditionalOrgas] = useState(false);
  const handleSubmit = async (data, userId) => {
    console.log(data);
    try {
      const response = await updateUser(token, data, userId);
      setRefreshHandler(response);
      console.log(response);

      if (response) {
        handleDialog();
      }
    } catch (error) {
      console.log("Fehler beim posten eines neuen users")
    }
  };

  const deleteUser = async (userId) => {
    try {
      const response = await deleteCall(
        "/api/user/" + userId,
        "Fehler beim Löschen des Users",
        token
      );
      setRefreshHandler(response);
      console.log(response);

      if (response) {
        handleDialog();
      }
    } catch (error) {
      console.error(error);
    }
  };

  useEffect(() => {
    setAllEditHandler(true);
    getAllOrgasForUser();

    const getOrgas = async () => {
      try {
        const result = await getCall(
          "/api/organizations?pageIndex=1&pageSize=9999999",
          token,
          "Error getting Organizations"
        );

        console.log(result);
        setOrgas(result.items);
        setOrgaNames(buildOrgaNameArray(result.items));
        console.log(result);
      } catch (error) {
        console.error("Error fetching roles:", error);
      }

      try {
        const result = await getRoles(token);
        setRoles(result);

        const roleInformationAsArray = roleInformationAsArrays(result);
        setRoleNames(roleInformationAsArray.roleNames);
        setRoleNames(roleNames.filter((role) => role !== "Admin"));
        console.log(roleNames);

        console.log(result);
      } catch (error) {
        console.error("Error fetching roles:", error);
      }
    };

    getOrgas();
  }, [open, refreshHander]);

  const getAllOrgasForUser = async () => {
    try {
      const result = await getCall("/api/user/" + user.id +  "/organizations", token, "Error getting Organisation for a User");
      setUserOrgas(result);
      console.log(result);
    } catch (error) {
      console.error("Error getting Organisation for a User:", error);
    }

  }

  const setAllEditHandler = (bool) => {
    setEditEmail(bool);
    setEditFirstName(bool);
    setEditLastName(bool);
  };

  const addOrgaToUser = async () => {
    let orgaName = document.getElementById("orgaName").value;
    let roleName = document.getElementById("roleName").value;

    const orgaIdToAdd = findIdByName(orgaName, orgas);
    const roleIdToAdd = findIdByName(roleName, roles);

    try {
      const response = await postCall(
        "",
        "/api/user/" +
          user.id +
          "/organization/" +
          orgaIdToAdd +
          "/role/" +
          roleIdToAdd,
        "Error adding new Orgas to the user",
        token
      );
      console.log(response);
      if (response) {
        handleLeaveDialog();
        setRefreshHandler(response);
      }
    } catch (error) {
      console.error(error);
      setOrgaError(true);
    }
  };

  const handleShowAddOrga = () => {
    if(showAdditionalOrgas){
      setShowAdditionalOrgas(!showAdditionalOrgas);
      setShowAddOrga(!showAddOrga);
    } else{
      setShowAddOrga(!showAddOrga);
    }
  }

  const handleShowAdditionalOrgas= () => {
    console.log(userOrgas);
    if(showAddOrga){
      setShowAdditionalOrgas(!showAdditionalOrgas);
      setShowAddOrga(!showAddOrga);
    } else{
      setShowAdditionalOrgas(!showAdditionalOrgas);
    }
    getAllOrgasForUser();  }

  const handleLeaveDialog = () => {
    handleDialog();
    setShowAdditionalOrgas(false);
    setShowAddOrga(false);
    setOrgaError(false);
  }
  return (
    <Dialog
      open={open}
      onClose={handleLeaveDialog}
      PaperProps={{
        component: "form",
        onSubmit: (event) => {
          event.preventDefault();
          const formData = new FormData(event.currentTarget);
          const formJson = Object.fromEntries(formData.entries());
          console.log(formJson);
          handleSubmit(formJson, user.id);
        },
      }}
    >
      <DialogTitle>Nutzer Daten aktualisieren</DialogTitle>
      <DialogContent sx={{ minWidth: "350px" }}>
        <DialogContentText></DialogContentText>
        <div className="row">
          <label htmlFor="firstName">Vorname:</label>
          <div>
            <EditIcon
              className="edit-icon"
              color={editFirstName ? "action" : "secondary"}
              onClick={() => setEditFirstName(!editFirstName)}
            />
            <input
              type="text"
              id="FIRSTNAME"
              name="firstName"
              defaultValue={user.firstName}
              disabled={editFirstName}
            ></input>
          </div>
        </div>
        <div className="row">
          <label htmlFor="firstName">Nachname:</label>
          <div>
            <EditIcon
              className="edit-icon"
              color={editLastName ? "action" : "secondary"}
              onClick={() => setEditLastName(!editLastName)}
            />
            <input
              type="text"
              id="LASTNAME"
              name="lastName"
              defaultValue={user.lastName}
              disabled={editLastName}
            ></input>
          </div>
        </div>
        <div className="row">
          <label htmlFor="firstName">Email:</label>
          <div>
            <EditIcon
              className="edit-icon"
              color={editEmail ? "action" : "secondary"}
              onClick={() => setEditEmail(!editEmail)}
            />
            <input
              type="text"
              id="EMAIL"
              name="email"
              defaultValue={user.email}
              disabled={editEmail}
            ></input>
          </div>
        </div>

        <Box mt={2}></Box>
        <Divider sx={{ width: "98%", borderWidth: "2px" }} />
        <Box mt={2}></Box>

        <div className="row">
          <Button
            color="secondary"
            variant={showAddOrga ? "contained" : "outlined"} // Variante basierend auf showAddOrga
            onClick={handleShowAddOrga}
          >
            Nutzer einer weiteren Organisation zuweisen.
          </Button>

          <Button
            color="secondary"
            variant={showAdditionalOrgas ? "contained" : "outlined"} // Variante basierend auf showAdditionalOrgas
            onClick={handleShowAdditionalOrgas}
          >
            Alle Organisationen dieses Nutzers anzeigen.
          </Button>
        </div>

        {showAddOrga ? (

          <div>
            {orgaError ? <div className="row">
              Fehler beim Updaten des Nutzers. Überprüfen Sie ob der Nutzer bereits Mitglied der Organisation ist.
            
            </div> : null }
                      
            {orgaNames !== null ? (
              <>
                <div className="row">
                  <DropDownWithData
                    label="Organisation"
                    name="orgaName"
                    id="orgaName"
                    optionsArray={orgaNames}
                  ></DropDownWithData>
                </div>

                {roleNames !== null ? (
                  <div className="row">
                    <DropDownWithData
                      label="Rolle"
                      name="roleName"
                      id="roleName"
                      optionsArray={roleNames}
                    ></DropDownWithData>
                  </div>
                ) : null}


                <div className="row">
                  <Button variant="outlined" onClick={addOrgaToUser}>
                    Nutzer dieser Organisation zuweisen
                  </Button>
                </div>
              </>
            ) : null}
          </div>
        ) : null}

        {showAdditionalOrgas ?
        <div>
          {userOrgas !== null ?
                   <AdditionalOrgasForUserList orgas={userOrgas} user={user}/>
          : null}
   

        </div>
        : null}
      </DialogContent>

      <DialogActions>
        <Button onClick={() => deleteUser(user.id)}>Nutzer Löschen</Button>
      </DialogActions>
      <DialogActions>
        <Button onClick={handleLeaveDialog}>Abbruch</Button>
        <Button type="submit" onClick={() => setAllEditHandler(false)}>
          Daten aktualisieren
        </Button>
      </DialogActions>
    </Dialog>
  );
};
