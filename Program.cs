using System;
using System.IO;
using System.Collections.Generic;
using Ileto.Debug;

namespace Ileto;

public class Program
{
    static bool HadError = false;
    static void Main(string[] args)
    {
        if (args.Length == 2 && args[0] == "--tokenize")
        {
            TokenizeFile(args[1]);
        }
        else if (args.Length > 1)
        {
            Console.WriteLine("Usage: Ileto [script]");
            Environment.Exit(64);
        } 
        else if (args.Length == 1)
        {
            RunFile(args[0]);
        }
        else
        {
            Console.WriteLine("Initiating REPL...");
            RunPrompt();
        }
    }

    // separate tokenize into its own method to preserve functionality
    // even as RunFile is expanded upon
    static void TokenizeFile(string path)
    {
        string source = File.ReadAllText(path);
        TokenPrinter printer = new TokenPrinter();

        printer.PrintTokens(Tokenize(source));
        if (HadError) Environment.Exit(65);
    }


    // called by default when there are no flags passed in
    // actually *does the things the code is supposed to do*
    static void RunFile(string path)
    {
        string source = File.ReadAllText(path);
        Run(source);
        if (HadError) Environment.Exit(65);
    }


    // called during the 'prompting' part of the REPL mode
    // simply reads lines and runs them
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


    // runs a string from start to finish; includes all
    // the cogs working together (scanner, parser, ...)
    private static void Run(string source)
    {
        Scanner scanner = new Scanner(source);
        List<Token> tokens = scanner.ScanTokens();

        TokenPrinter printer = new TokenPrinter();
        printer.PrintTokens(tokens);
    }


    // converts a string source into a stream of tokens
    // and then returns it (--tokenize flag)
    private static List<Token> Tokenize(string source)
    {
        Scanner scanner = new Scanner(source);
        List<Token> tokens = scanner.ScanTokens();
        return tokens;
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

