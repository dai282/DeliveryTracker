namespace TrackerAPI;

// A "record" gives us immutability and value equality for free.
public record ShipmentUpdate(
    string DriverId, 
    string ShipmentId, 
    double Latitude, 
    double Longitude, 
    long Timestamp
);