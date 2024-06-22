import React, { useState, useContext } from 'react';
import { useNavigate } from 'react-router-dom';
import AuthContext from "../API/AuthProvider";
import { putCall } from "../API/putCall";
import Dialog from '@mui/material/Dialog';
import DialogTitle from '@mui/material/DialogTitle';
import DialogActions from '@mui/material/DialogActions';
import Button from '@mui/material/Button';
import TextField from '@mui/material/TextField';
import Box from '@mui/material/Box';

export default function ComposeMail({ protocolID, handleCloseComposeMail }) {
    const { token } = useContext(AuthContext);
    const [emailSubject, setEmailSubject] = useState('');
    const [emailContent, setEmailContent] = useState('');
    const navigate = useNavigate();
    const [open, setOpen] = useState(true);

    const handleSubmit = (e) => {
        e.preventDefault();
        const data = {
            id: protocolID,
            name: "string",
            sendEmail: true,
            emailSubject,
            emailContent,
        };

        putCall(data, `/api/protocol/${protocolID}`, 'Error sending email', token)
            .then(response => {
                console.log('Email sent successfully:', response);
                handleCloseComposeMail();
                navigate(-1);
            })
            .catch(error => {
                console.error(error);
            });
    };

    const handleClose = () => {
        setOpen(false);
        handleCloseComposeMail();
    };

    return (
        <Dialog open={open} onClose={handleClose}>
            <DialogTitle>Email verfassen</DialogTitle>
            <Box component="form" onSubmit={handleSubmit} sx={{ p: 2 }}>
                <TextField
                    margin="dense"
                    label="Betreff"
                    type="text"
                    fullWidth
                    variant="outlined"
                    value={emailSubject}
                    onChange={(e) => setEmailSubject(e.target.value)}
                    required
                />
                <TextField
                    margin="dense"
                    label="Inhalt"
                    type="text"
                    fullWidth
                    variant="outlined"
                    multiline
                    rows={4}
                    value={emailContent}
                    onChange={(e) => setEmailContent(e.target.value)}
                    required
                />
                <DialogActions>
                    <Button type="submit" variant="contained" color="primary">
                        Senden
                    </Button>
                    <Button type="button" onClick={handleClose} variant="contained" color="secondary">
                        Abbrechen
                    </Button>
                </DialogActions>
            </Box>
        </Dialog>
    );
}
