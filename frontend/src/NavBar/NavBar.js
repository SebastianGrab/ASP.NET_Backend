
import OverlayMenu from './OverlayMenu';
import { Routes, Route, Outlet, Link, useNavigate } from "react-router-dom";

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faBars, faCircleArrowLeft } from '@fortawesome/free-solid-svg-icons';
import { useState } from 'react';


export default function NavBar(){
    const [isOverlayMenuOpen, setIsOverlayMenuOpen] = useState(false);
    const navigate = useNavigate();

    const menuHandler = () => {
        setIsOverlayMenuOpen(!isOverlayMenuOpen);
    }

   const  backHandler = () => {
        navigate(-1);
    }

    return(
        <>
        <div className='navbar'>

            <FontAwesomeIcon icon={faCircleArrowLeft} size="3x" onClick={backHandler} className='navbar-icon'/>
            <div className="navbar-logo-box">
            <img className='navbar-logo' src='/Images/DRK-Logo_kompakt_RGB.jpg' alt="DRK_Logo"/>
            </div>
            <FontAwesomeIcon icon={faBars} size="3x" className='navbar-icon' onClick={menuHandler}/>

            <OverlayMenu isOpen={isOverlayMenuOpen} onClose={menuHandler} />

        </div>

        <p/>
        <Outlet/>
        
        </>
    )
}