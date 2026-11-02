namespace ParkingLot.domain.vechile;

public abstract class Vechile
{
    private string vechileNumber;
    private VechileType vechileType;

    public Vechile(string vechileNumber, VechileType vechileType)
    {
        this.vechileNumber = vechileNumber;
        this.vechileType = vechileType;
    }

    public string GetVechileNumber()
    {
        return vechileNumber;
    }

    public VechileType GetVechileType()
    {
        return vechileType;
    }
}