# ============================================================================= #
#                          THE ILETO PROJECT: Overview                          #
# ============================================================================= #

## Creators
  - Drew T. Cudiamat 		(pojomo2)
  - Jared Ramyll D. Chua 	(ultramarine23)


## Overview 
  The Ileto language is a high-level, event-driven language with a readability-
  first design philosophy. It is designed for a different take on traditional
  OOP, with a more intuitive event-centric design that removes the boilerplate
  associated with events in other languages.


## Host language and build
  - Host language: C#
  - Version metadata: dotnet 10.0.400
  - Build: `./build.sh`


## Running it
| Command 					| What it does										|
|---------------------------|---------------------------------------------------|
| `./run <file>` 			| [Executes a program. Available from Lab 4.] 		|
| `./run --tokenize <file>` | [Prints the token stream.] 						|
| `./run --parse <file>` 	| [Prints the parsed tree.] 						|
| `./run --eval <file>` 	| [Evaluates each expression and prints its value.] |
| `./run` 					| [Starts the REPL.]								|

Exit codes: 
  0		: file scanned/ran cleanly, no errors
  65	: scanner rejected the file before running any of it (e.g. unterminated string, unexpected character)
  70	: not applicable yet — reserved for runtime errors, starting Lab 3


## File extension
  Ileto files are declared with the `.teic` file extension.


# ============================================================================= #
#                    THE ILETO PROJECT: Lexical Information                     #
# ============================================================================= #

### Keywords
|---------------------------|---------------------------------------------------|
| Keyword 					| What it does										|
|---------------------------|---------------------------------------------------|
| perhaps 					| variable declaration								|
|                           | ex. perhaps int foo = 5;							|
|---------------------------|---------------------------------------------------|
| proclaim 					| constant declaration								|
|                           | ex. proclaim int FOO = 5;							|
|---------------------------|---------------------------------------------------|
| provided					| if block declaration								|
|---------------------------|---------------------------------------------------|
| alternatively				| else if block declaration							|
|---------------------------|---------------------------------------------------|
| otherwise					| else block declaration							|
|                           | ex. provided (foo == 5) { 						|
|                           | 	      foo += 3;									|
|                           | 	  } alternatively (foo == 4) { 					|
|                           |         foo += 2;									|
|                           | 	  } otherwise { 								|
|                           | 	      foo -= 3;									|
|                           | 	  } 											|
|---------------------------|---------------------------------------------------|
| whilst 					| while loop block declaration						|
|                           | ex. whilst (i < 5) { 								|
|                           | 	      i++;										|
|                           | 	  } 											|
|---------------------------|---------------------------------------------------|
| for	 					| for loop block declaration						|
|                           | ex. for (perhaps int i = 0; i < 5; i++) {			|
|                           | 	      foo++;									|
|                           | 	  } 											|
|---------------------------|---------------------------------------------------|
| amongst 					| foreach loop block declaration					|
|                           | ex. amongst (int i within array) {				|
|                           | 	      say(i);									|
|                           | 	  } 											|
|---------------------------|---------------------------------------------------|


### Operators


### Operators

### Operators

| Operator | Category   | Operands | Associativity | Precedence |
|----------|------------|----------|----------------|------------|
| `=`      | assignment | binary   | right          | 1          |
| `+=`     | assignment | binary   | right          | 1          |
| `-=`     | assignment | binary   | right          | 1          |
| `*=`     | assignment | binary   | right          | 1          |
| `/=`     | assignment | binary   | right          | 1          |
| `\|\|`   | logical    | binary   | left           | 2          |
| `&&`     | logical    | binary   | left           | 3          |
| `==`     | comparison | binary   | left           | 4          |
| `!=`     | comparison | binary   | left           | 4          |
| `<`      | comparison | binary   | left           | 5          |
| `<=`     | comparison | binary   | left           | 5          |
| `>`      | comparison | binary   | left           | 5          |
| `>=`     | comparison | binary   | left           | 5          |
| `+`      | arithmetic | binary   | left           | 6          |
| `-`      | arithmetic | binary   | left           | 6          |
| `*`      | arithmetic | binary   | left           | 7          |
| `/`      | arithmetic | binary   | left           | 7          |
| `!`      | logical    | unary    | right          | 8          |
| `-`      | arithmetic | unary    | right          | 8          |

