using System;
using System.Threading;
using System.Threading.Tasks;
using Nethereum.Signer;
using Nethereum.Util;

namespace BNB_WalletGenerator
{
    internal class Program
    {
        private static volatile bool isMatchFound = false; // Shared flag for stopping threads
        private static readonly object consoleLock = new object(); // Lock for thread-safe console output
        private static int attempts = 0; // Track the number of attempts

        static void Main(string[] args)
        {
            string targetPrefix = "deadc"; // Target wallet address string after '0x'

            // Start a timer to display the number of attempts every second
            Timer timer = new Timer(DisplayAttempts, null, 0, 1000);

            GenerateVanityBnbAddress(targetPrefix);
            Console.ReadLine(); // Keep the console open
        }

        // Method to display attempts every second
        private static void DisplayAttempts(object state)
        {
            lock (consoleLock)
            {
                // Move the cursor to the beginning of the line
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write($"Wallets checked: {attempts}");
            }
        }

        static void GenerateVanityBnbAddress(string targetPrefix)
        {
            string foundPrivateKey = string.Empty;
            string foundAddress = string.Empty;

            // Use multiple tasks for parallel generation
            Parallel.For(0, Environment.ProcessorCount, (i, state) =>
            {
                while (!isMatchFound)
                {
                    // Generate a new private key and address
                    var ecKey = EthECKey.GenerateKey();
                    var privateKeyHex = ecKey.GetPrivateKey();
                    var address = ecKey.GetPublicAddress();

                    // Check if the address matches the target prefix
                    if (address.StartsWith("0x" + targetPrefix, StringComparison.OrdinalIgnoreCase))
                    {
                        isMatchFound = true; // Set the flag to stop other threads
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

                        // Stop other tasks
                        state.Stop();
                    }

                    // Increment attempts in a thread-safe manner
                    Interlocked.Increment(ref attempts);
                }
            });
        }
    }
}
