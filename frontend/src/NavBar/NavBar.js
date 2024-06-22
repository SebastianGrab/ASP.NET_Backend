
import OverlayMenu from './OverlayMenu';
import { Routes, Route, Outlet, Link, useNavigate } from "react-router-dom";

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faBars, faCircleArrowLeft, faHouse } from '@fortawesome/free-solid-svg-icons';
import { useState } from 'react';
import { Drawer } from '@mui/material';
import { SideDrawer } from './Drawer';


export default function NavBar(){
    const [isOverlayMenuOpen, setIsOverlayMenuOpen] = useState(false);
    const navigate = useNavigate();

    const menuHandler = () => {
        setIsOverlayMenuOpen(!isOverlayMenuOpen);
    }

   const  backHandler = () => {
        navigate(-1);
    }

    const homeHandler = () => {
        navigate("")
    }



    return(
        <>
        <div className='navbar'>

            <div>
            <FontAwesomeIcon icon={faCircleArrowLeft} size="2x" onClick={backHandler} className='navbar-icon'/>
            {/* <FontAwesomeIcon icon={faHouse}  style={{marginLeft: '30px'}} size="3x" className='navbar-icon' onClick={homeHandler}/> */}
            </div>
            <div className="navbar-logo-box">
            <img className='navbar-logo' src='/Images/DRK-Logo_kompakt_RGB.jpg' alt="DRK_Logo" onClick={homeHandler}/>
            </div>
            <FontAwesomeIcon icon={faBars} size="2x" className='navbar-icon' onClick={menuHandler}/>

        
            
            {/* <OverlayMenu isOpen={isOverlayMenuOpen} onClose={menuHandler} /> */}

        </div>

        <p/>
        <Outlet/>
        <SideDrawer open={isOverlayMenuOpen} toggleDrawer={menuHandler}></SideDrawer>
        

        </>
    )
}