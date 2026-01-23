import { useState, useEffect } from "react";
import * as signalR from "@microsoft/signalr";

function useDeliveryStream() {
  const [latestUpdate, setLatestUpdate] = useState(null);
  const [connectionStatus, setConnectionStatus] = useState("Disconnected");

  useEffect(() => {
    //1. Configure connection
    const connection = new signalR.HubConnectionBuilder()
      .withUrl("http://localhost:8080/trackingHub")
      .withAutomaticReconnect()
      .build();

    //2. Define the listener before starting the connection
    // "ReceiveUpdate" must match the string in your .NET Background Worker: .SendAsync("ReceiveUpdate", ...)
    connection.on("ReceiveUpdate", (message) => {
      console.log("Update received", message);

      const data = typeof message === "string" ? JSON.parse(message) : message;
      console.log("Parsed Data:", data); // Check your console! It should show Capitalized Keys

      setLatestUpdate(data);
    });

    //3. Start the connection
    const startConnection = async () => {
      try {
        await connection.start();
        setConnectionStatus("Connected");
        console.log("SignalR Connected");
      } catch (err) {
        console.error("SignalR Connection Failed: ", err);
        setConnectionStatus("Error");
      }
    };

    startConnection();

    // 4. Cleanup: Stop connection when the component unmounts
    return () => {
      connection.stop();
    };
  }, []);

  return { latestUpdate, connectionStatus };
}

export default useDeliveryStream;
