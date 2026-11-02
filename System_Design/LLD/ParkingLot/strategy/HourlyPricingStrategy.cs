using System;
using ParkingLot.domain.ticket;

namespace ParkingLot.strategy;

public class HourlyPricingStrategy : PricingStrategy
{
    private readonly double ratePerHour;

    public HourlyPricingStrategy(double ratePerHour)
    {
        this.ratePerHour = ratePerHour;
    }

    public double calculatePrice(Ticket ticket)
    {
        DateTime entryTime = ticket.getEntryTime();
        DateTime exitTime = ticket.getExitTime();

        var duration = exitTime - entryTime;
        var hours = Math.Ceiling(duration.TotalHours);

        return hours * ratePerHour;
    }
}