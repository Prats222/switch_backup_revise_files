using ParkingLot.domain.ticket;
using ParkingLot.domain.vechile;
using ParkingLot.services;

namespace ParkingLot
{
    public class ParkingLotApplication
    {
        private readonly ParkingLotService _service;

        public ParkingLotApplication(ParkingLotService service)
        {
            _service = service;
        }

        public Ticket ParkVehicle(Vechile vechile)
        {
            return _service.ParkVehicle(vechile);
        }

        public double UnparkVehicle(Ticket ticket)
        {
            return _service.UnparkVehicle(ticket);
        }
    }
}
