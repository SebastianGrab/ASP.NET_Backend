import DRKLogo from "../Resources/Images/DRK_logo_logIn.png";
import List from "@mui/material/List";
import ListItem from "@mui/material/ListItem";
import ListItemText from "@mui/material/ListItemText";
import ListItemAvatar from "@mui/material/ListItemAvatar";
import Avatar from "@mui/material/Avatar";
import { Button, ListItemButton } from "@mui/material";
import React, { useContext, useEffect, useState } from "react";
import "../styles.css";
import AuthContext from "../API/AuthProvider";
import PublicIcon from "@mui/icons-material/Public";
import HolidayVillageIcon from "@mui/icons-material/HolidayVillage";
import HomeIcon from "@mui/icons-material/Home";
import GroupIcon from "@mui/icons-material/Group";
import LocalHospitalIcon from "@mui/icons-material/LocalHospital";
import DeleteIcon from "@mui/icons-material/Delete";
import {
  Dialog,
  DialogContent,
  DialogContentText,
  DialogTitle,
} from "@mui/material";

import { deleteCall } from "../API/deleteCall";

export const AdditionalOrgasForUserList = ({ orgas, user }) => {
  const [open, setOpen] = useState(false);
  const [selectedOrga, setSelectedOrga] = useState(null);
  const { token, setRefreshHandler, refreshHander } =
  useContext(AuthContext);

  const handleOpen = (orga) => {
    console.log(orga);
    setSelectedOrga(orga);
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
    setSelectedOrga(null);
  };

  const removeOrgaFromUser = async ({orga}) => {
    console.log(orga);


    try{
        const response = await deleteCall("/api/user/" + 1 + "/organization/" + orga.orgaId );
        setRefreshHandler(response);

    } catch (error){
        console.log("Fheler beim entfernend es Users: " + error)

    }

  }

  const OrgaItem = ({ orgaName, orgaType, orgaId }) => {
    return (
      <ListItem>
        <ListItemButton onClick={() => handleOpen({ orgaName, orgaId })}>
          <ListItemAvatar>
            <Avatar>
              <OrgaIcon type={orgaType} />
            </Avatar>
          </ListItemAvatar>
          <ListItemText primary={orgaName} secondary={orgaType} />
          <DeleteIcon color="primary" />
        </ListItemButton>
      </ListItem>
    );
  };

  const OrgaIcon = ({ type }) => {
    switch (type) {
      case "Land":
        return <PublicIcon />;
      case "Kreisverband":
        return <HolidayVillageIcon />;
      case "Ortsverein":
        return <HomeIcon />;
      case "Einsatzformation":
        return <GroupIcon />;
      default:
        return <LocalHospitalIcon />;
    }
  };

  const ConfirmDeleteDialog = ({ orga }) => {
    return (
      <Dialog open={open} onClose={handleClose}>
        <DialogTitle>Nutzer aus dieser Organisation entfernen.</DialogTitle>
        <DialogContent>
          <DialogContentText>
            Sind Sie sicher, dass Sie den Nutzer: "{user?.username}" aus der Organisation "{orga?.orgaName}" entfernen möchten?
          </DialogContentText>
          <div className="row">

          <Button onClick={() => removeOrgaFromUser({orga})} variant="contained" color="primary">
                    Löschen!
                </Button>
                <Button onClick={handleClose} variant="contained" color="secondary">
                    Abbrechen
                </Button>


          </div>
        </DialogContent>
      </Dialog>
    );
  };

  return (
    <>
      <List sx={{ width: "100%", maxWidth: 360, bgcolor: "background.paper" }}>
        {orgas.map((orga, index) => (
          <OrgaItem
            key={index}
            orgaName={orga.name}
            orgaType={orga.organizationType}
            orgaId={orga.id}
          />
        ))}
      </List>

      {selectedOrga && <ConfirmDeleteDialog orga={selectedOrga} />}
    </>
  );
};
