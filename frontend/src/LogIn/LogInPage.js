import DRKLogo from '../Resources/Images/DRK_logo_logIn.png';
import { useNavigate } from "react-router-dom";
import React, { useContext, useEffect, useState } from 'react';
import '../styles.css';
import AuthContext from '../API/AuthProvider';
import { postCall } from "../API/postCall";
import { logInEP } from '../API/endpoints';
import { ResetPassword } from './ResetPassword';

export const LogInPage = () => {
    const navigate = useNavigate();
    const { setUserID, setOrgaID, setToken, setRole } = useContext(AuthContext);

    const [authData, setAuthData] = useState(null);
    const [showDialog, setShowDialog] = useState(null);
    const [errorMessage, setErrorMessage] = useState("");

    const logIn = async () => {
        const mail = document.getElementById("mail").value;
        const pw = document.getElementById("pw").value;

        const data = {
            email: mail,
            password: pw
        };

        try {
            const response = await postCall(data, logInEP.ep, logInEP.errMsg);
            if (response) {
                console.log(response);
                setAuthData(response);
            }
        } catch (error) {
            setErrorMessage("Falsche Email oder falsches Passwort");
        }
    };

    const handleDialog = () => {
        setShowDialog(!showDialog);
    };

    useEffect(() => {
        if (authData !== null) {
            handleLogInResponse(authData);
        }
    }, [authData]);

    const handleLogInResponse = (response) => {
        if (response.token !== "Wrong username or password.") {
            localStorage.setItem('loginData', JSON.stringify(response));
            setToken(response.token);
            setOrgaID(response.organizationId);
            setUserID(response.userId);
            setRole(response.role);
            navigate(`${response.userId}` + "/chooseProfile", { state: {response}});

        } else {
            setErrorMessage("Falsche Email oder falsches Passwort");
        }
    };

    return (
        <>
            {showDialog !== null ? <ResetPassword open={showDialog} handleDialog={handleDialog} /> : null}

            <div className="container">
                <div>
                    <img src={DRKLogo} alt="DRK_Logo" />
                </div>
                <h2>Willkommen!</h2>
                <h3>Melden Sie sich an.</h3>
                <label>
                    <input type="text" id="mail" placeholder="Email" />
                </label>
                <input type="password" id="pw" placeholder="Passwort" />
                {errorMessage && <p style={{ color: 'red' }}>{errorMessage}</p>}
                <input className="button" value="Anmelden" type="button" onClick={logIn} />

                <div className='row' onClick={handleDialog}>Passwort zurücksetzen</div>
            </div>

            
        </>
    );
};
