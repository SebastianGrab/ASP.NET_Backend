import React from "react";
import Dialog from "@mui/material/Dialog";
import DialogTitle from "@mui/material/DialogTitle";
import DialogContent from "@mui/material/DialogContent";
import DialogContentText from "@mui/material/DialogContentText";
import DialogActions from "@mui/material/DialogActions";
import Button from "@mui/material/Button";

export default function PiPdialog({ open, handleClose, handleSend }) {
    return (
        <Dialog open={open} onClose={handleClose}>
            <DialogTitle>Protokoll absenden</DialogTitle>
            <DialogContent>
                <DialogContentText>
                    Sie sind im Begriff, das Protokoll an Ihren Zugführer zu senden. Sie
                    haben danach keine Möglichkeit mehr, Änderungen vorzunehmen.
                </DialogContentText>
            </DialogContent>
            <DialogActions>
                <Button onClick={handleSend} variant="contained" color="primary">
                    Absenden
                </Button>
                <Button onClick={handleClose} variant="contained" color="secondary">
                    Abbrechen
                </Button>
            </DialogActions>
        </Dialog>
    );
}
