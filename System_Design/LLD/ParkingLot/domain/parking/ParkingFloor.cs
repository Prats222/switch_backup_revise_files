namespace ParkingLot.domain.parking;
using System.Collections.Generic;

public class ParkingFloor
{
    private string floorId;
    private List<ParkingSpot> parkingSpots;

    public ParkingFloor(string floorId, List<ParkingSpot> parkingSpots)
    {
        this.floorId = floorId;
        this.parkingSpots = parkingSpots;
    }

    public List<ParkingSpot> getParkingSpots()
    {
        return parkingSpots;
    }

    public string getFloorId()
    {
        return floorId;
    }
}