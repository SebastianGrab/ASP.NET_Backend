import * as React from "react";
import { DataGrid } from "@mui/x-data-grid";
import { IconButton, TextField } from "@mui/material";
import { useContext, useState, useEffect } from "react";
import AuthContext from "../API/AuthProvider";
import FormControlLabel from "@mui/material/FormControlLabel";
import Switch from "@mui/material/Switch";
import EditNoteIcon from "@mui/icons-material/EditNote";
import { getCall } from "../API/getCall";
import { Button, ListItemButton } from "@mui/material";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import {
    faFileImport
} from "@fortawesome/free-solid-svg-icons";

import { Dialog, DialogTitle, DialogContent, DialogActions } from '@mui/material';
import { useNavigate } from 'react-router-dom';
import { postCall } from "../API/postCall";


export const ListAllTemplates = () => {

    const columnsFullScreen = [
        { field: "id", headerName: "ID", width: 70 },
        { field: "name", headerName: "Name", width: 350 },
        { field: "description", headerName: "Beschreibung", width: 500 },

      ];
    
      const columnsMobile = [
          { field: "id", headerName: "ID", width: 70 },
          { field: "name", headerName: "Name", width: 350 },
      ];
    
      const { token, setToken, orgaID, refreshHandler, setOrgaID, setRole, setUserID, setRefreshHandler } = useContext(AuthContext);
      const [view, setView] = useState("mobile");
      const [columns, setColumns] = useState(columnsMobile);
      const [userData, setUserData] = useState([]);
      const [sortRows, setRows] = useState([]);
      const [showEditUserDialog, setShowEditUserDialog] = useState(false);
      const [currentUser, setCurrentUser] = useState(null);
      const [open, setOpen] = useState(false);
      const [selectedRows, setSelectedRows] = useState([]);
      const [errorMessage, setErrorMessage] = useState(false);
      const navigate = useNavigate();
    

    useEffect(() => {
        const storedLoginData = JSON.parse(localStorage.getItem("loginData"));
        if (storedLoginData) {
            console.log(storedLoginData);
          setToken(storedLoginData.token);
          setOrgaID(storedLoginData.organizationId);
          setRole(storedLoginData.role);
          setUserID(storedLoginData.userId);
        }
    
        const fetchUsers = async () => {
          try {
            const result = await getCall( "/api/templates?pageIndex=1&pageSize=999999", token, "Fehler beim lesen aller Templates");
            setUserData(result.items);
            console.log(result);
            setRows(result.items);
            console.log(result.items);
          } catch (error) {
            console.error("getting All Templates:", error);
          }
        };
    
        if (token) {
          fetchUsers();
          setErrorMessage(false);
        }
      }, [token, setToken, refreshHandler]);

      const handleRowClick = (param) => {
        setShowEditUserDialog(true);
        setCurrentUser(param);
      };

      const toggleView = () => {
        if (view === "mobile") {
          setView("desktop");
          setColumns(columnsFullScreen);
        } else {
          setView("mobile");
          setColumns(columnsMobile);
        }
      };

      const filter = (event) => {
        const value = event.target.value.toLowerCase();
        setRows(
          value
            ? userData.filter((row) =>
                Object.values(row).join("").toLowerCase().includes(value)
              )
            : userData
        );
      };

      const addTemplatesToOrga = async () => {
        console.log(selectedRows);

        if(selectedRows.length >= 1){
            for(let i = 0; i<selectedRows.length; i++){
                try{
                    const response = await postCall("", "/api/template/" + selectedRows[i] + "/add-to-organization/" + orgaID,"Fehler beim hinzufügen des Tempaltes" , token);
                    setRefreshHandler(response);
    
                } catch(error){
                    console.log("Fehler beim hinzufügen des Tempaltes", error)
                    setErrorMessage(true);
                }
            }


            navigate(-1);
            setOpen(!open);
        }

      }



    return(
        <>

        <div className="row">
        <h1>Liste mit allen Templates im System</h1>
        <FontAwesomeIcon icon={faFileImport}  size="2x" onClick={() => setOpen(!open)} className='mgmt-icon' />

        </div>
    

                <div className="row">
        <input
          type="text"
          id="outlined-basic"
          label="Search"
          variant="outlined"
          placeholder="Filter"
          onChange={filter}
        />


        <FormControlLabel
          label="Ansicht wechseln"
          control={<Switch onChange={toggleView} color="secondary" />}
           labelPlacement="start"
        />
      </div>

      <div style={{ height: `auto`, width: "100%" }}>
        {sortRows.length > 0 && (
          <DataGrid
            rows={sortRows}
            columns={columns}
            initialState={{
              pagination: {
                paginationModel: { page: 0, pageSize: 50 },
              },
            }}
            pageSizeOptions={[50, 100]}
            checkboxSelection
            onRowSelectionModelChange={(newSelection) => {
                setSelectedRows(newSelection);
            }}

          />
        )}
      </div>

      <Dialog open={open} onClose={() => setOpen(false)}>
            <DialogTitle>Prüfung</DialogTitle>
            <DialogContent>
                <p>Wollen Sie {selectedRows.length} neue Templates Ihrer Organisation hinzufügen?</p>
            </DialogContent>
            {errorMessage ?
            <TextField>Fehler beim hinzufügen eines Templates. Möglicherweise, ist diese Template bereits Ihrer Organisation zugewiesen.</TextField>
            : null}
            <DialogActions>
                <Button variant="contained" color="primary" onClick={addTemplatesToOrga}>
                    Hinzufügen
                </Button>
                <Button variant="contained" color="primary" onClick={() => setOpen(false)}>
                    Abbrechen
                </Button>
            </DialogActions>
        </Dialog>
        </>

    )
}