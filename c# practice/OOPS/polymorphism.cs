public class Animal
{
    public virtual void MakeSound()
    {
        Console.WriteLine("Some generic animal sound");
    }
}

public class Dog : Animal
{
    // Method Overriding
    public override void MakeSound()
    {
        Console.WriteLine("Bark!");
    }
}

public class Calculator
{
    // Method Overloading (Compile-time polymorphism)
    public int Add(int a, int b) => a + b;
    public double Add(double a, double b) => a + b;
}