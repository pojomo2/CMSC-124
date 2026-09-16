namespace Ileto;

public class Scanner
{
	// _line: tracks current row being scanned
	// _start: index of the current lexeme
	// _curr: index of the one being scanned 	
	private int _line;
	private int _start;
	private int _curr;

	private string _source;
	private List<Token> _tokens = new();

	public Scanner(string source)
	{
		_line = 0;
		_start = 0;
		_curr = 0;
		_source = source;
	}
	
	public List<Token> ScanTokens()
	{
		while (!IsAtEnd())
		{
			_start = _curr;
			ScanToken();
		}

		// add EOF after all the tokens are scanned
		// _tokens.Add(new() { 
		// 	Type = TokenType.EOF, 
		// 	Lexeme = "", 
		// 	Location = new Location(_line, _start, _curr - _start)
		// });

		return _tokens;
	}

	private bool IsAtEnd() 
	{
		return _curr >= _source.Length;
	}

	private void ScanToken()
	{
		var next_char = Advance();
		switch (next_char)
		{
			case '(': AddToken(TokenType.LEFT_PAREN); break;
			case ')': AddToken(TokenType.RIGHT_PAREN); break;
			case '+': AddToken(TokenType.PLUS); break;
			default: AddToken(TokenType.EOF); break;
		}
	}

	private char Advance()
	{
		_curr++;
		return _source[_curr - 1];
	}

	private void AddToken(TokenType type)
	{
		AddToken(type, null);
	}

	private void AddToken(TokenType type, object? literal)
	{
		var text = _source.Substring(_start, _curr);
		var location = new Location(_line, _start, _curr - _start);
		_tokens.Add(new Token()
		{
			Type = type,
			Lexeme = text,
			Literal = literal,
			Location = location
		});
	}
}