using System;
using System.Collections.Generic;
using ParkingLot.domain.parking;
using ParkingLot.domain.ticket;
using ParkingLot.domain.vechile;
using ParkingLot.services;
using ParkingLot.strategy;

namespace ParkingLot
{
    public class DemoApplication
    {
        public static void Main(string[] args)
        {
            ParkingSpot spot1 = new ParkingSpot("00-S1", VechileType.CAR);
            ParkingSpot spot2 = new ParkingSpot("01-S2", VechileType.BIKE);

            ParkingFloor groundFloor = new ParkingFloor("Ground", new List<ParkingSpot> { spot1 });
            ParkingFloor firstFloor = new ParkingFloor("First", new List<ParkingSpot> { spot2 });

            PricingStrategy strategy = new HourlyPricingStrategy(20);

            ParkingLotService service = new ParkingLotService(new List<ParkingFloor> { groundFloor, firstFloor }, strategy);
            ParkingLotApplication application = new ParkingLotApplication(service);

            Vechile bike = new Bike("ABC01");
            Vechile car = new Car("ABC02");

            Ticket bikeTicket = application.ParkVehicle(bike);
            Console.WriteLine("Bike Parked at spot: " + bikeTicket.getParkingSpot().GetId());

            Ticket carTicket = application.ParkVehicle(car);
            Console.WriteLine("Car Parked at spot: " + carTicket.getParkingSpot().GetId());

            Console.WriteLine("Fare: " + application.UnparkVehicle(bikeTicket));
            Console.WriteLine("Fare: " + application.UnparkVehicle(carTicket));
        }
    }
}
