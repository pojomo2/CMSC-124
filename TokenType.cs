public enum TokenType
{
    // Single-character tokens.
    LEFT_PAREN, RIGHT_PAREN, LEFT_BRACE, RIGHT_BRACE,
    COMMA, DOT, MINUS, PLUS, SEMICOLON, SLASH, STAR, PERCENT,

    // One or two character tokens.
    BANG, BANG_EQUAL,
    EQUAL, EQUAL_EQUAL,
    GREATER, GREATER_EQUAL,
    LESS, LESS_EQUAL,
    AMP_AMP, PIPE_PIPE,          // && , ||
    SLASH_SLASH,                 // // (integer division)

    // Compound assignment operators.
    PLUS_EQUAL, MINUS_EQUAL, STAR_EQUAL, SLASH_EQUAL,
    SLASH_SLASH_EQUAL, PERCENT_EQUAL,

    // Literals.
    IDENTIFIER, STRING, NUMBER, CHARACTER,

    // Keywords — special values.
    TRUE, FALSE, NIL,

    // Keywords — variables, functions, classes.
    VAR, CONST, FUNC, RETURN, CLASS,

    // Keywords — logic and membership.
    IN, IS, INHERITS,

    // Keywords — control flow and loops.
    WHILE, FOR, FOREACH, BREAK, CONTINUE, IF, YET, ELSE,

    // Keywords — event-related.
    PROCLAMATION, PROCLAIM, UPON, OBSERVE,

    EOF
}