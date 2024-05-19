import { Routes, Route, Outlet, Link } from "react-router-dom";
import { useState } from 'react'
import {FlexibleXYPlot, LineSeries,VerticalBarSeries, XAxis, YAxis, VerticalGridLines, HorizontalGridLines} from 'react-vis';

function Stats() {
    // Daten für das erste Linien-Diagramm
    const data1 = [
        { x: 0, y: 10 },
        { x: 1, y: 15 },
        { x: 2, y: 20 },
        { x: 3, y: 25 },
        { x: 4, y: 30 }
    ];

    // Daten für das zweite Linien-Diagramm
    const data2 = [
        { x: 0, y: 8 },
        { x: 1, y: 12 },
        { x: 2, y: 16 },
        { x: 3, y: 20 },
        { x: 4, y: 24 }
    ];

    return (
        <>
            <div style={{ display: 'flex' }}>
               {/* Erste Kachel */}
                <div className="tile-diagram" style={{ flex: '1', marginRight: '10px' }}>
                    <FlexibleXYPlot height={250}>
                        <LineSeries data={data1} />
                        <VerticalGridLines />
                        <HorizontalGridLines />
                        <XAxis />
                        <YAxis />
                    </FlexibleXYPlot>
                </div>
                <div className="tile-diagram" style={{ flex: '1' }}>
                    <FlexibleXYPlot  height={250}>
                        <VerticalBarSeries data={data2} />
                        <XAxis />
                        <YAxis />
                    </FlexibleXYPlot>
                </div>
            </div>
            <div style={{ display: 'flex' }}>
                {/* Erste Kachel */ }
               <div className="tile-diagram" style={{ flex: '1', marginRight: '10px' }}>
                    <FlexibleXYPlot height={250}>
                        <VerticalBarSeries data={data1} />
                        <XAxis />
                        <YAxis />
                    </FlexibleXYPlot>
                </div>
                <div className="tile-diagram" style={{ flex: '1' }}>
                    <FlexibleXYPlot  height={250}>
                        <LineSeries data={data2} />
                        <XAxis />
                        <YAxis />
                    </FlexibleXYPlot>
                </div>
            </div>
        </>
    );
}

export default Stats;

