import { useState } from 'react'
import './App.css'
import useDeliveryStream from './hooks/useDeliveryStream'
import DeliveryMap from './components/DeliveryMap';

function App() {

  const {latestUpdate, connectionStatus} = useDeliveryStream();

  return (
    <div className='app-container'>
      <header>
        <h1>📦 Real-Time Delivery Tracker</h1>
        <div className={`status-badge ${connectionStatus.toLowerCase()}`}>
          SignalR: {connectionStatus}
        </div>
      </header>

      <main>
        <div className='map-wrapper'>
          <DeliveryMap shipment = {latestUpdate} />
        </div>

        <div className='info-panel'>
          <h3>Live Shipment Data</h3>
          {latestUpdate ? (
            <pre>{JSON.stringify(latestUpdate, null, 2)}</pre>
          ) : (
            <p>Waiting for shipment updates...</p>
          )}

        </div>
      </main>
    </div>
  )
}

export default App
