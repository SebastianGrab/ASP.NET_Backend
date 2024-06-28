import * as React from 'react';
import { BarChart } from '@mui/x-charts/BarChart';
import { PieChart } from '@mui/x-charts/PieChart';
import { useParams } from 'react-router-dom';
import { TextField } from '@mui/material';
import { useEffect, useState } from 'react';
import { LocalizationProvider } from '@mui/x-date-pickers/LocalizationProvider';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs';
import { DatePicker } from '@mui/x-date-pickers/DatePicker';
import dayjs from 'dayjs';

export default function FullScreenChart() {
    const [data, setData] = useState([]);
    const [userData, setUserData] = useState([]);
    const [token, setToken] = useState('');
    const [orgaID, setOrgaID] = useState('');
    const [userID, setUserID] = useState('');
    const [yearlyData, setYearlyData] = useState([]);
    const [placeData, setPlaceData] = useState([]);
    const [organizationData, setOrganizationData] = useState([]);
    const [typeData, setTypeData] = useState([]);

    useEffect(() => {
        const storedloginData = JSON.parse(localStorage.getItem('loginData'));
        // if (storedloginData) {
        //     setToken(storedloginData.token);
        //     setUserID(storedloginData.userId);
        // }

        if (storedloginData.token) {
            const fetchData = async () => {
                try {
                    const response1 = await getCall("/api/statistics/number-of-protocols-by-date", storedloginData.token, "Error getting data");
                    setData(response1);

                    const response2 = await getCall("/api/statistics/number-of-protocols-by-user", storedloginData.token, "Error getting user data");
                    setUserData(response2);

                    const response4 = await getCall("/api/statistics/number-of-protocols-by-year", storedloginData.token, "Error getting yearly data");
                    setYearlyData(response4);

                    const response5 = await getCall("/api/statistics/number-of-protocols-by-place", storedloginData.token, "Error getting place data");
                    setPlaceData(response5);

                    const response6 = await getCall("/api/statistics/number-of-protocols-by-organization", storedloginData.token, "Error getting organization data");
                    setOrganizationData(response6);

                    const response7 = await getCall("/api/statistics/number-of-protocols-by-type", storedloginData.token, "Error getting type data");
                    setTypeData(response7);

                    console.log("Get Data successful!");
                } catch (error) {
                    console.error(error);
                }
            };

            fetchData();
        }
    }, []);

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

    const displayData = {
        bar1: {
            type: 'bar',
            title: 'Anzahl Einsätze nach Tag',
            xAxis: {
                id: 'barCategories',
                data: chartLabels,
                scaleType: 'band',
            },
            series: [
                { data: chartData },
            ],
        },
        pie1: {
            type: 'pie',
            title: 'Anzahl Einsätze nach Helfer',
            series:
                [
                    { data: pieChartData }
                ]
        },
        pie2: {
            type: 'pie',
            title: 'Anzahl Einsätze nach Ort',
            series: [
                {
                    data: placeChartData,
                },
            ],
        },

        pie3: {
            type: 'pie',
            title: 'Anzahl Einsätze nach Alarmschlüssel',
            series: [
                {
                    data: organizationChartData,
                },
            ],
        },
        bar2: {
            type: 'bar',
            title: 'Anzahl Einsätze nach Jahren',
            xAxis: {
                id: 'barCategories',
                data: yearlyChartLabels,
                scaleType: 'band',
                tickLabelInterval: () => true,
            },
            series: [
                { data: yearlyChartData },
            ],
        },
        pie4: {
            type: 'pie',
            title: 'Anzahl Einsätze nach Einsatzart',
            series: [
                {
                    data: typeChartData,
                },
            ],
        },

    };

    const { type } = useParams();
    const currentChartData = displayData[type];

    const [filteredValue, setFilterValue] = useState('');
    const [filteredData, setFilteredData] = useState(currentChartData.series[0].data);

    const [startDate, setStartDate] = useState(dayjs().subtract(1, 'month'));
    const [endDate, setEndDate] = useState(dayjs());

    useEffect(() => {
        filterData(filteredValue, startDate, endDate);
    }, [filteredValue, startDate, endDate, currentChartData]);

    const handleFilterChange = (event) => {
        setFilterValue(event.target.value);
        filterData(event.target.value, startDate, endDate);
    };

    const handleStartDateChange = (date) => {
        setStartDate(date);
        filterData(filteredValue, date, endDate);
    };

    const handleEndDateChange = (date) => {
        setEndDate(date);
        filterData(filteredValue, startDate, date);
    };

    const filterData = (filter, start, end) => {
        let filteredNames = [];

        if (currentChartData.type === 'pie') {
            filteredNames = currentChartData.series[0].data.filter((item) => {
                const matchesFilter = item.label.toLowerCase().includes(filter.toLowerCase());
                return matchesFilter;
            });
        } else {
            filteredNames = currentChartData.series[0].data;
        }

        const filteredByDate = filteredNames.filter((item, index) => {
            let itemDate;
            if (currentChartData.type === 'bar') {
                itemDate = dayjs(currentChartData.xAxis.data[index], 'DD.MM.YYYY');
            } else {
                itemDate = dayjs(item.label, 'DD.MM.YYYY');
            }
            return itemDate.isBetween(start, end, null, '[]');
        });

        setFilteredData(filteredByDate);
    };

    return (
        <div style={{ display: 'flex', flexDirection: 'column', alignItems: 'center' }}>
            <div style={{ display: 'flex', flexDirection: 'row', alignItems: 'center', justifyContent: 'flex-start', marginBottom: '20px' }}>
                <div style={{ display: 'flex', alignItems: 'center' }}>
                    {currentChartData.type === 'pie' && (
                        <TextField
                            label="Nach Inhalt suchen"
                            value={filteredValue}
                            onChange={handleFilterChange}
                        />
                    )}
                    <div style={{ marginLeft: '20px' }}>
                        <LocalizationProvider dateAdapter={AdapterDayjs}>
                            <DatePicker
                                label="Startdatum"
                                value={startDate}
                                onChange={handleStartDateChange}
                            />
                        </LocalizationProvider>
                    </div>
                    <div style={{ marginLeft: '20px' }}>
                        <LocalizationProvider dateAdapter={AdapterDayjs}>
                            <DatePicker
                                label="Enddatum"
                                value={endDate}
                                onChange={handleEndDateChange}
                            />
                        </LocalizationProvider>
                    </div>
                </div>
            </div>
            <div className="tile-diagram-fullscreen" style={{ marginTop: '20px' }}>
                <h4 className="chart-title">{currentChartData.title}</h4>
                {currentChartData.type === 'bar' && (
                    <BarChart
                        xAxis={currentChartData.xAxis ? [currentChartData.xAxis] : []}
                        series={[{ data: filteredData }]}
                        width={window.innerWidth * 0.7}
                        height={window.innerHeight * 0.5}
                    />
                )}
                {currentChartData.type === 'pie' && (
                    <PieChart
                        series={[{ data: filteredData }]}
                        width={window.innerWidth * 0.7}
                        height={window.innerHeight * 0.5}
                    />
                )}
                {currentChartData.type === 'barstack' && (
                    <BarChart
                        xAxis={currentChartData.xAxis ? [currentChartData.xAxis] : []}
                        series={[{ data: filteredData }]}
                        width={window.innerWidth * 0.7}
                        height={window.innerHeight * 0.5}
                    />
                )}
            </div>
        </div>
    );
}
