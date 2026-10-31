using System;
using System.Collections.Generic;

public class Emp
{
    //fields
    private string name; 
    private decimal baseSalary; // name and salary are fields
    //Encapsulation - getter setters for private name.
    public string Name { get{return name;} set{name=value;} }

    // constructor
    public Emp(string name,decimal baseSalary)
    {
        Name = name;
        this.baseSalary=baseSalary;// now we have initialized it, now user can make new emp every time, else we would have to write manual code like emp.Name="xyz"
    }
    //method
    public virtual void DisplayInfo()
    {
        System.Console.WriteLine($"Emp : {Name}, Salary : {baseSalary}");
    }
}
class Program
{
    public static void Main(string[] args)
    {
        Emp e1 = new Emp("Prats",36000);
        e1.DisplayInfo();
    }
}