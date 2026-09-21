using Ileto;
namespace Ileto;



public class Scanner
{

    private readonly string _source;
    private readonly List<Token> _tokens = new List<Token>();
    private int _start = 0;
    private int _current = 0;
    private int _line = 1;


    private static readonly Dictionary<string, TokenType> keywords = new()
    {
        // Special values
        { "Yea",          TokenType.TRUE },
        { "Nay",          TokenType.FALSE },
        { "Nil",          TokenType.NIL },

        // Variables, functions, classes
        { "var",          TokenType.VAR },
        { "const",        TokenType.CONST },
        { "func",         TokenType.FUNC },
        { "requite",      TokenType.RETURN },
        { "class",        TokenType.CLASS },

        // Logic and membership
        { "in",           TokenType.IN },
        { "is",           TokenType.IS },
        { "inherits",     TokenType.INHERITS },

        // Control flow and loops
        { "whilst",       TokenType.WHILE },
        { "for",          TokenType.FOR },
        { "amongst",      TokenType.FOREACH },
        { "desist",       TokenType.BREAK },
        { "proceed",      TokenType.CONTINUE },
        { "provided",     TokenType.IF },
        { "yet",          TokenType.YET },        // "yet provided" = else-if, parser combines these
        { "otherwise",    TokenType.ELSE },

        // Event-related
        { "proclamation", TokenType.PROCLAMATION },
        { "proclaim",     TokenType.PROCLAIM },
        { "upon",         TokenType.UPON },
        { "observe",      TokenType.OBSERVE },
    };


    public Scanner(string source)
    {
        _source = source;
    }

    public List<Token> ScanTokens()
    {
        while(!IsAtEnd())
        {
            //we are at the beginning of the next lexeme
            _start = _current;
            ScanToken();
        }

        _tokens.Add(new Token(TokenType.EOF, "", null, _line));
        return _tokens;
    }

    private void ScanToken()
    {
        char c = Advance();
        switch(c)
        {
            case '(': AddToken(TokenType.LEFT_PAREN); break;
            case ')': AddToken(TokenType.RIGHT_PAREN); break;
            case '{': AddToken(TokenType.LEFT_BRACE); break;
            case '}': AddToken(TokenType.RIGHT_BRACE); break;
            case ',': AddToken(TokenType.COMMA); break;
            case '.': AddToken(TokenType.DOT); break;
            case '-': AddToken(TokenType.MINUS); break;
            case '+': AddToken(TokenType.PLUS); break;
            case ';': AddToken(TokenType.SEMICOLON); break;
            case '*': AddToken(TokenType.STAR); break;
            case '!':
                AddToken(Match('=') ? TokenType.BANG_EQUAL: TokenType.BANG);
                break;
            case '=':
                AddToken(Match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL);
                break;
            case '<':
                AddToken(Match('=') ? TokenType.LESS_EQUAL : TokenType.LESS);
                break;
            case '>':
                AddToken(Match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER);
                break;
            case '/':
                if(Match('/'))
                {
                     while (Peek() != '\n' && !IsAtEnd()) Advance();
                } else
                {
                    AddToken(TokenType.SLASH);
                }
                break;

            case ' ':
            case '\r':
            case '\t':
                //ignore white space
                break;
            case '\n':
                _line++;
                break;

            case '"': String(); break;

            default:
                if (IsDigit(c))
                {
                    Number();
                } 
                else if (IsAlpha(c))
                {
                    Identifier();
                }
                else
                {
                    Program.Error(_line, "Unexpected character.");
                }
                break;


     
        }
    }

    private void Identifier()
    {
        while (IsAlphaNumeric(Peek())) Advance();

        String text = _source[_start.._current];
        TokenType type = keywords.TryGetValue(text, out var keywordType)
            ? keywordType
            : TokenType.IDENTIFIER;

        AddToken(type);
    }

    private void Number()
    {
        while (IsDigit(Peek())) Advance();

        //look for decimal dot
        if (Peek() == '.' && IsDigit(PeekNext()))
        {
            //Consume the "."
            Advance();

            while(IsDigit(Peek())) Advance();
        }

        AddToken(TokenType.NUMBER, Double.Parse(_source.Substring(_start, _current - _start)));
    }

    private void String()
    {
        while (Peek() != '"' && !IsAtEnd())
        {
            if (Peek() == '\n') _line++;
            Advance();
        }

        if(IsAtEnd())
        {
            Program.Error(_line, "Untermianted string.");
            return;
        }

       

        Advance(); //the closing ".

        String value = _source.Substring(_start + 1, _current - _start - 2);
        AddToken(TokenType.STRING, value);
        
    }


    private bool Match(Char expected)
    {
        if(IsAtEnd()) return false;
        if(_source[_current] != expected) return false;

        _current++;
        return true;
    }

    private Char Peek()
    {
        if (IsAtEnd()) return '\0';
        return _source[_current];
    }

    private Char PeekNext()
    {
        if(_current + 1 >= _source.Length) return '\0';
        return _source[_current + 1];

    }

    private bool IsAlpha(Char c)
    {
        return (c >= 'a' && c <= 'z') || 
               (c >= 'A' && c <= 'Z') ||
               c == '_';
    }

    private bool IsAlphaNumeric(Char c)
    {
        return IsAlpha(c) || IsDigit(c);
    }

    private bool IsDigit(Char c)
    {
        return c >= '0' && c <= '9';
    }



    private Boolean IsAtEnd()
    {
        return _current >= _source.Length;
    }

    private char Advance() 
    {
        _current++;
        return _source[_current - 1];
    }

    private void AddToken(TokenType type)
    {
        AddToken(type, null);
    }

    private void AddToken(TokenType type, Object? literal)
    {
        String text = _source.Substring(_start, _current - _start);
        _tokens.Add(new Token(type, text, literal, _line));

    }
}


