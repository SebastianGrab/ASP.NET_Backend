import { LogInPage } from "./LogIn/LogInPage";
import NoMatch from "./NoMatch/NoMatch";
import NavBar from "./NavBar/NavBar";
import Dashboard from "./Dashboard/Dashboard";
import Archive from "./Archiv/Archive";
import Messages from "./Messages/Messages";
import NewProtocol from "./NewProtocol/NewProtocol";
import ProtocolInProgress from "./ProtocolInProgress/ProtocolInProgress";
import { TemplateChoice } from "./NewProtocol/TemplateChoice";
import UpdateProtocolInProgress from "./ProtocolInProgress/UpdateProtocolInProgress";
import Stats from "./Stats/Stats";
import FullScreenChart from "./Stats/FullScreenChart";
import { AuthProvider } from "./API/AuthProvider";
import { Routes, Route } from "react-router-dom";
import ProtocolsToReview from "./Leader/ProtocolsToReview";
import ReviewProtocol from "./Leader/ReviewProtocol";
import ArchiveProtocol from "./Archiv/ArchiveProtocol";
import { UserOverview } from "./UserManagement/UserOverview";
import { OrganizationOverview } from "./OrganizationManagement/OrganizationsOverview";
import { PrivateRoute, AdminRoute } from './PrivateRoute';
import { UserProfilChosser } from "./LogIn/UserProfilChooser";
import UserStats from "./Stats/UserStats";
import React from 'react';
import TemplatePreview from "./TemplateManagement/TemplatePreview";
import ComposeMail from "./ProtocolInProgress/ComposeMail";
import { TemplateManagement } from "./TemplateManagement/TemplateManagement";
import { ListAllTemplates } from "./TemplateManagement/ListAllTemplates";
import "./styles.css";

export const App = () => {
  return (
    <>
      <AuthProvider>
        <Routes>
          <Route path="/" element={<LogInPage />} />
          <Route path="/:userId/chooseProfile" element={<UserProfilChosser />} />
          <Route path="/:userId/:orgaId" element={<NavBar />}>
          <Route element={<PrivateRoute />}>

              <Route index element={<Dashboard />} />
              <Route path="newProtocol" element={<TemplateChoice />} />
              <Route path="newProtocol/:TemplateName" element={<NewProtocol />} />
              <Route path="protocolInProgress" element={<ProtocolInProgress />} />
              <Route path="protocolInProgress/:ProtocolID" element={<UpdateProtocolInProgress />} />
              <Route path="protocolInProgress/:ProtocolID/composeMail" element={<ComposeMail />} />
              <Route path="messages" element={<Messages />} />
              <Route path="archive" element={<Archive />} />
              <Route path="archive/:protocolID" element={<ArchiveProtocol />} />
              <Route path="userStats" element={<UserStats />} />
            </Route>
            <Route element={<AdminRoute />}>
              <Route path="userOverview" element={<UserOverview />} />
              <Route path="organizationsOverview" element={<OrganizationOverview />} />
              <Route path="protocolsToReview" element={<ProtocolsToReview />} />
              <Route path="protocolsToReview/:ProtocolID" element={<ReviewProtocol />} />
              <Route path="templateManager" element={<TemplateManagement />} />
              <Route path="templateManager/ListAllTemplates" element={<ListAllTemplates />} />
              <Route path="templateManager/:TemplateName" element={<TemplatePreview />} />
              <Route path="stats" element={<Stats />} />
              <Route path="stats/chart/:type" element={<FullScreenChart />} />
            </Route>

            <Route path="*" element={<NoMatch />} />
          </Route>
        </Routes>
      </AuthProvider>
    </>
  );
};
