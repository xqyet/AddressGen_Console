# BNB Wallet Generator

A simple C# console application to generate BNB Smart Chain wallet addresses, with the ability to find a "vanity address" containing a specific string after the `0x` prefix.

## About

This application generates BNB Smart Chain addresses and allows you to search for a "vanity address," where the beginning of the address matches a specific prefix. This can be useful if you want an address with a custom pattern for aesthetic or branding purposes.

## Getting Started

### Prerequisites

- **.NET Framework**: Make sure you have the .NET Framework installed on your machine. You can download it from [Microsoft's .NET Download page](https://dotnet.microsoft.com/download).
- **Nethereum Libraries**: This project uses the Nethereum library for Ethereum-compatible address generation.

### Installation

1. Clone this repository to your local machine:
   ```bash
   git clone https://github.com/yourusername/BNB_WalletGenerator.git
   cd BNB_WalletGenerator
'''
2. Open the project in Visual Studio.

3. Install the required **Nethereum** packages via NuGet:
   - `Nethereum.Signer`
   - `Nethereum.Util`

   You can install these packages through the **NuGet Package Manager** in Visual Studio or by using the Package Manager Console:
   ```powershell
   Install-Package Nethereum.Signer
   Install-Package Nethereum.Util
'''
## Usage

1. Open the `Program.cs` file.
2. Edit the `targetPrefix` variable in `Main` to specify the prefix you want after `0x`. For example, if you want an address starting with `0xdA`, set `targetPrefix` as `"dA"`.
   ```csharp
   string targetPrefix = "dA"; // Target string after '0x'
'''
3. Run the application:
   - **In Visual Studio**: Press `F5` or go to **Debug > Start Debugging**.
   - **From Command Line**: Navigate to the project directory and use `dotnet run`.

The program will continue to generate addresses until it finds one that matches the specified prefix.

### Important Note

Generating a specific vanity prefix can take time. The longer the prefix, the more attempts may be needed. For an 8-character prefix, it could take a considerable amount of time to find a match.

## Example Output

After running the program, you should see output similar to the following once a match is found: (example output)

```plaintext
Match found!
Attempts: 14235
Private Key: 0xbd8f582bb673ba06e3420858706e07a8bf600f288550c9ed508928840ad6301e
BNB Smart Chain Address: 0xdACA990A5F4F29259D2D4ED6D71B5F63430AEF57
```
For more projects and information, visit my website: [xqyet.dev](https://xqyet.dev)

