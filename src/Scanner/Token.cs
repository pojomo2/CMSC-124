namespace Ileto;


public enum TokenType
{
	// Literals
	IDENTIFIER,

	// Only single characters
	LEFT_PAREN,
	RIGHT_PAREN,
	PLUS,
	MINUS,
	STAR,
	SLASH,

	// Single/double characters
	EQUAL, EQUAL_EQUAL,
	BANG, BANG_EQUAL,

	// Keywords
	VAR,

	// End-of-file
	EOF
}


public record Location(int Line, int Column, int Length);

public class Token
{
	// token information {o, 3}
	public required TokenType Type { get; init; }
	public required string Lexeme { get; init; }
	public object? Literal { get; init; }

	// error handling info {g, 1}
	public required Location Location { get; init; }


	public Token()
	{
		// properties are init with braces notation
	}
}