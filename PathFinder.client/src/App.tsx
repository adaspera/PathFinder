import React from 'react'
import { MapContainer, TileLayer } from 'react-leaflet'
import {Button} from "@mui/material";

const App: React.FC = () => {

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
            </MapContainer>
        </div>
    );
};

export default App;
