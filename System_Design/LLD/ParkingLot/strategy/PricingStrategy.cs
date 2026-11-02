using ParkingLot.domain.ticket;

namespace ParkingLot.strategy;

public interface PricingStrategy
{
    double calculatePrice(Ticket ticket);
}