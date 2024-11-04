using System;
using Nethereum.Signer;
using Nethereum.Util;

namespace BNB_WalletGenerator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string targetPrefix = "target_string_here"; // target wallet address string after '0x'
            GenerateVanityBnbAddress(targetPrefix);
        }

        static void GenerateVanityBnbAddress(string targetPrefix)
        {
            int attempts = 0;
            string address = string.Empty;
            string privateKeyHex = string.Empty;

            // This continues generating addresses until one matches the target prefix
            do
            {
                attempts++;

                // Generate a new private key
                var ecKey = EthECKey.GenerateKey();
                privateKeyHex = ecKey.GetPrivateKey();
                address = ecKey.GetPublicAddress();

            } while (!address.StartsWith("0x" + targetPrefix, StringComparison.OrdinalIgnoreCase));

            // Display the results (will add multi-threading later)
            Console.WriteLine("Match found!");
            Console.WriteLine("Attempts: " + attempts);
            Console.WriteLine("Private Key: " + privateKeyHex);
            Console.WriteLine("BNB Smart Chain Address: " + address);

            // Keep the console open please!!!!!!
            Console.ReadLine();
        }
    }
}
