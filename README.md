# Real-Time Distributed Delivery Tracker 🚚

A robust full-stack distributed system designed to handle high-frequency geospatial data. This project demonstrates a scalable event-driven architecture utilizing **.NET 10**, **Apache Kafka**, and **SignalR**.

---

## 📖 Overview
This application tracks delivery assets in real-time, showcasing how to decouple high-throughput data ingestion from background processing. By leveraging a message broker, the system ensures data integrity and system responsiveness even under heavy loads.

## ✨ Key Features

*   🚀 **High Throughput:** API ingestion is decoupled from data processing via Kafka, allowing the system to handle bursts of geospatial updates.
*   🔄 **Real-Time Updates:** Uses SignalR to push location data instantly from the server to connected React clients.
*   🛡️ **Fault Tolerance:** Robust error handling and retry logic for Kafka connectivity issues.
*   🐳 **Containerization:** The entire ecosystem (Brokers, Backend, Frontend) is orchestrated via Docker Compose for easy deployment.

---

## 📸 Demo 
![2026-01-2316-47-04-ezgif com-crop](https://github.com/user-attachments/assets/cab71900-4bc7-43dd-ad53-9cc25d83f507)

---
## 🛠 Tech Stack

| Layer | Technologies |
| :--- | :--- |
| **Backend** | .NET 10, ASP.NET Core Web API, Background Services (`IHostedService`) |
| **Messaging** | Apache Kafka (KRaft mode) |
| **Real-time** | SignalR (WebSockets) |
| **Frontend** | React (Vite), Leaflet.js (Geospatial mapping) |
| **Infrastructure** | Docker & Docker Compose |
---

## 🏗 Architecture Diagram
<img width="1118" height="682" alt="image" src="https://github.com/user-attachments/assets/5ec03685-09b5-4af9-acd1-74de286c2844" />

---

## 🚀 Getting Started

### Prerequisites
*   [Docker Desktop](https://www.docker.com/products/docker-desktop/)
*   [Node.js](https://nodejs.org/) (v18+)
*   [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)

### Installation & Setup

1.  **Spin up the Infrastructure**
    ```bash
    docker-compose up -d
    ```
    *This starts Kafka, SQL Server, and the Backend services.*

2.  **Start the Frontend**
    ```bash
    cd client
    npm install
    npm run dev
    ```

3.  **Access the Dashboard**
    Open [http://localhost:5173](http://localhost:5173) in your browser.

---

## 📡 API Usage

To simulate a delivery update, send a `POST` request to the following endpoint:

**Endpoint:** `http://localhost:8080/api/shipments/update`

**Sample Payload:**
```json
{
    "driverId": "Dave-007",
    "shipmentId": "PKG-998877",
    "latitude": 40.7580,
    "longitude": -79.9855,
    "timestamp": 1705500000
}
```

