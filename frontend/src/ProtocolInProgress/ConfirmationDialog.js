import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import Dialog from '@mui/material/Dialog';
import DialogTitle from '@mui/material/DialogTitle';
import DialogContent from '@mui/material/DialogContent';
import DialogContentText from '@mui/material/DialogContentText';
import DialogActions from '@mui/material/DialogActions';
import Button from '@mui/material/Button';
import ComposeMail from './ComposeMail';

export default function ConfirmationDialog({ handleShowConfirmation, protocolID, isInReview }) {
    const [composeMailOpen, setComposeMailOpen] = useState(false);
    const [open, setOpen] = useState(true);
    const navigate = useNavigate();

    const handleComposeEmail = () => {
        setComposeMailOpen(true);
        setOpen(false);
    };

    const handleCloseConfirmation = () => {
        setOpen(false);
        handleShowConfirmation();
    };

    const handleNavigateToHome = () => {
        setOpen(false);
        navigate(-1);
    };

    return (
        <>
            <Dialog open={open} onClose={handleCloseConfirmation}>
                <DialogTitle>Protokoll abgeschickt</DialogTitle>
                <DialogContent>
                    {isInReview ? (
                        <DialogContentText>
                            Das Protokoll wurde erfolgreich zur erneuten Überprüfung an Ihren Zugführer gesendet.
                        </DialogContentText>
                    ) : (
                        <DialogContentText>
                            Das Protokoll wurde erfolgreich an Ihren Zugführer gesendet. Möchten Sie aufgrund besonderer Vorkommnisse eine zusätzliche E-Mail an die Leiter verfassen?
                        </DialogContentText>
                    )}
                </DialogContent>
                <DialogActions>
                    {!isInReview && (
                        <Button onClick={handleComposeEmail} variant="contained" color="primary">
                            Ja, Email senden
                        </Button>
                    )}
                    <Button onClick={handleNavigateToHome} variant="contained" color="secondary">
                        Beenden
                    </Button>
                </DialogActions>
            </Dialog>

            {composeMailOpen && (
                <ComposeMail protocolID={protocolID} handleCloseComposeMail={() => setComposeMailOpen(false)} />
            )}
        </>
    );
}
