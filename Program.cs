using System;
using Ileto;
using Ileto.Debug;

/* {white}
Program.cs acts as the interpreter's entry point. 
The main pipeline implementation is written here.
*/


Console.WriteLine("Hello, World!");

if (args.Length > 1)
{
	Console.WriteLine("Usage: ./run [filename]. Exiting...");
	System.Environment.Exit(65);
}
else if (args.Length == 1)
{
	Console.WriteLine($"Reading file at: {args[0]}");
	System.Environment.Exit(0);
}
else
{
	// run CLI code editor
	TokenPrinter tokenPrinter = new TokenPrinter();

	for (;;)
	{
		Console.Write(">> ");
		string? codeLine = Console.ReadLine();

		if (codeLine == null)
		{
			Console.WriteLine("\nEOF detected. Exiting...");
			break;
		}

		Scanner newScanner = new Scanner(codeLine);
		List<Token> tokens = newScanner.ScanTokens();
		tokenPrinter.PrintTokens(tokens);
	}
}
