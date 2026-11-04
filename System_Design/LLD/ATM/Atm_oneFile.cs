using System;
using System.Collections.Generic;
using System.Threading;

namespace ATMSystemSingleFile
{
    // This is a small single-file version of the ATM LLD.
    // Approach: keep the logic simple, but still model the real-world responsibilities:
    // - BankAccount handles money safety using locking
    // - Card and UPI are two authentication methods
    // - ATM decides the flow
    // - CashDispenser simulates physical cash dispensing

    // BankAccount stores balance and protects it from race conditions.
    // We use a lock so two threads cannot withdraw the same money at the same time.
    public class BankAccount
    {
        private readonly object sync = new object();
        private readonly string number;
        private double balance;

        public BankAccount(string number, double balance)
        {
            this.number = number;
            this.balance = balance;
        }

        public double Balance
        {
            get
            {
                lock (sync)
                {
                    return balance;
                }
            }
        }

        public bool Withdraw(double amount)
        {
            lock (sync)
            {
                if (balance < amount)
                {
                    return false;
                }

                Thread.Sleep(50);
                balance -= amount;
                return true;
            }
        }

        public void Deposit(double amount)
        {
            lock (sync)
            {
                balance += amount;
            }
        }
    }

    // Card represents a physical debit card used for ATM authentication.
    public class Card
    {
        public Card(string number)
        {
            Number = number;
        }

        public string Number { get; }
    }

    // CashDispenser models the physical machine that gives cash.
    // It checks whether the requested amount can be formed using available note denominations.
    public class CashDispenser
    {
        private readonly Dictionary<int, int> notes = new Dictionary<int, int>
        {
            { 500, 10 },
            { 100, 10 },
            { 50, 10 },
            { 20, 20 },
            { 10, 50 }
        };

        private readonly object sync = new object();

        public bool DispenseCash(int amount)
        {
            lock (sync)
            {
                if (amount % 10 != 0)
                {
                    Console.WriteLine("Amount must be a multiple of 10.");
                    return false;
                }

                int remaining = amount;
                var used = new Dictionary<int, int>();

                foreach (int denomination in new[] { 500, 100, 50, 20, 10 })
                {
                    int available = notes[denomination];
                    int count = Math.Min(remaining / denomination, available);

                    if (count > 0)
                    {
                        used[denomination] = count;
                        remaining -= count * denomination;
                    }
                }

                if (remaining != 0)
                {
                    Console.WriteLine("Exact denominations unavailable for ₹" + amount);
                    return false;
                }

                Console.WriteLine("--- Dispensing Physical Cash ---");
                foreach (var item in used)
                {
                    notes[item.Key] -= item.Value;
                    Console.WriteLine("  -> Dropping " + item.Value + " x ₹" + item.Key);
                }
                Console.WriteLine("--------------------------------");

                return true;
            }
        }
    }

    // BankService acts like the backend data source.
    // It stores account data, card mappings, PINs, and UPI mappings.
    public class BankService
    {
        private readonly Dictionary<string, BankAccount> accounts = new Dictionary<string, BankAccount>();
        private readonly Dictionary<string, string> cardToAccount = new Dictionary<string, string>();
        private readonly Dictionary<string, string> cardPins = new Dictionary<string, string>();
        private readonly Dictionary<string, string> upiToAccount = new Dictionary<string, string>();

        public BankService()
        {
            accounts["ACC-1001"] = new BankAccount("ACC-1001", 5000.00);
            cardToAccount["CARD-123"] = "ACC-1001";
            cardPins["CARD-123"] = "4321";
            upiToAccount["user@upi"] = "ACC-1001";
        }

        public bool VerifyCardPin(string cardNumber, string pin)
        {
            return cardPins.TryGetValue(cardNumber, out string expectedPin) && expectedPin == pin;
        }

        public BankAccount GetAccountByCard(string cardNumber)
        {
            if (cardToAccount.TryGetValue(cardNumber, out string accountNumber))
            {
                return accounts[accountNumber];
            }
            return null;
        }

        public bool VerifyUpi(string upiId)
        {
            return upiToAccount.ContainsKey(upiId);
        }

        public BankAccount GetAccountByUpi(string upiId)
        {
            if (upiToAccount.TryGetValue(upiId, out string accountNumber))
            {
                return accounts[accountNumber];
            }
            return null;
        }
    }

