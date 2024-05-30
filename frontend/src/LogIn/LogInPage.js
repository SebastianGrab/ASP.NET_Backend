import DRKLogo from '../Resources/Images/DRK_logo_logIn.png'
import { Routes, Route, Outlet, Link, useNavigate } from "react-router-dom";
import { sendLogIn } from '../API/archive/sendLogIn';
import React, { createContext, useContext, useEffect, useState } from 'react';
import '../styles.css';
import  AuthContext  from '../API/AuthProvider';
import { postCall } from "../API/postCall";
import { logInEP }   from '../API/endpoints';

export const LogInPage = () => {
    const navigate = useNavigate();
    const { setUserID, setOrgaID, setToken} = useContext(AuthContext);

    const [authData, setAuthData] = useState(null);

    const logIn = () => {

        const mail = document.getElementById("mail").value;
        const pw = document.getElementById("pw").value;

        const data = {
            "email": mail,
            "password": pw
        }


        postCall(data, logInEP.ep, logInEP.errMsg)
        .then((response) => {
            setAuthData(response);


        })
        .catch((error) => {
            console.error('Fehler beim Aufruf von sendLogIn:', error);
        });



    }

    useEffect(() => {
        if(authData !== null){
            console.log(authData);
            handleLogInResponse(authData)
        }
      }, [authData]);



    const handleLogInResponse = (response) => {
        if(response.token !== "Wrong username or password."){
            localStorage.setItem('loginData', JSON.stringify(response));
            setToken(response.token);
            setOrgaID(response.organizationId);
            setUserID(response.userId);

            navigate(`${response.userId}`);
        
        }




    }

    return(
        <>
        <div className="container">

            <div>
            <img src={DRKLogo} alt="DRK_Logo"/>
            </div>


            <h2>Willkommen!</h2>
            <h3>Melden Sie sich an.</h3>
            <label>
            <input type="text" id="mail"></input>
            </label>
    
            <input type="password" id="pw"></input>
            <p></p>

            <input className="button" value="Anmelden" type="button" onClick={logIn}></input>

        </div>
        
        </>
    )
}