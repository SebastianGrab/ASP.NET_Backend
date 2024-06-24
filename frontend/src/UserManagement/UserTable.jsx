import * as React from "react";
import { DataGrid } from "@mui/x-data-grid";
import { IconButton} from "@mui/material";
import { useContext, useState, useEffect } from "react";
import AuthContext from "../API/AuthProvider";
import { buildTableData, getUsers } from "./UserManagementService";
import FormControlLabel from "@mui/material/FormControlLabel";
import Switch from "@mui/material/Switch";
import EditNoteIcon from "@mui/icons-material/EditNote";
import { EditUserDialog } from "./EditUserDialog";
export const UserTable = () => {
  const columnsFullScreen = [
    {
      field: "actions",
      headerName: "Actions",
      width: 100,
      renderCell: (params) => (
        <IconButton onClick={() => handleRowClick(params.row)}>
          <EditNoteIcon />
        </IconButton>
      ),
    },
    { field: "id", headerName: "ID", width: 70 },
    { field: "username", headerName: "Nutzer", width: 250 },
    { field: "firstName", headerName: "Vorname", width: 130 },
    { field: "lastName", headerName: "Nachname", width: 130 },
    { field: "email", headerName: "E-Mail", width: 250 },
    { field: "role", headerName: "Rolle", width: 130 },
    { field: "organization", headerName: "Organisation", width: 130 },
  ];

  const columnsMobile = [
    {
      field: "actions",
      headerName: "Actions",
      width: 100,
      renderCell: (params) => (
        <IconButton onClick={() => handleRowClick(params.row)}>
          <EditNoteIcon />
        </IconButton>
      ),
    },
    { field: "id", headerName: "ID", width: 70 },
    { field: "username", headerName: "Nutzer", width: 250 },
  ];

  const { token, setToken, orgaID, refreshHandler, orgaName } = useContext(AuthContext);
  const [view, setView] = useState("mobile");
  const [columns, setColumns] = useState(columnsMobile);
  const [userData, setUserData] = useState([]);
  const [sortRows, setRows] = useState([]);
  const [showEditUserDialog, setShowEditUserDialog] = useState(false);
  const [currentUser, setCurrentUser] = useState(null);
  const [globalUser, setGlobalUser] = useState(true);

  useEffect(() => {
    const storedLoginData = JSON.parse(localStorage.getItem("loginData"));
    if (storedLoginData) {
      setToken(storedLoginData.token);
    }

    const fetchUsers = async () => {
      try {
        const result = await getUsers(token, orgaID);
        setUserData(result.items);
        setRows(buildTableData(result.items, token));
      } catch (error) {
        console.error("Error fetching roles:", error);
      }
    };

    if (token) {
      fetchUsers();
    }
  }, [token, setToken, refreshHandler]);

  const toggleView = () => {
    if (view === "mobile") {
      setView("desktop");
      setColumns(columnsFullScreen);
    } else {
      setView("mobile");
      setColumns(columnsMobile);
    }
  };

  const changeData = () => {

    if(globalUser){
    const value = orgaName.toLowerCase();
    setRows(
        userData.filter((row) =>
            Object.values(row).join("").toLowerCase().includes(value)
    ));
    setGlobalUser(!globalUser);
  } else {
    setRows(userData);
    setGlobalUser(!globalUser);
  }
  }


  const handleRowClick = (param) => {
    setShowEditUserDialog(true);
    setCurrentUser(param);
  };

  const handleDialog = () => {
    setShowEditUserDialog(!showEditUserDialog);
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
      setGlobalUser(!globalUser);

  };

  return (
    <>
      {currentUser !== null ? (
        <EditUserDialog
          open={showEditUserDialog}
          handleDialog={handleDialog}
          user={currentUser}
        ></EditUserDialog>
      ) : null}

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
          label="Alle Nutzer anzeigen"
          control={<Switch onChange={changeData} color="secondary" defaultChecked />}
           labelPlacement="start"
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
          />
        )}
      </div>
    </>
  );
};
