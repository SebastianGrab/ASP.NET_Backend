import { LogInPage } from "./LogIn/LogInPage";
import NoMatch from "./NoMatch/NoMatch";
import NavBar from "./NavBar/NavBar";
import Dashboard from "./Dashboard/Dashboard";
import Archive from "./Archiv/Archive";
import Messages from "./Messages/Messages";
import NewProtocol from "./NewProtocol/NewProtocol";
import ProtocolInProgress from "./ProtocolInProgress/ProtocolInProgress";
import { TemplateChoice } from "./NewProtocol/TemplateChoice";
import PiP3 from "./ProtocolInProgress/PiP3";
import UpdateProtocolInProgress from "./ProtocolInProgress/UpdateProtocolInProgress";
import Stats from "./Stats/Stats";
import { AuthProvider } from "./API/AuthProvider";
import { Routes, Route, Outlet, Link, useNavigate } from "react-router-dom";

import "./styles.css";

export const App = () => {

  

  return (
    <>
      <AuthProvider>
        <Routes>
          <Route path="/" element={<LogInPage />} />
          <Route path="/:userId" element={<NavBar />}>
            <Route index element={<Dashboard />} />
            <Route path="newProtocol" element={<TemplateChoice />} />
            <Route path="newProtocol/:TemplateName" element={<NewProtocol />} />
            <Route path="newProtocol11" element={<NewProtocol />} />
            <Route path="protocolInProgress" element={<ProtocolInProgress />} />
            <Route path="protocolInProgress/:ProtocolID" element={<UpdateProtocolInProgress />} />

            <Route path="messages" element={<Messages />} />
            <Route path="archive" element={<Archive />} />
            <Route path="stats" element={<Stats />} />
            <Route path="*" element={<NoMatch />} />
          </Route>
        </Routes>
      </AuthProvider>
    </>
  );
}
