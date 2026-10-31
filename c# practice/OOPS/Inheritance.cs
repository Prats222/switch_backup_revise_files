//parent class
public class Person
{
    public string FullName { get; set; }
    public Person(string fullname){
        FullName=fullname;
    }
    public virtual void Introduce()
    {
        System.Console.WriteLine($"Hi i am {FullName}");
    }
}
public class Manager : Person
{
    public string Department { get; set; }
    public Manager(string fullName, string department): base(fullName)
    {
        Department=department;
    }
    public override void Introduce()
    {
        System.Console.WriteLine($"{FullName} ullu log {Department}");
    }
}

public class Program
{
    public static void Main()
    {
        Person p = new Person("PratsJod");
        Manager mc = new Manager("mc","QE");

        p.Introduce();
        mc.Introduce();
    }
}