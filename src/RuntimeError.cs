using System.Reflection.Metadata;
using Ileto.Took;
using Microsoft.CSharp.RuntimeBinder;

namespace Ileto;

class RuntimeError : Exception
{
    public Token Token { get; }

    public RuntimeError(Token token, string message) : base(message)
    {
        Token = token;
    }
}