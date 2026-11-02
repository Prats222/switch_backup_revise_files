using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using ParkingLot.domain.parking;
using ParkingLot.domain.ticket;
using ParkingLot.domain.vechile;
using ParkingLot.exception;
using ParkingLot.strategy;

namespace ParkingLot.services
{
    public class ParkingLotService
    {
        private readonly ConcurrentDictionary<string, Ticket> _activeTickets;
        private readonly List<ParkingFloor> _parkingFloors;
        private PricingStrategy _pricingStrategy;

        public ParkingLotService(List<ParkingFloor> parkingFloors, PricingStrategy pricingStrategy)
        {
            _parkingFloors = parkingFloors;
            _activeTickets = new ConcurrentDictionary<string, Ticket>();
            _pricingStrategy = pricingStrategy;
        }

        public Ticket ParkVehicle(Vechile vechile)
        {
            ParkingSpot spot = GetAvailableParkingSpot(vechile.GetVechileType());

            if (!spot.ParkVechile(vechile))
            {
                throw new ParkingException("Parking spot was taken concurrently");
            }

            Ticket ticket = new Ticket(spot, vechile);

            if (!_activeTickets.TryAdd(ticket.getTicketId(), ticket))
            {
                throw new ParkingException($"Duplicate ticket insertion attempt: {ticket.getTicketId()}");
            }

            return ticket;
        }

        public double UnparkVehicle(Ticket ticket)
        {
            if (!_activeTickets.ContainsKey(ticket.getTicketId()))
            {
                throw new ParkingException("Invalid ticket: " + ticket.getTicketId());
            }

            ticket.setExitTime(DateTime.Now);
            ticket.closeTicket(_pricingStrategy.calculatePrice(ticket));
            ticket.getParkingSpot().removeVechile();

            _activeTickets.TryRemove(ticket.getTicketId(), out _);

            return ticket.getCharges();
        }

        private ParkingSpot GetAvailableParkingSpot(VechileType vechileType)
        {
            foreach (ParkingFloor parkingFloor in _parkingFloors)
            {
                foreach (ParkingSpot spot in parkingFloor.getParkingSpots())
                {
                    if (spot.GetVechileType() == vechileType && spot.isSpotAvailable())
                    {
                        return spot;
                    }
                }
            }

            throw new ParkingException("No spot available for vehicle type " + vechileType);
        }

        public void SetPricingStrategy(PricingStrategy pricingStrategy)
        {
            _pricingStrategy = pricingStrategy;
        }
    }
}
