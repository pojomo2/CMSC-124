using System;
using System.IO;
using System.Collections.Generic;
using Ileto.Debug;
using Ileto.Took;

namespace Ileto;

public class Program
{
    public class Program
    {
        static bool HadError = false;
        static void Main(string[] args)
        {
            if (args.Length == 0 )
                {
                    RunPrompt();
                }
                else if (args.Length == 2 && args[0] == "--tokenize")
                {
                    RunTokenizeFile(args[1]);
                }
                else if (args.Length == 2 && args[0] == "--parse")
                {
                    RunParseFile(args[1]);
                }        
                else
                {
                    Console.Error.WriteLine("Usage: Ileto [--tokenize | --parse] <file>");
                    Environment.Exit(64);
                }
        }

        static void RunTokenizeFile(string path)
        {
            string source = File.ReadAllText(path);
            var tokens = new Scanner(source).ScanTokens();

            foreach(Token token in tokens)
                Console.WriteLine(token);

        }

        static void RunParseFile(string path)
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

            if (HadError) return;

            Parser parser = new Parser(tokens);
            Expr? expression = parser.Parse();

            // stop if there was a syntax error
            if (HadError || expression is null) return;


            Console.WriteLine(new AstPrinter().Print(expression));
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

        public static void Error(Token token, string message)
        {
            if (token.type == TokenType.EOF)
            {
                Report(token.line, " at end", message);
            } else
            {
                Report(token.line, " at '" + token.lexeme + "'", message);
            }
        }
    }
    
}

