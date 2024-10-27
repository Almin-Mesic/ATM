using System;

namespace ATMApp
{
    class ATM
    {
        private static decimal balance = 1000m; // Initial balance
        private static string correctPin = "1234"; // Simulated PIN
        private static int pinAttempts = 0;
        private static int maxPinAttempts = 3;
        private static int menuInvalidAttempts = 0; // Tracks invalid menu choices
        private static int maxMenuInvalidAttempts = 5;

        public static void Main(string[] args)
        {
            // Start ATM process
            if (!Authenticate())
            {
                Console.WriteLine("You have lost your card.");
                return;
            }

            // Show ATM menu
            MainMenu();
        }

        // Authenticate user by PIN
        private static bool Authenticate()
        {
            while (pinAttempts < maxPinAttempts)
            {
                Console.WriteLine("Please enter your PIN:");
                string enteredPin = Console.ReadLine();

                if (enteredPin == correctPin)
                {
                    return true;
                }

                pinAttempts++;
                Console.WriteLine($"Incorrect PIN. You have {maxPinAttempts - pinAttempts} attempt(s) left.");
            }

            return false; // Fail to authenticate
        }

        // Main ATM menu after PIN authentication
        private static void MainMenu()
        {
            while (true)
            {
                Console.WriteLine("\nATM Menu:");
                Console.WriteLine("1. Check Balance");
                Console.WriteLine("2. Withdraw Money");
                Console.WriteLine("Enter your choice (1 or 2):");

                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    CheckBalance();
                }
                else if (choice == "2")
                {
                    WithdrawMoney();
                }
                else
                {
                    menuInvalidAttempts++;
                    Console.WriteLine("Invalid choice, please try again.");

                    // Check if invalid menu choices exceed limit
                    if (menuInvalidAttempts >= maxMenuInvalidAttempts)
                    {
                        Console.WriteLine("You have entered too many invalid choices. Thank you for using ATM. Goodbye.");
                        Environment.Exit(0); // Exit the program
                    }
                }
            }
        }

        // Check balance
        private static void CheckBalance()
        {
            Console.WriteLine($"Your current balance is: {balance:C}");
        }

        // Withdraw money
        private static void WithdrawMoney()
        {
            int attempts = 0;
            bool successfulWithdrawal = false;

            while (attempts < 3 && !successfulWithdrawal)
            {
                Console.WriteLine("\nSelect the amount you want to withdraw:");
                Console.WriteLine("1. $20");
                Console.WriteLine("2. $40");
                Console.WriteLine("3. $80");
                Console.WriteLine("4. $200");
                Console.WriteLine("or 5. Enter custom amount");
                Console.WriteLine("Enter your choice:");

                string choice = Console.ReadLine();
                decimal withdrawalAmount = 0m;

                switch (choice)
                {
                    case "1":
                        withdrawalAmount = 20m;
                        break;
                    case "2":
                        withdrawalAmount = 40m;
                        break;
                    case "3":
                        withdrawalAmount = 80m;
                        break;
                    case "4":
                        withdrawalAmount = 200m;
                        break;
                    case "5":
                        Console.WriteLine("Enter the amount you want to withdraw:");
                        if (!decimal.TryParse(Console.ReadLine(), out withdrawalAmount))
                        {
                            Console.WriteLine("Invalid amount, please try again.");
                            continue;
                        }
                        break;
                    default:
                        Console.WriteLine("Invalid choice, please try again.");
                        continue;
                }

                // Confirm withdrawal amount
                if (!ConfirmWithdrawal(withdrawalAmount))
                {
                    continue; // If not confirmed, go back to withdrawal menu
                }

                // Check if the amount is valid
                if (withdrawalAmount > balance)
                {
                    Console.WriteLine("You do not have enough balance.");

                    // Ask if they want to check their current balance
                    if (AskToSeeBalanceAfterInsufficientFunds())
                    {
                        CheckBalance();
                    }

                    attempts++;
                }
                else
                {
                    balance -= withdrawalAmount;
                    Console.WriteLine($"You have successfully withdrawn {withdrawalAmount:C}. Your new balance is {balance:C}");
                    successfulWithdrawal = true;

                    // Ask if they want to return to the ATM menu
                    if (AskToRestartATMMenu())
                    {
                        MainMenu();
                    }
                    else
                    {
                        Console.WriteLine("Thank you for using the ATM. Goodbye!");
                        Environment.Exit(0); // Exit the program 
                    }
                }
            }

            if (!successfulWithdrawal)
            {
                Console.WriteLine("You have exceeded the maximum number of attempts. Goodbye.");
                Environment.Exit(0); // Exit the program after failed attempts
            }
        }

        // Confirm withdrawal amount
        private static bool ConfirmWithdrawal(decimal withdrawalAmount)
        {
            Console.WriteLine($"You have chosen to withdraw {withdrawalAmount:C}. Are you sure? (yes/no):");
            string response = Console.ReadLine().ToLower();

            return response == "yes";
        }

        // Ask if user wants to see balance after insufficient funds
        private static bool AskToSeeBalanceAfterInsufficientFunds()
        {
            while (true)
            {
                Console.WriteLine("Would you like to see your current balance? (yes/no):");
                string response = Console.ReadLine().ToLower();

                if (response == "yes")
                {
                    return true;
                }
                else if (response == "no")
                {
                    return false;
                }
                else
                {
                    Console.WriteLine("Invalid response, please try again.");
                }
            }
        }

        // Ask if user wants to restart from the ATM menu
        private static bool AskToRestartATMMenu()
        {
            while (true)
            {
                Console.WriteLine("Would you like to return to the ATM menu? (yes/no):");
                string response = Console.ReadLine().ToLower();

                if (response == "yes")
                {
                    return true;
                }
                else if (response == "no")
                {
                    return false;
                }
                else
                {
                    Console.WriteLine("Invalid response, please try again.");
                }
            }
        }
    }
}
