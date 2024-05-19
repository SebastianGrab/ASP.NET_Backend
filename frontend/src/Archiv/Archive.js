import { Routes, Route, Outlet, useNavigate } from "react-router-dom";
import TileArchive from "../Components/TileArchive";


export default function Archive(){

    return(
        <>

            <h1>Archive</h1>
            <TileArchive/>
            <TileArchive/>
            <TileArchive/>
            <TileArchive/>

        </>
    )
}