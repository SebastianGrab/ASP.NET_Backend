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
import FullScreenChart from "./Stats/FullScreenChart"


export const pages = [
    {path: "/", name:"LogInPage", component:<LogInPage/>, isAdmin:false , isManager:false , isPublic:true },
    {path: "/:userId", name:"NavBar", component:<NavBar/>, isAdmin: false, isManager: false, isPublic: false },
    {path: "", name:"Dashboard", component:<Dashboard/>, isAdmin: false, isManager: false, isPublic: false },
    {path: "newProtocol", name:"TemplateChoice", component:<TemplateChoice/>, isAdmin: false, isManager: false, isPublic: false },
    {path: "newProtocol/:TemplateName", name:"NewProtocol", component:<NewProtocol/>, isAdmin: false, isManager: false, isPublic: false },
    {path: "protocolInProgress", name:"protocolInProgress", component:<ProtocolInProgress/>, isAdmin: false, isManager: false, isPublic: false },
    {path: "protocolInProgress/:ProtocolID", name:"NavBar", component:<UpdateProtocolInProgress/>, isAdmin: false, isManager: false, isPublic: false },
    {path: "messages", name:"Messages", component:<Messages/>, isAdmin: false, isManager: false, isPublic: false },
    {path: "archive", name:"Archive", component:<Archive/>, isAdmin: false, isManager: false, isPublic: false },
    {path: "statistics", name:"Statistics", component:<Stats/>, isAdmin: false, isManager: false, isPublic: false },
    {path: "statistics/chart/:type", name:"FullScreenChart", component:<FullScreenChart/>, isAdmin: false, isManager: false, isPublic: false },
    {path: "*", name:"NoMatch", component:<NoMatch/>, isAdmin: false, isManager: false, isPublic: false },
]