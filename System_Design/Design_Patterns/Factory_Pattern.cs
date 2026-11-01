// Low-Level Concrete Classes
public class CreditCardProcessor {
    public void Process(decimal amount) => Console.WriteLine($"Charging ${amount} to Credit Card.");
}

public class PayPalProcessor {
    public void Process(decimal amount) => Console.WriteLine($"Charging ${amount} to PayPal.");
}

// High-Level Business Logic
public class PaymentService
{
    public void ExecutePayment(string paymentType, decimal amount)
    {
        // TIGHT COUPLING: This class directly depends on concrete classes using 'new'
        if (paymentType == "CreditCard")
        {
            var processor = new CreditCardProcessor(); 
            processor.Process(amount);
        }
        else if (paymentType == "PayPal")
        {
            var processor = new PayPalProcessor();
            processor.Process(amount);
        }
    }
}


//______________****************________________
// Better approach: Using Factory Pattern to decouple the PaymentService from concrete payment processors
public interface IPaymentProccessor // all payment types have to implement this
{
    void Process(decimal amount);
}

//concrete classes - all will implement the common interface
public class CreditCardProcessor : IPaymentProccessor
{
    public void Process(decimal amount) => System.Console.WriteLine($"Charging ${amount} via credit card");
}
public class Paypal : IPaymentProccessor
{
    public void Process(decimal amount) => System.Console.WriteLine($"Charging ${amount} via Paypal");
}

// Payment Factory - all implementation and new logic in 1 place
public static class PaymentProcessorFactory
{
    public static IPaymentProcessor CreateProcessor(string paymentType)
    {
        return paymentType.ToLower() switch
        {
            "creditcard" => new CreditCardProcessor(),
            "paypal"     => new PayPalProcessor(),
            _            => throw new ArgumentException("Unsupported payment type!")
        };
    }
}

//Loosely coupled execution now-
public class PaymentService
{
    public void ExecutePayment(string paymentType, decimal amount)
    {
        // LOOSE COUPLING: We depend strictly on the IPaymentProcessor abstraction.
        // We have no idea which concrete class is being created under the hood!
        IPaymentProcessor processor = PaymentProcessorFactory.CreateProcessor(paymentType);
        
        processor.Process(amount);
    }
}