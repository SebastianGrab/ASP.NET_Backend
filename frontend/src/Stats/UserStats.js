import * as React from 'react';
import { DatePicker } from '@mui/x-date-pickers/DatePicker';
import { BarChart } from '@mui/x-charts/BarChart';
import Typography from '@mui/material/Typography';
import { PieChart } from '@mui/x-charts/PieChart';
import { Link } from 'react-router-dom';
import { useEffect, useState } from 'react';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import TextField from '@mui/material/TextField';
import dayjs from 'dayjs';

export default function UserStats() {
    const [startDate, setStartDate] = useState(dayjs().subtract(1, 'month'));
    const [endDate, setEndDate] = useState(dayjs());
    const [data, setData] = useState([]);
    const [userData, setUserData] = useState([]);
    const [token, setToken] = useState('');
    const [orgaID, setOrgaID] = useState('');
    const [userID, setUserID] = useState('');
    const [totalProtocols, setTotalProtocols] = useState(0);
    const [yearlyData, setYearlyData] = useState([]);
    const [placeData, setPlaceData] = useState([]);
    const [organizationData, setOrganizationData] = useState([]);
    const [typeData, setTypeData] = useState([]);

    useEffect(() => {
        const storedloginData = JSON.parse(localStorage.getItem('loginData'));
        if (storedloginData) {
            setToken(storedloginData.token);
            setOrgaID(storedloginData.organizationId);
            setUserID(storedloginData.userId);
        }

        if (storedloginData && storedloginData.token) {
            fetchData(storedloginData.token, startDate, endDate);
        }
    }, [startDate, endDate]);

    const fetchData = async (token, date) => {
        const minDate = date.format('YYYY-MM-DD');
        const startOfYear = dayjs().startOf('year').format('YYYY-MM-DD');
        const maxDate = date.format('YYYY-MM-DD');
        try {
            const response1 = await getCall(`/api/statistics/number-of-protocols-by-date?minDate=${minDate}`, token, "Error getting data");
            setData(response1);

            const response2 = await getCall(`/api/statistics/number-of-protocols-by-user?minDate=${minDate}`, token, "Error getting user data");
            setUserData(response2);

            const response3 = await getCall(`/api/statistics/number-of-protocols??minDate=${startOfYear}`, token, "Error getting total protocols");
            setTotalProtocols(response3);

            const response4 = await getCall(`/api/statistics/number-of-protocols-by-year`, token, "Error getting yearly data");
            setYearlyData(response4);

            const response5 = await getCall(`/api/statistics/number-of-protocols-by-place?minDate=${minDate}`, token, "Error getting place data");
            setPlaceData(response5);

            const response6 = await getCall(`/api/statistics/number-of-protocols-by-organization?minDate=${minDate}`, token, "Error getting organization data");
            setOrganizationData(response6);

            const response7 = await getCall(`/api/statistics/number-of-protocols-by-type?minDate=${minDate}`, token, "Error getting type data");
            setTypeData(response7);

            console.log("Get Data successful!");
        } catch (error) {
            console.error(error);
        }
    };

    const getCall = (url, token, errorMessage) => {
        return fetch(url, {
            headers: {
                Authorization: `Bearer ${token}`,
            },
        })
            .then(response => response.json())
            .catch(error => {
                console.error(errorMessage, error);
                throw error;
            });
    };

    const chartData = data.map(item => item.count);
    const chartLabels = data.map(item => item.date);

    const pieChartData = userData.map(item => ({
        id: item.user.id,
        value: item.count,
        label: item.user.username
    }));

    const yearlyChartData = yearlyData.map(item => item.count);
    const yearlyChartLabels = yearlyData.map(item => item.year.toString());

    const placeChartData = placeData.map((item, index) => ({
        id: index,
        value: item.count,
        label: item.place
    }));

    const organizationChartData = organizationData.map((item, index) => ({
        id: index,
        value: item.count,
        label: item.organization.name
    }));

    const typeChartData = typeData.map((item, index) => ({
        id: index,
        value: item.count,
        label: item.type
    }));

    return (
        <div style={{ display: 'flex', flexDirection: 'column', alignItems: 'center' }}>
            <div style={{ display: 'flex', alignItems: 'center' }}>
                <div style={{ marginLeft: '20px' }}>
                    <LocalizationProvider dateAdapter={AdapterDayjs}>
                        <DatePicker
                            label="Startdatum"
                            value={startDate}
                            onChange={(newValue) => setStartDate(newValue)}
                            renderInput={(params) => <TextField {...params} />}
                        />
                    </LocalizationProvider>
                </div>
                <div style={{ marginLeft: '20px' }}>
                    <LocalizationProvider dateAdapter={AdapterDayjs}>
                        <DatePicker
                            label="Enddatum"
                            value={endDate}
                            onChange={(newValue) => setEndDate(newValue)}
                            renderInput={(params) => <TextField {...params} />}
                        />
                    </LocalizationProvider>
                </div>
            </div>
            <div className="tile-diagram" style={{ flex: '1', flexDirection: 'column', justifyContent: 'center', alignItems: 'center' }}>
                <Typography variant="h6" style={{ textAlign: 'center' }}>
                    {totalProtocols}
                </Typography>
                <Typography variant="caption">
                    Anzahl der Einsätze in diesem Jahr
                </Typography>
            </div>

                <div className="tile-diagram" style={{ flex: '1' }}>
                    <h4 className={"chart-title"}>Anzahl Einsätze nach Tag</h4>
                    <BarChart
                        xAxis={[
                            {
                                id: 'barCategories',
                                data: chartLabels,
                                scaleType: 'band',
                            },
                        ]}
                        series={[
                            {
                                data: chartData,
                            },
                        ]}
                        width={500}
                        height={300}
                    />
                </div>

                <div className="tile-diagram" style={{ flex: '1' }}>
                    <h4 className={"chart-title"}>Anzahl Einsätze nach Einsatzart</h4>
                    <PieChart
                        series={[
                            {
                                data: typeChartData,
                            },
                        ]}
                        width={500}
                        height={200}
                    />
                </div>
        </div>
    );
}
