import React, {useEffect, useState} from 'react'
import {MapContainer, Marker, Popup, TileLayer} from 'react-leaflet'
import {Button} from "@mui/material";
import {getStopsByFeedId, Stop} from "@/api/stopApi.ts";

const App: React.FC = () => {
    const [stops, setStops] = useState<Stop[]>([]);

    useEffect(() => {
        getStopsByFeedId("mdb-195").then(setStops);
    }, []);

    return (
        <div style={{ height: '100vh', width: '80%' }}>
            <Button>aaaaaaa</Button>
            <MapContainer
                center={[51.505, -0.09]}
                zoom={13}
                style={{ height: '100%', width: '100%' }}
            >
                <TileLayer
                    attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
                    url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
                />
                {stops.map((stop) => (
                    <Marker
                        key={stop.id}
                        position={[stop.latitude, stop.longitude]}
                    >
                        <Popup>
                            <div>
                                <h3>{stop.name}</h3>
                                <p>Zone: {stop.zoneId}</p>
                                <p>ID: {stop.id}</p>
                            </div>
                        </Popup>
                    </Marker>
                ))}
            </MapContainer>
        </div>
    );
};

export default App;
