using System;
using System.Threading;
using System.Threading.Tasks;
using Nethereum.Signer;
using Nethereum.Util;

namespace BNB_WalletGenerator
{
    internal class Program
    {
        private static volatile bool isMatchFound = false; // Shared flag for stopping tasks
        private static readonly object consoleLock = new object(); // Lock for thread-safe console output
        private static int attempts = 0; // Track the number of attempts

        static void Main(string[] args)
        {
            string targetPrefix = "deadc4"; // Target wallet address string after '0x'

            // Start a timer to display the number of attempts every 5 seconds
            Timer timer = new Timer(DisplayAttempts, null, 0, 5000);

            // Create and start a fixed number of tasks to maximize parallelism without overhead
            int taskCount = Environment.ProcessorCount * 2; // Adjust this value as needed
            Task[] tasks = new Task[taskCount];
            for (int i = 0; i < taskCount; i++)
            {
                tasks[i] = Task.Run(() => GenerateVanityBnbAddress(targetPrefix));
            }

            // Wait for all tasks to complete (one will stop others upon finding a match)
            Task.WaitAll(tasks);
            Console.ReadLine(); // Keep the console open
        }

        // Method to display attempts every 5 seconds
        private static void DisplayAttempts(object state)
        {
            lock (consoleLock)
            {
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write($"Wallets checked: {attempts}");
            }
        }

        static void GenerateVanityBnbAddress(string targetPrefix)
        {
            string foundPrivateKey = string.Empty;
            string foundAddress = string.Empty;

            while (!isMatchFound)
            {
                // Generate a new private key and address
                var ecKey = EthECKey.GenerateKey();
                var privateKeyHex = ecKey.GetPrivateKey();
                var address = ecKey.GetPublicAddress();

                // Check if the address matches the target prefix
                if (address.StartsWith("0x" + targetPrefix, StringComparison.OrdinalIgnoreCase))
                {
                    isMatchFound = true; // Set the flag to stop other tasks
                    foundPrivateKey = privateKeyHex;
                    foundAddress = address;

                    // Lock console output to prevent overlaps
                    lock (consoleLock)
                    {
                        Console.WriteLine();
                        Console.WriteLine("Match found!");
                        Console.WriteLine("Total Attempts: " + attempts);
                        Console.WriteLine("Private Key: " + foundPrivateKey);
                        Console.WriteLine("BNB Smart Chain Address: " + foundAddress);
                    }

                    return; // Exit the method once a match is found
                }

                // Increment attempts in a thread-safe manner
                Interlocked.Increment(ref attempts);
            }
        }
    }
}
