using Ileto;

namespace Ileto.Debug;

public class TokenPrinter
{
    public void PrintTokens(List<Token> tokens)
    {
        foreach (Token t in tokens)
        {
            var stringified = StringifyToken(t);
            Console.Write(stringified);
            Console.Write("\n");
        }
    }

    public string StringifyToken(Token t)
    {
        var literal = t.literal;
        if (literal == null) literal = "n/a";

        return $"line {t.line} : Found {t.type} [lexeme: {t.lexeme}, literal: {literal}]";
    }
}