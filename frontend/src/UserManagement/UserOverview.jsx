
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faUserPlus } from '@fortawesome/free-solid-svg-icons';
import { NewUserDialog } from "./NewUserDialog";
import { UserTable } from "./UserTable";
import React, { useState, useEffect, useContext } from "react";
import AuthContext from "../API/AuthProvider";
import { getCall } from "../API/getCall";




export const UserOverview = () => {
    const [openDialog, setOpenDialog] = useState(false);
    const [orgaData, setOrgaData] = useState(null);
    const { token, setToken, refreshHandler } = useContext(AuthContext);


    const dialogHandlerNewUser = () => {
        setOpenDialog(!openDialog);
        console.log(openDialog);
    }



    return(
        <>
     
                <div className='row'>
                <h1>Nutzer Übersicht</h1>
                <FontAwesomeIcon icon={faUserPlus} size="2x" onClick={dialogHandlerNewUser} className='mgmt-icon'/>


                </div>

                <NewUserDialog open={openDialog} handleDialog={dialogHandlerNewUser} ></NewUserDialog>
                <UserTable ></UserTable>


        </>

    )
}