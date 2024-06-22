import React, { useContext, useState, useEffect } from "react";
import Button from "@mui/material/Button";
import TextField from "@mui/material/TextField";
import Dialog from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import DialogContentText from "@mui/material/DialogContentText";
import DialogTitle from "@mui/material/DialogTitle";
import AuthContext from "../API/AuthProvider";
import { DropDownWithData } from "../Components/DropDownWithData";
import { postCall } from "../API/postCall";
import { getCall } from "../API/getCall";
import { handleFileUpload, buildTemplateData } from "./TemplateService";

export const NewTemplateDialog = ({ open, handleDialog }) => {
  const { token, setToken, userID, orgaID, setRefreshHandler } = useContext(AuthContext);
  const [template, setTemplate] = useState(null);
  console.log(orgaID )




  const handleSubmit = () => {

    console.log(template);


    const name = document.getElementById("templateName").value;
    const description = document.getElementById("description").value;

    const data = {
        name: name,
        description: description,
        templateContent: template
    };

    console.log(data);



    console.log(data);

    const postTemplate = async () => {
        const response = await postCall(data, "/api/templates?organizationId=" + orgaID, "Error posting a new Template", token);
        console.log(response);
        if(response) {
            handleDialog();
            setRefreshHandler(response);

        }

    }

    postTemplate();

    };
    
    // try {
    //     const response = await postCall(stringData, "/api/organizations", "Error posting a new Organisation", token);
    //     console.log(response);
    //     if(response) {
    //         handleDialog();
    //         setRefreshHandler(response);
    //     }
    // } catch (error) {
    //     console.error(error);
    //     throw error;
    // };



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
        handleSubmit(formJson);

      },
    }}>
      <DialogTitle>Neues Template Anlegen</DialogTitle>
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
        <Button onClick={handleSubmit}>
          Template anlegen
        </Button>
      </DialogActions>
    </Dialog>
  );
};
