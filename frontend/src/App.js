import LogInPage from './LogIn/LogInPage';
import NoMatch from './NoMatch/NoMatch';
import NavBar from './NavBar/NavBar';
import Dashboard from './Dashboard/Dashboard';
import Archive from './Archiv/Archive';
import Messages from './Messages/Messages';
import NewProtocol from './NewProtocol/NewProtocol';
import ProtocolInProgress from './ProtocolInProgress/ProtocolInProgress';
import PiP3 from './ProtocolInProgress/PiP3';
import PiP4 from './ProtocolInProgress/PiP4';
import Stats from './Stats/Stats';
import { Routes, Route, Outlet, Link, useNavigate } from "react-router-dom";

import './styles.css';

export default function App() {

    let navigation = useNavigate();

    return (
        <>
            <Routes>
                <Route path="/" element={<LogInPage />} />
                <Route path="/user" element={<NavBar />}>

                    <Route index element={<Dashboard />} />
                    <Route path="newProtocol" element={<NewProtocol />} />
                    <Route path="protocolInProgress" element={<ProtocolInProgress />} />
                    <Route path="protocolInProgress/3" element={<PiP3 />} />
                    <Route path="protocolInProgress/4" element={<PiP4 />} />


                    <Route path="messages" element={<Messages />} />
                    <Route path="archive" element={<Archive />} />
                    <Route path="stats" element={<Stats />} />
                    <Route path="*" element={<NoMatch />} />

                </Route>


            </Routes>

        </>
    )

}

