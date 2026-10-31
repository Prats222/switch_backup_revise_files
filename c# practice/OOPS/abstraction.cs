using System;

public interface Ipayable
{
    decimal CalculatePay();
}

//abstract
public abstract class Worker : Ipayable
{
    public string Name { get; set; }

    protected Worker(string name)
    {
        Name = name;
    }
    public abstract decimal CalculatePay(); // derived must implement now
}
public class Contractor : Worker
{
     public decimal HourlyRate { get; set; }
    public int HoursWorked { get; set; }

    public Contractor(string name, decimal hourlyRate, int hoursWorked) : base(name)
    {
        HourlyRate = hourlyRate;
        HoursWorked = hoursWorked;
    }
    public override decimal CalculatePay()
    {
        return HourlyRate*HoursWorked;
    }
}