import { Routes, Route, Outlet, Link } from "react-router-dom";
import { faPenToSquare } from '@fortawesome/free-solid-svg-icons';
import { getAllProtocolsInProgress } from '../API/archive/getAllProtocolsInProgress';
import AuthContext from "../API/AuthProvider";
import { useContext, useEffect, useState } from "react";
import { getCall } from "../API/getCall";
import TileArchive from "../Components/TileArchive";
import TextField from '@mui/material/TextField';
import { LocalizationProvider, DatePicker } from '@mui/x-date-pickers';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs'; // Import AdapterDayjs
import dayjs from 'dayjs';

export default function Archive() {

    const [protocols, setProtocols] = useState(null);

    const {token, userID, orgaID, setUserID, setOrgaID, setToken} = useContext(AuthContext);
    const [ownerName, setOwnerName] = useState("");
    const [minCreatedDate, setMinCreatedDate] = useState(null);
    const [maxCreatedDate, setMaxCreatedDate] = useState(null); // State for the max date filter


    useEffect(() => {
        const storedloginData = JSON.parse(localStorage.getItem('loginData'));
        console.log(storedloginData);
        if (storedloginData) {
            setToken(storedloginData.token);
            setOrgaID(storedloginData.organizationId);
            setUserID(storedloginData.userId);
        }

        const fetchProtocols = (ownerName = "", minCreatedDate = null, maxCreatedDate = null) => {
            let url = `/api/protocols?pageIndex=1&pageSize=9999999&IsDraft=false&IsClosed=true`;
            if (ownerName) {
                url += `&OwnerName=${ownerName}`;
            }
            if (minCreatedDate) {
                url += `&minCreatedDateTime=${minCreatedDate.toISOString()}`;
            }
            if (maxCreatedDate) {
                url += `&maxCreatedDateTime=${maxCreatedDate.toISOString()}`;
            }
            getCall(url, token, "Error getting templates")
                .then((response) => {
                    setProtocols(response);
                    console.log("Get Templates successful!");
                    console.log(response.items[0]?.id);
                })
                .catch((error) => {
                    console.error(error);
                });
        };

        fetchProtocols(ownerName, minCreatedDate, maxCreatedDate); // Fetch protocols initially and when ownerName, minCreatedDate, or maxCreatedDate changes
    }, [orgaID, ownerName, minCreatedDate, maxCreatedDate]);

    console.log(protocols);

    return (
        <>

            <h1>Archiv</h1>

            <div className="row">

        

            <TextField
                label="Nach Ersteller suchen"
                value={ownerName}
                onChange={(e) => setOwnerName(e.target.value)}
            />
            <LocalizationProvider dateAdapter={AdapterDayjs}>
                <div style={{ marginLeft: '10px' }}>
                <DatePicker
                    label="Startdatum"
                    value={minCreatedDate}
                    onChange={(newValue) => {
                        setMinCreatedDate(newValue);
                    }}
                    renderInput={(params) => <TextField {...params} />}
                />
                </div>
                <div style={{ marginLeft: '10px' }}>
                <DatePicker
                    label="Enddatum"
                    value={maxCreatedDate}
                    onChange={(newValue) => {
                        setMaxCreatedDate(newValue);
                    }}
                    renderInput={(params) => <TextField {...params} />}
                />
                </div>
            </LocalizationProvider>
            </div>

            {protocols !== null
                ? protocols.items.map((protocol) => (
                    <TileArchive
                        pagePath={String(protocol.id)}
                        icon={faPenToSquare}
                        description={String(protocol.id)}
                        info={`Archiviert am: ${protocol.closedAt.substring(0, 10)}`}
                        payload={protocol}
                        key={protocol.id}
                    />
                ))
                : null}
        </>
    )

}


