namespace Ileto.Debug;


public class TokenPrinter
{
	public void PrintTokens(List<Token> tokens)
	{
		foreach (Token t in tokens)
		{
			var stringified = StringifyToken(t);
			Console.WriteLine(stringified);
		}
	}

	public string StringifyToken(Token t)
	{		
		var literal = t.Literal;
		if (literal == null) literal = "n/a";

		return $"{t.Location.Line}, {t.Location.Column}\t: Found {t.Type} [lexeme: {t.Lexeme}, literal: {literal}]";
	}
}