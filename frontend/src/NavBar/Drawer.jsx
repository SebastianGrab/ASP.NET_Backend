import * as React from 'react';
import Box from '@mui/material/Box';
import Drawer from '@mui/material/Drawer';
import Button from '@mui/material/Button';
import List from '@mui/material/List';
import Divider from '@mui/material/Divider';
import ListItem from '@mui/material/ListItem';
import ListItemButton from '@mui/material/ListItemButton';
import ListItemText from '@mui/material/ListItemText';
import { useNavigate } from 'react-router-dom';
import { useContext } from 'react';
import AuthContext from '../API/AuthProvider';
import { useState, useEffect } from 'react';


export const SideDrawer = ({open, toggleDrawer}) => {
  let pagesAdmin = [
    { page: "", description: "Dashboard" },
    { page: "newProtocol", description: "Neues Protokoll" },
    { page: "protocolInProgress", description: "Protokoll in Bearbeitung" },
    { page: "messages", description: "Nachrichten" },
    { page: "archive", description: "Archiv" },
    { page: "stats", description: "Statistiken" },
    { page: "protocolsToReview", description: "Protokolle zur Überprüfung" },
    { page: "userOverview", description: "Nutzer Übersicht" },
    { page: "organizationsOverview", description: "Organisations Übersicht" },
    { page: "templateManager", description: "Template Übersicht" },
    
  ];

  let pagesHelper = [
    { page: "", description: "Dashboard" },
    { page: "newProtocol", description: "Neues Protokoll" },
    { page: "protocolInProgress", description: "Protokoll in Bearbeitung" },
    { page: "messages", description: "Nachrichten" },
    { page: "archive", description: "Archiv" },
    { page: "stats", description: "Statistiken" },
  ];
    const navigate = useNavigate();
    const {setRole, setToken, setUserID, setOrgaID, role } = useContext(AuthContext);
    const [drawer, setDrawerPoints] = useState(pagesHelper);


    const logOut = () => {
        setToken(null);
        setUserID(null);
        setOrgaID(null);
        setRole(null);
        navigate('/');
    }

    const drawerPoints =() => {
      if(role === "Admin" || role === "Leiter"){
        return pagesAdmin;
      } else{
        return pagesHelper;
      }
    }

    useEffect(() => {
      setDrawerPoints(drawerPoints());
      }, [role]);
  

    

    

      const handleClick = (path) => {
        navigate(path);
      }


  const DrawerList = (
    <Box sx={{ width: 250}} role="presentation" onClick={toggleDrawer}>
      <List>
        {drawer.map((page, index) => (
          <ListItem key={index} disablePadding>
            <ListItemButton onClick={() => handleClick(page.page)}>
              <ListItemText primary={page.description} />
            </ListItemButton>
          </ListItem>
        ))}
      </List>
      <Divider />
      <List>
      <ListItem disablePadding>
            <ListItemButton onClick={logOut}>
              <ListItemText primary="Abmelden"/>
            </ListItemButton>
          </ListItem>
      
      </List>
    </Box>

  );

  return (
    <div>
      <Drawer open={open} onClose={toggleDrawer} anchor="right" >
        {DrawerList}
      </Drawer>
    </div>
  );
}