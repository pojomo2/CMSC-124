using System.Collections.Generic;
using System;
using System.IO;
using Ileto;
using Ileto.Debug;

using System.Runtime.InteropServices;
using System.Collections;
using System.Diagnostics;


public class Scanner
{

    private readonly string _source;
    private readonly List<Token> _tokens = new List<Token>();
    private int _start = 0;
    private int _current = 0;
    private int _line = 1;
    static Boolean hadError = false;



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
        }
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

    private void AddToken(TokenType type, Object literal)
    {
        String text = _source.Substring(_start, _current);
        _tokens.Add(new Token(type, text, literal, _line));

    }
}


