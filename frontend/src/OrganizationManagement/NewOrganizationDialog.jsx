import React, { useContext, useState, useEffect } from "react";
import Button from "@mui/material/Button";
import Dialog from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import DialogContentText from "@mui/material/DialogContentText";
import DialogTitle from "@mui/material/DialogTitle";
import AuthContext from "../API/AuthProvider";
import { DropDownWithData } from "../Components/DropDownWithData";
import { postCall } from "../API/postCall";
import { postOrganization, buildOrgaNameArray, getOrgaIdByName } from "./OrganizationService";

export const NewOrganizationDialog = ({ open, handleDialog, allOrgas }) => {
  const { token, setRefreshHandler } = useContext(AuthContext);
  const [orgaNames, setOrgaNames] = useState(null);
  const [errorMessage, setErrorMessage] = useState("");

  useEffect(() => {
    setOrgaNames(buildOrgaNameArray(allOrgas));
  }, [allOrgas]);

  const handleSubmit = async (data) => {
    console.log(data);
    data.parentId = getOrgaIdByName(allOrgas, data.parentId);

    const stringData = JSON.stringify(data);
    console.log(stringData);

    try {
      const response = await postCall(stringData, "/api/organizations", "Error posting a new Organisation", token);
      console.log(response);
      if (response) {
        handleDialog();
        setRefreshHandler(response);
      }
    } catch (error) {
      setErrorMessage("Fehler beim Anlegen der Organisation");
    }
  };

  return (
    <Dialog
      open={open}
      onClose={handleDialog}
      PaperProps={{
        component: 'form',
        onSubmit: (event) => {
          event.preventDefault();
          const formData = new FormData(event.currentTarget);
          const formJson = Object.fromEntries(formData.entries());
          console.log(formJson);
          handleSubmit(formJson);
        },
      }}
    >
      <DialogTitle>Neue Organisation Anlegen</DialogTitle>
      <DialogContent sx={{ minWidth: "350px" }}>
        <DialogContentText>Bitte füllen Sie die folgenden Felder aus, um eine neue Organisation anzulegen.</DialogContentText>
        <div className="row">
          <label htmlFor="name">Name:</label>
          <div>
            <input type="text" name="name" required />
          </div>
        </div>
        <div className="row">
          <label htmlFor="address">Adresse:</label>
          <div>
            <input type="text" name="address" required />
          </div>
        </div>
        <div className="row">
          <label htmlFor="postalCode">PLZ:</label>
          <div>
            <input type="number" name="postalCode" required />
          </div>
        </div>
        <div className="row">
          <label htmlFor="city">Stadt:</label>
          <div>
            <input type="text" name="city" required />
          </div>
        </div>
        <div className="row">
        {orgaNames !== null ? (
          <DropDownWithData
            label="Mutterorganisation"
            name="parentId"
            id="parentId"
            optionsArray={orgaNames}
          ></DropDownWithData>
        ) : null}

        </div>

        <div className="row">

        {orgaNames !== null ? (
          <DropDownWithData
            label="Organisationstyp"
            name="organizationType"
            id="organizationType"
            optionsArray={["Land", "Kreisverbad", "Ortsverein", "Einsatzformation"]}
          ></DropDownWithData>
        ) : null}
    </div>
        {errorMessage && <p style={{ color: 'red' }}>{errorMessage}</p>}
      </DialogContent>
      <DialogActions>
        <Button onClick={handleDialog}>Abbruch</Button>
        <Button type="submit">
          Organisation anlegen
        </Button>
      </DialogActions>
    </Dialog>
  );
};
