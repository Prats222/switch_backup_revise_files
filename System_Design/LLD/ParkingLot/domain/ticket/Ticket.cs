namespace ParkingLot.domain.ticket;

using System;
using ParkingLot.domain.parking;
using ParkingLot.domain.vechile;

public class Ticket
{
    private string ticketId;
    private ParkingSpot parkingSpot;
    private Vechile vechile;
    private DateTime entryTime;

    private DateTime exitTime;
    private double charges;

    public Ticket(ParkingSpot spot, Vechile vechile)
    {
        this.ticketId = Guid.NewGuid().ToString();
        this.parkingSpot = spot;
        this.vechile = vechile;
        this.entryTime = DateTime.Now;
    }

    public void closeTicket(double charges)
    {
        this.charges = charges;
    }

    public void setExitTime(DateTime exitTime)
    {
        this.exitTime = exitTime;
    }

    public string getTicketId()
    {
        return ticketId;
    }

    public ParkingSpot getParkingSpot()
    {
        return parkingSpot;
    }

    public Vechile getVechile()
    {
        return vechile;
    }

    public DateTime getEntryTime()
    {
        return entryTime;
    }

    public DateTime getExitTime()
    {
        return exitTime;
    }

    public double getCharges()
    {
        return charges;
    }
}