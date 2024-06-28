import DRKLogo from "../Resources/Images/DRK_logo_logIn.png";
import List from "@mui/material/List";
import ListItem from "@mui/material/ListItem";
import ListItemText from "@mui/material/ListItemText";
import ListItemAvatar from "@mui/material/ListItemAvatar";
import Avatar from "@mui/material/Avatar";
import ImageIcon from "@mui/icons-material/Image";
import WorkIcon from "@mui/icons-material/Work";
import BeachAccessIcon from "@mui/icons-material/BeachAccess";
import { Button, ListItemButton } from "@mui/material";
import { useNavigate } from "react-router-dom";
import React, { useContext, useEffect, useState } from "react";
import "../styles.css";
import AuthContext from "../API/AuthProvider";
import PersonOutlineIcon from "@mui/icons-material/PersonOutline";
import { useLocation } from "react-router-dom";
import { OrgaIcon } from "../UserManagement/AdditionalOrgasForUserList";

export const UserProfilChosser = () => {
  const {
    orgaID,
    userID,
    setUserID,
    setOrgaID,
    setToken,
    setRole,
    role,
    setOrgaName,
    orgaName,
  } = useContext(AuthContext);

  const location = useLocation();
  const loginData = location.state?.response;


  const ProfileItem = ({ orga, role }) => {
    const secondaryInfo = orga.organizationType + " - " + role.name;
    return (
      <ListItem>
        <ListItemButton onClick={() => setProfile({ orga, role })}>
          <ListItemAvatar>
            <Avatar>
              <OrgaIcon type={orga.organizationType}/>
            </Avatar>
          </ListItemAvatar>
          <ListItemText primary={orga.name} secondary={secondaryInfo} />
        </ListItemButton>
      </ListItem>
    );
  };

  const navigate = useNavigate();

  const clickHandler = () => {
    console.log(userID, "prga: ", orgaID);
    navigate(`/${userID}/${orgaID}`);
  };

  const setProfile = ({ orga, role }) => {
    setOrgaID(orga.id);
    //console.log(orga.id);
    console.log(orgaID);
    setRole(role.name);
    setOrgaName(orga.name);
  };
  return (
    <>
      <div className="container">
        <div>
          <img src={DRKLogo} alt="DRK_Logo" />
        </div>
        <h3>Wählen Sie Ihr Profil aus.</h3>

        <List
          sx={{ width: "100%", maxWidth: 360, bgcolor: "background.paper" }}
        >
          {loginData.uor.map((profile, index) => (
            <ProfileItem
            key={index}
              orga={profile.organization}
              role={profile.role}
            />
          ))}
        </List>

        <Button variant="outlined" onClick={clickHandler}>
          Als {role} für {orgaName} fortfahren.
        </Button>
      </div>
    </>
  );
};
