using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace ParkingLotSingleFile
{
    // -----------------------------
    // Enums
    // -----------------------------
    public enum VehicleType
    {
        BIKE,
        CAR,
        TRUCK
    }

    // -----------------------------
    // Exceptions
    // -----------------------------
    public class ParkingException : Exception
    {
        public ParkingException(string message) : base(message)
        {
        }
    }

    // -----------------------------
    // Domain model
    // -----------------------------
    public abstract class Vehicle
    {
        private readonly string vehicleNumber;
        private readonly VehicleType vehicleType;

        protected Vehicle(string vehicleNumber, VehicleType vehicleType)
        {
            this.vehicleNumber = vehicleNumber;
            this.vehicleType = vehicleType;
        }

        public string GetVehicleNumber() => vehicleNumber;
        public VehicleType GetVehicleType() => vehicleType;
    }

    public class Bike : Vehicle
    {
        public Bike(string vehicleNumber) : base(vehicleNumber, VehicleType.BIKE)
        {
        }
    }

    public class Car : Vehicle
    {
        public Car(string vehicleNumber) : base(vehicleNumber, VehicleType.CAR)
        {
        }
    }

    public class Truck : Vehicle
    {
        public Truck(string vehicleNumber) : base(vehicleNumber, VehicleType.TRUCK)
        {
        }
    }

    public class ParkingSpot
    {
        private readonly object lockObject = new object();
        private readonly string id;
        private readonly VehicleType vehicleType;
        private Vehicle currentVehicle;

        public ParkingSpot(string id, VehicleType vehicleType)
        {
            this.id = id;
            this.vehicleType = vehicleType;
            currentVehicle = null;
        }

        public bool ParkVehicle(Vehicle vehicle)
        {
            lock (lockObject)
            {
                if (currentVehicle == null && vehicle.GetVehicleType() == vehicleType)
                {
                    currentVehicle = vehicle;
                    return true;
                }

                return false;
            }
        }

        public void RemoveVehicle()
        {
            lock (lockObject)
            {
                currentVehicle = null;
            }
        }

        public bool IsSpotAvailable()
        {
            lock (lockObject)
            {
                return currentVehicle == null;
            }
        }

        public string GetId() => id;
        public VehicleType GetVehicleType() => vehicleType;
    }

    public class ParkingFloor
    {
        private readonly string floorId;
        private readonly List<ParkingSpot> parkingSpots;

        public ParkingFloor(string floorId, List<ParkingSpot> parkingSpots)
        {
            this.floorId = floorId;
            this.parkingSpots = parkingSpots;
        }

        public List<ParkingSpot> GetParkingSpots() => parkingSpots;
        public string GetFloorId() => floorId;
    }

    public class Ticket
    {
        private readonly string ticketId;
        private readonly ParkingSpot parkingSpot;
        private readonly Vehicle vehicle;
        private readonly DateTime entryTime;
        private DateTime exitTime;
        private double charges;

        public Ticket(ParkingSpot parkingSpot, Vehicle vehicle)
        {
            this.ticketId = Guid.NewGuid().ToString();
            this.parkingSpot = parkingSpot;
            this.vehicle = vehicle;
            this.entryTime = DateTime.Now;
            this.exitTime = default;
            this.charges = 0;
        }

        public string GetTicketId() => ticketId;
        public ParkingSpot GetParkingSpot() => parkingSpot;
        public Vehicle GetVehicle() => vehicle;
        public DateTime GetEntryTime() => entryTime;
        public DateTime GetExitTime() => exitTime;
        public double GetCharges() => charges;

        public void SetExitTime(DateTime exitTime)
        {
            this.exitTime = exitTime;
        }

        public void CloseTicket(double charges)
        {
            this.charges = charges;
        }
    }

    // -----------------------------
    // Pricing strategy pattern
    // -----------------------------
    public interface PricingStrategy
    {
        double CalculatePrice(Ticket ticket);
    }

    public class HourlyPricingStrategy : PricingStrategy
    {
        private readonly double ratePerHour;

        public HourlyPricingStrategy(double ratePerHour)
        {
            this.ratePerHour = ratePerHour;
        }

        public double CalculatePrice(Ticket ticket)
        {
            if (ticket.GetExitTime() == default)
            {
                throw new InvalidOperationException("Exit time is not set for the ticket.");
            }

            TimeSpan duration = ticket.GetExitTime() - ticket.GetEntryTime();
            double hours = Math.Ceiling(duration.TotalHours);
            return hours * ratePerHour;
        }
    }

    // -----------------------------
    // Service layer
    // -----------------------------
    public class ParkingLotService
    {
        private readonly List<ParkingFloor> parkingFloors;
        private readonly ConcurrentDictionary<string, Ticket> activeTickets;
        private readonly PricingStrategy pricingStrategy;

        public ParkingLotService(List<ParkingFloor> parkingFloors, PricingStrategy pricingStrategy)
        {
            this.parkingFloors = parkingFloors;
            this.pricingStrategy = pricingStrategy;
            this.activeTickets = new ConcurrentDictionary<string, Ticket>();
        }

        public Ticket ParkVehicle(Vehicle vehicle)
        {
            ParkingSpot spot = GetAvailableParkingSpot(vehicle.GetVehicleType());

            if (!spot.ParkVehicle(vehicle))
            {
                throw new ParkingException("Parking spot was taken concurrently");
            }

            Ticket ticket = new Ticket(spot, vehicle);
            if (!activeTickets.TryAdd(ticket.GetTicketId(), ticket))
            {
                throw new ParkingException("Duplicate ticket insertion attempt: " + ticket.GetTicketId());
            }

            return ticket;
        }

        public double UnparkVehicle(Ticket ticket)
        {
            if (!activeTickets.ContainsKey(ticket.GetTicketId()))
            {
                throw new ParkingException("Invalid ticket: " + ticket.GetTicketId());
            }

            ticket.SetExitTime(DateTime.Now);
            ticket.CloseTicket(pricingStrategy.CalculatePrice(ticket));
            ticket.GetParkingSpot().RemoveVehicle();
            activeTickets.TryRemove(ticket.GetTicketId(), out _);

            return ticket.GetCharges();
        }

        private ParkingSpot GetAvailableParkingSpot(VehicleType vehicleType)
        {
            foreach (ParkingFloor floor in parkingFloors)
            {
                foreach (ParkingSpot spot in floor.GetParkingSpots())
                {
                    if (spot.GetVehicleType() == vehicleType && spot.IsSpotAvailable())
                    {
                        return spot;
                    }
                }
            }

            throw new ParkingException("No spot available for vehicle type " + vehicleType);
        }
    }

    public class ParkingLotApplication
    {
        private readonly ParkingLotService service;

        public ParkingLotApplication(ParkingLotService service)
        {
            this.service = service;
        }

        public Ticket ParkVehicle(Vehicle vehicle)
        {
            return service.ParkVehicle(vehicle);
        }

        public double UnparkVehicle(Ticket ticket)
        {
            return service.UnparkVehicle(ticket);
        }
    }

    // -----------------------------
    // Demo / Main
    // -----------------------------
    public static class SingleFileDemo
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== Parking Lot Demo ===");

            ParkingSpot spot1 = new ParkingSpot("00-S1", VehicleType.CAR);
            ParkingSpot spot2 = new ParkingSpot("01-S2", VehicleType.BIKE);

            ParkingFloor groundFloor = new ParkingFloor("Ground", new List<ParkingSpot> { spot1 });
            ParkingFloor firstFloor = new ParkingFloor("First", new List<ParkingSpot> { spot2 });

            PricingStrategy strategy = new HourlyPricingStrategy(20);
            ParkingLotService service = new ParkingLotService(new List<ParkingFloor> { groundFloor, firstFloor }, strategy);
            ParkingLotApplication application = new ParkingLotApplication(service);

            Vehicle bike = new Bike("ABC01");
            Vehicle car = new Car("ABC02");

            Ticket bikeTicket = application.ParkVehicle(bike);
            Console.WriteLine("Bike parked at spot: " + bikeTicket.GetParkingSpot().GetId());

            Ticket carTicket = application.ParkVehicle(car);
            Console.WriteLine("Car parked at spot: " + carTicket.GetParkingSpot().GetId());

            Console.WriteLine("Bike fare: " + application.UnparkVehicle(bikeTicket));
            Console.WriteLine("Car fare: " + application.UnparkVehicle(carTicket));
        }
    }
}
