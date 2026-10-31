public class BankAccount
{
    private decimal balance; //private field
    
    public decimal Balance { get{return balance;} 
    private set //getter setter same as prop, just adjusted.
        {
            if (value > 0)
            {
                balance=value;
            }
        } }
    public BankAccount(decimal balance)
    {
        Balance = balance;
    }
    public void Deposit(decimal amount)
    {
        if(amount>0) Balance+=amount;
        System.Console.WriteLine(Balance);
    }
    public bool Withdraw(decimal amount)
    {
        if (amount > 0 && amount<=Balance)
        {
            Balance-=amount;
            System.Console.WriteLine(Balance);
            return true;
        }
        return false;
    }
}
class Program
{
    public static void Main()
    {
        BankAccount acc = new BankAccount(600000.5m);
        acc.Withdraw(20);
        acc.Deposit(600);
    }
}