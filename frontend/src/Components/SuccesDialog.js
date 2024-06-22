import React from 'react';
import { Dialog, DialogTitle, DialogContent, DialogActions, Button } from '@mui/material';
import { useNavigate } from 'react-router-dom';

export default function SuccessDialog({ open, handleClose, header, text }) {
    const navigate = useNavigate();

    const handleOk = () => {
        handleClose(); // Schließe den Dialog
        navigate('..'); // Navigiere zur vorherigen Seite
    };

    return (
        <Dialog open={open} onClose={handleClose}>
            <DialogTitle>{header}</DialogTitle>
            <DialogContent>
                <p>{text}</p>
            </DialogContent>
            <DialogActions>
                <Button onClick={handleOk} variant="contained" color="primary">
                    Ok
                </Button>
            </DialogActions>
        </Dialog>
    );
}