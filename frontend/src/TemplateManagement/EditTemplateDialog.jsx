import React, { useContext, useState, useEffect } from "react";
import Button from "@mui/material/Button";
import Dialog from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import DialogContentText from "@mui/material/DialogContentText";
import DialogTitle from "@mui/material/DialogTitle";
import AuthContext from "../API/AuthProvider";
import { handleFileUpload } from "./TemplateService";
import { putCall } from "../API/putCall";

export const EditTemplateDialog = ({ open, handleDialog, templateData }) => {
  const { token, setToken, userID, orgaID, setRefreshHandler } =
    useContext(AuthContext);
  const [template, setTemplate] = useState(null);

  const handleSubmit = () => {
    const name = document.getElementById("templateName").value;
    const description = document.getElementById("description").value;

    const data = {
      id: templateData.id,
      name: name,
      description: description,
      templateContent: template,
    };

    const updateTemplate = async () => {
      const response = await putCall(
        data,
        "/api/template/" + templateData.id,
        "Error updating template",
        token
      );
      console.log(response);
      if (response) {
        handleDialog();
        setRefreshHandler(response);
      }
    };

    updateTemplate();
  };

  return (
    <Dialog
      open={open}
      onClose={handleDialog}
      PaperProps={{
        component: "form",
        onSubmit: (event) => {
          event.preventDefault();
          const formData = new FormData(event.currentTarget);
          const formJson = Object.fromEntries(formData.entries());
          console.log(formJson);
          handleSubmit(formJson);
        },
      }}
    >
      <DialogTitle>Template updaten</DialogTitle>
      <DialogContent sx={{ minWidth: "350px" }}>
        <DialogContentText></DialogContentText>
        <div className="row">
          <label htmlFor="templateName">Template Name:</label>
          <div>
            <input type="text" name="templateName" id="templateName"></input>
          </div>
        </div>
        <div className="row">
          <label htmlFor="description">Beschreibung:</label>
          <div>
            <input type="text" name="description" id="description"></input>
          </div>
        </div>
        <div className="row">
          <label htmlFor="description">Datei auswählen:</label>
          <input
            type="file"
            accept=".json"
            onChange={(e) => handleFileUpload(e, setTemplate)}
          />
        </div>
      </DialogContent>
      <DialogActions>
        <Button onClick={handleDialog}>Abbruch</Button>
        <Button onClick={handleSubmit}>Template updaten!</Button>
      </DialogActions>
    </Dialog>
  );
};
