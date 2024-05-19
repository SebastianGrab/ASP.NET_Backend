import DRKLogo from '../Resources/Images/DRK_logo_logIn.png'
import { Routes, Route, Outlet, Link } from "react-router-dom";
import { sendLogIn } from '../API/sendLogIn';
import '../styles.css';

export default function LogInPage(){

    const getLogInData = () => {

        const mail = document.getElementById("mail").value;
        const pw = document.getElementById("pw").value;

        const data = {
            "email": mail,
            "password": pw
        }
        console.log(data);

        const response = sendLogIn(data);
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

            <input className="button" value="Anmelden" type="button" onClick={getLogInData}></input>

            <Link to="user" className="button">Dummy Nav</Link>

        </div>
        
        </>
    )
}