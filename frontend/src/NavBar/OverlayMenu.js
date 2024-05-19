
import { Routes, Route, Outlet, Link, useNavigate } from "react-router-dom";

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faXmark } from '@fortawesome/free-solid-svg-icons';

let pages = [
    { page: "", description: "Dashboard" },
    { page: "newProtocol", description: "Neues Protokoll" },
    { page: "protocolInProgress", description: "Protokoll in Bearbeitung" },
    { page: "messages", description: "Nachrichten" },
    { page: "archive", description: "Archiv" },
    { page: "stats", description: "Statistiken" }
  ];


export default function OverlayMenu({isOpen, onClose }){
    
    return(
        <>
        <div className={`overlay-menu ${isOpen ? 'open' : ''}`}>
            <FontAwesomeIcon className='close-menu-btn' icon={faXmark} size='2x' onClick={onClose}/>

            {pages.map((page) => (
                 <Link key={page.description} to={page.page} onClick={onClose} className="overlay-menu-link">{page.description}</Link>

            ))}

        </div>
        </>
    )
}