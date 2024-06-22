import React from "react";
import { pages } from "./Pages";
import { useState, useEffect, useContext } from "react";
import AuthContext from "./API/AuthProvider";
import { Route, Routes } from "react-router-dom";
import Stats from "./Stats/Stats";


export const RenderRoutes = () => {
    const { token, userID, orgaID, role} = useContext(AuthContext);

    return(

        <Route path="stats" element={<Stats />} />
    )

}