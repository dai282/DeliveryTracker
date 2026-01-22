import React, { useEffect, useState } from 'react';
import { MapContainer, TileLayer, Marker, Popup, useMap } from 'react-leaflet';
import 'leaflet/dist/leaflet.css'; // CRITICAL IMPORT

// Fix for default marker icon in Leaflet + Vite/Webpack
// (Leaflet's default icon paths often break in bundlers)
import L from 'leaflet';
import icon from 'leaflet/dist/images/marker-icon.png';
import iconShadow from 'leaflet/dist/images/marker-shadow.png';

let DefaultIcon = L.icon({
    iconUrl: icon,
    shadowUrl: iconShadow,
    iconSize: [25, 41],
    iconAnchor: [12, 41]
});
L.Marker.prototype.options.icon = DefaultIcon;

function MapUpdater({center}){
    const map = useMap();

    useEffect(() =>{
        map.flyTo(center, map.getZoom());
    }, [center, map]);

    return null;
}

function DeliveryMap({shipment}) {

    // Default to a starting location (e.g., New York) if no data yet
    const defaultPosition = [40.7128, -74.0060]; 

    // Safety check: Does the shipment exist? If not, use default.
    const position = shipment ? [shipment.Latitude, shipment.Longitude] : defaultPosition;

    return (
        <MapContainer
            center={position} 
            zoom={13} 
            style={{ height: "400px", width: "100%", borderRadius: "10px" }}>

            <TileLayer
                attribution='&copy; <a href="https://www.openstreetmap.org/copyright">OpenStreetMap</a> contributors'
                url="https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png"
            />

            {/* If we have a shipment, show the marker */}
            {shipment && (
                <Marker position={position}>
                    <Popup>
                        <strong>Order #{shipment.ShipmentId}</strong> <br/>
                        Driver: {shipment.DriverId}
                    </Popup>
                </Marker>
            )}

            {/* Smoothly animate map to new position */}
            <MapUpdater center = {position} />
        </MapContainer>
    );
}

export default DeliveryMap;