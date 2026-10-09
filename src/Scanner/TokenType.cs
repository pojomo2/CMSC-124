namespace Ileto;

public enum TokenType
{
    // Single-character tokens.
    LEFT_PAREN, RIGHT_PAREN, LEFT_BRACE, RIGHT_BRACE,
    COMMA, DOT, MINUS, PLUS, SEMICOLON, SLASH, STAR, PERCENT, MODULO,

    // One or two character tokens.
    BANG, BANG_EQUAL,
    EQUAL, EQUAL_EQUAL,
    GREATER, GREATER_EQUAL,
    LESS, LESS_EQUAL,
    AMP_AMP, PIPE_PIPE,          // && , ||
    SLASH_SLASH,                 // // (integer division)
    MODULO_EQUAL, // %=

    // Compound assignment operators.
    PLUS_EQUAL, MINUS_EQUAL, STAR_EQUAL, SLASH_EQUAL,
    SLASH_SLASH_EQUAL, PERCENT_EQUAL,

    // Literals.
    IDENTIFIER, STRING, NUMBER, CHARACTER, LEFT_BRACKET, RIGHT_BRACKET,

    // Keywords — special values.
    TRUE, FALSE, NIL, AND, OR, NOT,

    // Keywords — variables, functions, classes.
    VAR, CONST, FUNC, RETURN,

    // Keywords — logic and membership.
    IN, IS,

    // Keywords — control flow and loops.
    WHILE, FOR, FOREACH, BREAK, CONTINUE, IF, YET, ELSE,

    // Keywords — event-related.
    PROCLAMATION, PROCLAIM, UPON, OBSERVE,

    // Keywords — other.
    SAY,

    // Keywords — built-in type names used by the `is` operator.
    INTEGER, FLOAT, BOOLEAN, CHAR, STRING_TYPE,

    EOF
}