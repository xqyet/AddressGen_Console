using System;
using System.Threading.Tasks;
using Nethereum.Signer;
using Nethereum.Util;

namespace BNB_WalletGenerator
{
    internal class Program
    {
        private static volatile bool isMatchFound = false; // Shared flag for stopping threads
        private static readonly object consoleLock = new object(); // Lock for thread-safe console output

        static void Main(string[] args)
        {
            string targetPrefix = "dAC"; // Target wallet address string after '0x'
            GenerateVanityBnbAddress(targetPrefix);
            Console.ReadLine(); // Keep the console open
        }

        static void GenerateVanityBnbAddress(string targetPrefix)
        {
            int attempts = 0;
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
                            Console.WriteLine("Match found!");
                            Console.WriteLine("Attempts: " + attempts);
                            Console.WriteLine("Private Key: " + foundPrivateKey);
                            Console.WriteLine("BNB Smart Chain Address: " + foundAddress);
                        }

                        // Stop other tasks
                        state.Stop();
                    }

                    attempts++; // Increment attempts
                }
            });
        }
    }
}
