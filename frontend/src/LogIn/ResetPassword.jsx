import React from "react";
import Button from "@mui/material/Button";
import Dialog from "@mui/material/Dialog";
import DialogActions from "@mui/material/DialogActions";
import DialogContent from "@mui/material/DialogContent";
import DialogContentText from "@mui/material/DialogContentText";
import DialogTitle from "@mui/material/DialogTitle";
import { putCall } from "../API/putCall";


export const ResetPassword = ({ open, handleDialog }) => {
  const handleReset = async (mail) => {
    const encodedMail = encodeURIComponent(mail);
    const response = await putCall("", `/api/reset-password/${encodedMail}`, "Fehler beim Zurücksetzen des Passworts.");
    console.log(response);
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
          const mail = formJson.email;
          handleReset(mail);
        },
      }}
    >
      <DialogTitle>Passwort zurücksetzen</DialogTitle>
      <DialogContent sx={{ minWidth: "350px" }}>
        <DialogContentText></DialogContentText>
        <div>
          <label htmlFor="email">Nutzer Email :</label>
          <div>
            <input type="text" name="email" id="email"></input>
          </div>
          <Button onClick={handleDialog}>Abbruch</Button>
          <Button type="submit">Neues Passwort erhalten</Button>
        </div>
      </DialogContent>
    </Dialog>
  );
};
