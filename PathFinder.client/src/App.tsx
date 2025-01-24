import React from 'react'
import { MapContainer, TileLayer } from 'react-leaflet'
import {Button, buttonVariants} from "@/components/ui/button.tsx";
import {cn} from "@/lib/utils.ts";

const App: React.FC = () => {
    console.log(cn(buttonVariants({})));


    // return (
    //     <div style={{ height: '100vh', width: '80%' }}>
    //         <Button className="bg-zinc-950">AAA</Button>
    //         <MapContainer
    //             center={[51.505, -0.09]}
    //             zoom={13}
    //             style={{ height: '100%', width: '100%' }}
    //         >
    //             <TileLayer
    //                 attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
    //                 url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
    //             />
    //         </MapContainer>
    //     </div>
    // );

    return (
        <div className="bg-amber-600">
            <Button>aaaaa</Button>
        </div>
    );
};

export default App;
