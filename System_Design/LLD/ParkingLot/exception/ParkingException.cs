using System;

namespace ParkingLot.exception;

public class ParkingException : Exception
{
    public ParkingException(string message) : base(message)
    {
    }
}