namespace ParkingLot.domain.parking;

using ParkingLot.domain.vechile;

public class ParkingSpot
{
    private string id;
    private VechileType vechileType;

    private readonly object _lock = new object();
    private Vechile vechile;

    public ParkingSpot(string id, VechileType vechileType)
    {
        this.id = id;
        this.vechileType = vechileType;
    }

    public bool ParkVechile(Vechile vechile)
    {
        lock (_lock)
        {
            if (this.vechile == null && vechile.GetVechileType() == this.vechileType)
            {
                this.vechile = vechile;
                return true;
            }
            return false;
        }
    }

    public void removeVechile()
    {
        lock (_lock)
        {
            this.vechile = null;
        }
    }

    public bool isSpotAvailable()
    {
        lock (_lock)
        {
            return this.vechile == null;
        }
    }

    public string GetId()
    {
        return id;
    }

    public VechileType GetVechileType()
    {
        return vechileType;
    }
}