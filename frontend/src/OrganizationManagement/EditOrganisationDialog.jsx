import React, { useContext, useState, useEffect } from "react";
import Button from "@mui/material/Button";
import TextField from "@mui/material/TextField";
import Dialog from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import DialogContentText from "@mui/material/DialogContentText";
import DialogTitle from "@mui/material/DialogTitle";
import AuthContext from "../API/AuthProvider";
import EditIcon from '@mui/icons-material/Edit';
import { DropDownWithData } from "../Components/DropDownWithData";
import { buildOrgaNameArray, getOrgaIdByName } from "./OrganizationService";


import { deleteCall } from "../API/deleteCall";
import { putCall } from "../API/putCall";
import { postCall } from "../API/postCall";


export const EditOrganisationDialog = ({ open, handleDialog, orga, allOrgas}) => {
  const { token, setToken, userID, orgaID, setRefreshHandler } = useContext(AuthContext);
  const [roles, setRoles] = useState(null);
  const [roleNames, setRoleNames] = useState(null);
  const [editName, setEditName] = useState(true);
  const [editCity, setEditCity] = useState(true);
  const [editPostalCode, setEditPostalCode] = useState(true);
  const [editAddress, setEditAddress] = useState(true);
  const [editType, setEditType] = useState(true);
  const [editParent, setEditParent] = useState(true);

  const [orgaNames, setOrgaNames] = useState(null);
  const [orgaData, setOrgaData] = useState([]);

  useEffect(() => {
    setOrgaData(allOrgas);
    setOrgaNames(buildOrgaNameArray(allOrgas));
    console.log(allOrgas);

  }, [allOrgas]);

  const handleSubmit = async (data, orgaId) => {

    data.id = orgaId;
    data.parentId = getOrgaIdByName(allOrgas, data.parentId);
    console.log(data);
    const postData = JSON.stringify(data);
    try {
      const response = await putCall(postData, "/api/organization/" + orgaId ,"Fehler beim Updaten der Organisation" , token,);
      setRefreshHandler(setRefreshHandler + 1);
      console.log(response);
      handleDialog();
    
    } catch (error) {
        <div>Fehler!</div>
    }
  };

  const deleteOrga = async (orgaId) => {
    try {
      const response = await deleteCall("/api/organization/" + orgaId, "Fehler beim Löschen des Users", token);
      setRefreshHandler(response);
      console.log(response);
      
    if(response){
      handleDialog();
    };


    } catch (error) {
        console.error(error);
    }
  };

  useEffect(() => {
    setAllEditHandler(true);
    }, [open]);


    const setAllEditHandler = (bool) => {
        setEditPostalCode(bool);
        setEditName(bool);
        setEditCity(bool);
        setEditType(bool);
        setEditAddress(bool);
        setEditParent(bool);
    }


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
        console.log(formJson);
        handleSubmit(formJson, orga.id);

      },
    }}>
      <DialogTitle>Nutzer Daten aktualisieren</DialogTitle>
      <DialogContent sx={{ minWidth: "350px" }}>
        <DialogContentText></DialogContentText>
        <div className="row">
          <label htmlFor="firstName">Name:</label>
          <div >
          <EditIcon className="edit-icon" color={editName ? 'action' : 'secondary'} onClick={() => setEditName(!editName)} />
            <input type="text"  name="name"  defaultValue={orga.name} disabled={editName} ></input>
          </div>
        </div>
        <div className="row">
          <label htmlFor="firstName">Stadt:</label>
          <div>
          <EditIcon className="edit-icon" color={editCity ? 'action' : 'secondary'} onClick={() => setEditCity(!editCity)}/>
            <input type="text"  name="city" defaultValue={orga.city} disabled={editCity}></input>
          </div>
        </div>
        <div className="row">
          <label htmlFor="firstName" >PLZ:</label>
          <div>
          <EditIcon className="edit-icon"  color={editPostalCode ? 'action' : 'secondary'} onClick={() => setEditPostalCode(!editPostalCode)}/>
            <input type="text" name="postalCode" defaultValue={orga.postalCode} disabled={editPostalCode}></input>
          </div>
        </div>
        <div className="row">
          <label htmlFor="firstName" >Adresse:</label>
          <div>
          <EditIcon className="edit-icon"  color={editAddress ? 'action' : 'secondary'} onClick={() => setEditAddress(!editAddress)}/>
            <input type="text" name="address" defaultValue={orga.address} disabled={editAddress}></input>
          </div>
        </div>

        {orgaNames !== null ? (
            <DropDownWithData
            label="Mutterorganisation"
              name="parentId"
              id="parentId"
              optionsArray={orgaNames}
            ></DropDownWithData>
          ) : null}
        <div className="row">
          <label htmlFor="firstName" >Typ:</label>
          <div>
          <EditIcon className="edit-icon"  color={editType ? 'action' : 'secondary'} onClick={() => setEditType(!editType)}/>
            <input type="text"  name="organizationType" defaultValue={orga.organizationType} disabled={editType}></input>
          </div>
        </div>
      </DialogContent>

      <DialogActions>
      <Button onClick={() => deleteOrga(orga.id)}>Organisation Löschen</Button>

      </DialogActions>
      <DialogActions>
        <Button onClick={handleDialog}>Abbruch</Button>
        <Button type="submit" onClick={() => setAllEditHandler(false)} >
          Daten aktualisieren
        </Button>
  


        

      </DialogActions>

    </Dialog>
  );
};
