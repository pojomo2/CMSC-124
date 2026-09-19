using System;
using System.IO;
using System.Collections.Generic;

namespace Ileto
{
    public class Program
    {
    static bool HadError = false;
    static void Main(string[] args)
    {
        if (args.Length > 1)
        {
            Console.WriteLine("Usage: Ileto [script]");
            Environment.Exit(64);
        }
        else if (args.Length == 1)
        {
            //RunFile(args[0]);
            Console.WriteLine("Hello, world!");
        }
        else
        {
            RunPrompt();
        }
    }

    static void RunFile(string path)
    {
        string source = File.ReadAllText(path);
        Run(source);

        if (HadError)
            Environment.Exit(65);
    }

    static void RunPrompt()
    {
        for (;;)
        {
            Console.Write("> ");
            string? line = Console.ReadLine();
            if (line == null) break;
            Run(line);
            HadError = false;
        }
    }

    private static void Run(string source)
    {
        Scanner scanner = new Scanner(source);
        List<Token> tokens = scanner.ScanTokens();

        foreach (Token token in tokens)
        {
            Console.WriteLine(token);
        }
    }

    public static void Error(int line, string message)
    {
        Report(line, "", message);
    }

    private static void Report(int line, string where, string message)
    {
        Console.Error.WriteLine("[line " + line + "] Error" + where + ": " + message);
        HadError = true;
    }
    }
}

