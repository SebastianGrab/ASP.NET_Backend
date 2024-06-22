import React, { useState, useEffect, useContext } from "react";
import AuthContext from "../API/AuthProvider";
import AddBusinessIcon from '@mui/icons-material/AddBusiness';
import { NewOrganizationDialog } from "./NewOrganizationDialog";
import { OrganizationsTable } from "./OrganizationsTable";
import { getCall } from "../API/getCall";



export const OrganizationOverview = () => {
    const [openDialog, setOpenDialog] = useState(false);
    const [orgaData, setOrgaData] = useState(null);
    const { token, setToken, refreshHandler } = useContext(AuthContext);

    useEffect(() => {
        const storedLoginData = JSON.parse(localStorage.getItem("loginData"));
        if (storedLoginData) {
          setToken(storedLoginData.token);
        }
        const fetchUsers = async () => {
          try {
            const result = await getCall("/api/organizations?pageIndex=1&pageSize=9999999", token, "Error getting Organizations");
            console.log(result);
            setOrgaData(result.items);
          } catch (error) {
            console.error("Error fetching roles:", error);
          }
        };
        if (token) {
          fetchUsers();
        }
      }, [token, setToken, refreshHandler]);


    const dialogHandler = () => {
        setOpenDialog(!openDialog);
        console.log(openDialog);
    }



    return(
        <>
        <div className="row">
        <h1>Organisations Übersicht</h1>
        <AddBusinessIcon sx={{fontSize: 50 }} color= "secondary" onClick={dialogHandler}></AddBusinessIcon>
        </div>

                <NewOrganizationDialog open={openDialog} handleDialog={dialogHandler} allOrgas={orgaData}></NewOrganizationDialog>
                {orgaData !== null
                ?                 <OrganizationsTable data={orgaData}></OrganizationsTable>
                :null
            }



        </>

    )
}