    // ATM is the main controller class.
    // It manages the current session, decides the auth method, and performs withdrawal flow.
    public class ATM
    {
        private readonly BankService bankService;
        private readonly CashDispenser cashDispenser;
        private BankAccount activeAccount;
        private string authMode;
        private string activeCardNumber;

        public ATM(BankService bankService, CashDispenser cashDispenser)
        {
            this.bankService = bankService;
            this.cashDispenser = cashDispenser;
        }

        public void StartCardTransaction(Card card)
        {
            authMode = "CARD";
            activeCardNumber = card.Number;
            activeAccount = bankService.GetAccountByCard(card.Number);

            Console.WriteLine("Card inserted. Please enter your PIN.");
        }

        public void StartUpiTransaction(string upiId)
        {
            authMode = "UPI";
            activeAccount = bankService.GetAccountByUpi(upiId);

            Console.WriteLine("Please scan the QR code using your UPI App.");
        }

        public bool Authenticate(string input)
        {
            if (authMode == "CARD")
            {
                bool ok = bankService.VerifyCardPin(activeCardNumber, input);
                Console.WriteLine(ok ? "PIN Accepted." : "Invalid PIN.");
                return ok;
            }

            if (authMode == "UPI")
            {
                bool ok = bankService.VerifyUpi(input) && bankService.GetAccountByUpi(input) != null;
                Console.WriteLine(ok ? "UPI Scan Successful." : "UPI Scan Failed/Timeout.");
                return ok;
            }

            return false;
        }

        public bool Withdraw(int amount)
        {
            if (activeAccount == null)
            {
                Console.WriteLine("No active account.");
                return false;
            }

            if (activeAccount.Balance < amount)
            {
                Console.WriteLine("Insufficient funds in bank account.");
                return false;
            }

            if (!activeAccount.Withdraw(amount))
            {
                Console.WriteLine("Digital transaction failed.");
                return false;
            }

            if (!cashDispenser.DispenseCash(amount))
            {
                activeAccount.Deposit(amount);
                Console.WriteLine("Dispense failed. Rolling back ₹" + amount + " to account.");
                return false;
            }

            Console.WriteLine("Transaction Complete. Remaining Balance: ₹" + activeAccount.Balance);
            return true;
        }
    }

    // Demo class shows how the ATM is used in real life:
    // 1) debit card
    // 2) UPI
    // 3) concurrent withdrawal test
    public static class ATMDemo
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("=== BOOTING UP ATM SYSTEM ===");

            BankService bankService = new BankService();
            CashDispenser cashDispenser = new CashDispenser();

            Console.WriteLine("\n=== SCENARIO 1: DEBIT CARD WITHDRAWAL ===");
            ATM atm1 = new ATM(bankService, cashDispenser);
            atm1.StartCardTransaction(new Card("CARD-123"));
            if (atm1.Authenticate("4321"))
            {
                atm1.Withdraw(1680);
            }

            Console.WriteLine("\n=== SCENARIO 2: UPI WITHDRAWAL ===");
            ATM atm2 = new ATM(bankService, cashDispenser);
            atm2.StartUpiTransaction("user@upi");
            if (atm2.Authenticate("user@upi"))
            {
                atm2.Withdraw(300);
            }

            Console.WriteLine("\n=== SCENARIO 3: CONCURRENCY TEST ===");
            Console.WriteLine("The account started with ₹5000.");
            Console.WriteLine("Two threads will try to withdraw ₹3000 each. Only one should succeed.");

            ThreadStart task = () =>
            {
                ATM tempAtm = new ATM(bankService, cashDispenser);
                tempAtm.StartUpiTransaction("user@upi");
                if (tempAtm.Authenticate("user@upi"))
                {
                    Console.WriteLine(Thread.CurrentThread.Name + " attempting ₹3000 withdrawal...");
                    tempAtm.Withdraw(3000);
                }
            };

            Thread t1 = new Thread(task) { Name = "[ATM Machine A]" };
            Thread t2 = new Thread(task) { Name = "[Online Auto-Pay]" };

            t1.Start();
            t2.Start();
            t1.Join();
            t2.Join();

            Console.WriteLine("\nFinal Account Balance: ₹" + bankService.GetAccountByCard("CARD-123").Balance);
        }
    }
}
