using System.Globalization;
using System.Text;
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
        { "and",          TokenType.AND},
        { "or",           TokenType.OR},
        { "not",          TokenType.NOT},

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
        { "Integer", TokenType.INTEGER },
        { "Float", TokenType.FLOAT },
        { "Boolean", TokenType.BOOLEAN },
        { "Char", TokenType.CHAR },
        { "String", TokenType.STRING_TYPE },

        // Other
        { "say", TokenType.SAY }
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
            case '(':
                AddToken(TokenType.LEFT_PAREN);
                break;
            case ')':
                AddToken(TokenType.RIGHT_PAREN);
                break;
            case '{':
                AddToken(TokenType.LEFT_BRACE);
                break;
            case '}':
                AddToken(TokenType.RIGHT_BRACE);
                break;
            case ',':
                AddToken(TokenType.COMMA);
                break;
            case '.':
                AddToken(TokenType.DOT);
                break;
            case '-':
                AddToken(Match('=') ? TokenType.MINUS_EQUAL : TokenType.MINUS);
                break;
            case '+':
                AddToken(Match('=') ? TokenType.PLUS_EQUAL : TokenType.PLUS);
                break;
            case ';':
                AddToken(TokenType.SEMICOLON);
                break;
            case '[':
                AddToken(TokenType.LEFT_BRACKET);
                break;
            case ']':
                AddToken(TokenType.RIGHT_BRACKET);
                break;
            case '*':
                AddToken(Match('=') ? TokenType.STAR_EQUAL : TokenType.STAR);
                break;
            case '!':
                AddToken(Match('=') ? TokenType.BANG_EQUAL : TokenType.NOT);
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
            case '%':
                AddToken(Match('=') ? TokenType.MODULO_EQUAL : TokenType.MODULO);
                break;
            case '/':
                if(Match('/'))
                {
                    AddToken(Match('=') ? TokenType.SLASH_SLASH_EQUAL: TokenType.SLASH_SLASH);
                }
                else if (Match('*'))
                {
                    BlockComment();
                }
                else if (Match('='))
                {
                    AddToken(TokenType.SLASH_EQUAL);
                }
                else
                {
                    AddToken(TokenType.SLASH);
                }
                break;
            case '&':
                if (Match('&'))
                {
                    AddToken(TokenType.AND);
                }
                else
                {
                    Program.Error(_line, "Unexpected character.");
                }
                break;
            case '|':
                if (Match('|'))
                {
                    AddToken(TokenType.OR);
                }
                else
                {
                    Program.Error(_line, "Unexpected character.");
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
            case '\'': Character(); break;

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

        if (text == "annotate")
        {
            while (Peek() != '\n' && !IsAtEnd()) Advance();
            return; //discard, no token added
        }

        TokenType type = keywords.TryGetValue(text, out var keywordType)
            ? keywordType
            : TokenType.IDENTIFIER;

        AddToken(type);
    }

    private void Number()
    {
        while (IsDigit(Peek())) Advance();

        bool isFloat = false;

        //look for decimal dot
        if (Peek() == '.' && IsDigit(PeekNext()))
        {
            isFloat = true;
            Advance(); //Consume the "."

            while(IsDigit(Peek())) Advance();
        }

        string text = _source.Substring(_start, _current - _start);

        if (isFloat)
        {
            AddToken(TokenType.NUMBER, double.Parse(text, CultureInfo.InvariantCulture));
        } else
        {
            AddToken(TokenType.NUMBER, long.Parse(text, CultureInfo.InvariantCulture));
        }
    }

    private void String()
    {
        var value = new StringBuilder();

        while (Peek() != '"' && !IsAtEnd())
        {
            if (Peek() == '\\')
            {
                Advance(); // consume '\'
                if(IsAtEnd())
                {
                    Program.Error(_line, "Unterminated escape sequence.");
                    return;
                }

                char escape = Advance();

                switch(escape)
                {
                    case 'n': value.Append('\n'); break;
                    case 't': value.Append('\t'); break;
                    case 'r': value.Append('\r'); break;
                    case '"': value.Append('"'); break;
                    case '\\': value.Append('\\'); break;
                    default:
                        Program.Error(_line, "Invalid escape sequence.");
                        return;
                }
                continue;
            }

            if (Peek() == '\n') _line++;
            value.Append(Advance());
        }

        if(IsAtEnd())
        {
            Program.Error(_line, "Untermianted string.");
            return;
        }

       

        Advance(); //the closing ".

        AddToken(TokenType.STRING, value);
        
    }

    private void Character()
    {
        while(Peek() != '\'' && !IsAtEnd())
        {
            if (Peek() == '\n') _line++;
            Advance();
        }

        if (IsAtEnd())
        {
            Program.Error(_line, "Unterminated character.");
            return;
        }

        Advance(); //the closing '

        string value = _source.Substring(_start + 1, _current - _start - 2);

        if (value.Length != 1)
        {
            Program.Error(_line, "Character literal must contain exactly one character.");
            return;
        }

        AddToken(TokenType.CHARACTER, value[0]);
    }

    private void BlockComment()
    {
        while(!(Peek() == '*' && PeekNext() == '/') && !IsAtEnd())
        {
            if (Peek() == '\n') _line++;
            Advance();
        } 

        if (IsAtEnd())
        {
            Program.Error(_line, "Unterminated block comment.");
            return;
        }

        Advance(); // consume '*'
        Advance(); // consume '/'
        // no AddToken - comments are discarded, not tokenized
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


