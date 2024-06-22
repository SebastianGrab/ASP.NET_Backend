import * as React from "react";
import { DataGrid } from "@mui/x-data-grid";
import { IconButton, TextField } from "@mui/material";
import { useContext, useState, useEffect } from "react";
import AuthContext from "../API/AuthProvider";
import FormControlLabel from "@mui/material/FormControlLabel";
import Switch from "@mui/material/Switch";
import EditNoteIcon from "@mui/icons-material/EditNote";
import { getCall } from "../API/getCall";
import { EditOrganisationDialog } from "./EditOrganisationDialog";
import { addParentNames } from "./OrganizationService";

export const OrganizationsTable = ({data}) => {
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
    { field: "name", headerName: "Name", width: 250 },
    { field: "city", headerName: "Stadt", width: 130 },
    { field: "postalCode", headerName: "PLZ", width: 130 },
    { field: "address", headerName: "Adresse", width: 250 },
    { field: "parentName", headerName: "Mutterorganisation", width: 250 },
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
    { field: "name", headerName: "Name", width: 250 },
  ];

  const { token, setToken, refreshHandler } = useContext(AuthContext);
  const [view, setView] = useState("mobile");
  const [columns, setColumns] = useState(columnsMobile);
  const [orgaData, setOrgaData] = useState([]);
  const [sortRows, setRows] = useState([]);
  const [showEditOrgaDialog, setShowEditOrgaDialog] = useState(false);
  const [currentOrga, setCurrentOrga] = useState(null);




  useEffect(() => {
    setOrgaData(data);
    setRows(data);
    console.log(addParentNames(data));

  }, [data, refreshHandler]);

  const toggleView = () => {
    if (view === "mobile") {
      setView("desktop");
      setColumns(columnsFullScreen);
    } else {
      setView("mobile");
      setColumns(columnsMobile);
    }
  };

  const handleRowClick = (param) => {
    setShowEditOrgaDialog(true);
    setCurrentOrga(param);
  };

  const handleDialog = () => {
    setShowEditOrgaDialog(!showEditOrgaDialog);
  }

  const filter = (event) => {
    const value = event.target.value.toLowerCase();
    setRows(value ? data.filter((row) => Object.values(row).join("").toLowerCase().includes(value)) : data);
  };

  return (
    <>
    {currentOrga !== null
    ?       <EditOrganisationDialog open={showEditOrgaDialog} handleDialog={handleDialog} orga={currentOrga} allOrgas={orgaData}></EditOrganisationDialog>
  : null}
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
          />
        )}
      </div>
    </>
  );
};