### Literals


| Kind        | Syntax                          | Produces    |
| :---------: | :-----------------------------: | :---------: |
| [interger]  | 	42, 0, -7                      | [Integer]   |
| [string]    | e.g. "hello", escapes supported | [String]    |
| [boolean]   | yea, nay                        | [Boolean]   |
| [nil]       | spelling                        | [Nil]       |
| [float]     | 3.14, 0.5, 2.0                  | [Float]     |
| [character] | 'a'                             | [Character] |



### Identifiers

- Start characters: Letters (a-z, A-Z) and underscore (_) — not digits
- Continue characters: Letters, digits (0-9), and underscore
- Case-sensitive: yes
- No length limit for identifiers (aka variable names in my personal parlance). Identifiers cannot be a reserved keyword.

### Comments

- Line comments: `annotate` (everything from `annotate` to end of line is discarded)
- Block comments: `/* ... */`
- Nesting: not supported (a `*/` closes the nearest open `/*`, regardless of any `/*` inside it)
- [Harness note: comment_prefix in tests/lab*/manifest.json is set to the
  token above.]

## Whitespace and termination

- Whitespace significant: no
- Statement terminator: semicolon
- Block delimiters: braces
- Grouping delimiters: parentheses

## Token output format

```
[one line of real --tokenize output]
```

[What each field means. Frozen as of Lab 1; changes are recorded in the
changelog.]

## Grammar

```
[Your complete context-free grammar, current as of the latest activity.
Unambiguous, with precedence and associativity encoded in rule structure.]
```

## Parse output format

```
[one line of real --parse output, e.g. (+ 1.0 (* 2.0 3.0))]
```

- Groupings print as: [form]
- Numbers print as: [form]

## Semantics

### Values and types

[What runtime values exist, and how they are represented in the host
language.]

### Value printing

- Numbers: [e.g. 5 rather than 5.0]
- Nil: [spelling]
- Strings: [with or without quotes]

### Truthiness

[The complete rule. Which values are false in a condition; everything else is
true.]

### Operator semantics

- Arithmetic: [accepted operand types]
- `+` on strings: [concatenation, error, or coercion]
- Mixed types: [what happens]
- Comparison: [accepted operand types]
- Equality across types: [false, or an error]
- Division by zero: [value produced, or runtime error]

### Scope and bindings

- Redeclaration in the same scope: [allowed or an error]
- Uninitialized variable holds: [value]
- Shadowing: [behavior]
- Undefined name: [static error with exit 65, or runtime error with exit 70]

### Control flow and functions

- Logical operators return: [booleans, or the operand]
- Dangling else binds to: [which if]
- Closure capture of a loop variable: [per iteration, or shared]
- Function with no return statement produces: [value]
- Arity mismatch: [message and exit code]

## Native functions


| Name | Arguments | Returns | Notes |
|---|---|---|---|
| [name] | [count and types] | [type] | [caveats] |


## Errors and diagnostics

Message format:

```
[one real static error]
[one real runtime error]
```


| Failure | Exit code |
|---|---|
| [lexical error] | 65 |
| [syntax error] | 65 |
| [runtime error] | 70 |


## Testing conventions


| Folder | Activity | Mode | Flag |
|---|---|---|---|
| tests/lab1 | Scanner | sidecar | `--tokenize` |
| tests/lab2 | Parser | sidecar | `--parse` |
| tests/lab3 | Evaluator | inline | `--eval` |
| tests/lab4 | Context | inline | none |
| tests/lab5 | Functions | inline | none |


```
[specific tests]...
```

Run locally with:

```bash
curl -sSL https://raw.githubusercontent.com/WhiteLicorice/cmsc-124-harness/v1.1/run_tests.py -o run_tests.py
./build.sh
python3 run_tests.py tests/lab1
```

## Sample code

```
[a short program]
```

Output:

```
[its output]
```

## Design rationale

[Why the language is the way it is. Cover the choices that surprised you, the
features you cut, and the decisions you reversed. Specific reasons, not
approval of your own work.]

## Known limitations

- [What doesn't work, what is unimplemented, where behavior is worse than you
  would like.]

## Changelog


| Activity | What changed in the language |
|---|---|
| Lab 1 | [entry] |
