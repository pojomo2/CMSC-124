using System;


using System.IO;
using System.Collections.Generic;
using System.Reflection.Metadata;
using Microsoft.VisualBasic;

namespace Ileto.Took;

public static class GenerateAst
{
    public static void Main(string[] args){
        if (args.Length != 1)
        {
            Console.Error.WriteLine("Usage: generate_ast <output directory>");
            Environment.Exit(64);
            return;
        }

        var types = new List<string>
        {
            "Binary   : Expr left, Token operator, Expr right",
            "Grouping : Expr expression",
            "Literal  : Object value",
            "Unary    : Token operator, Expr right" 
        };

        string outputDir = args[0];
        DefineAst(outputDir, "Expr", types);
    }

    private static void DefineAst(
        String outputDir, 
        String baseName, 
        List<String> types) 
    {
        Directory.CreateDirectory(outputDir);
        string path = Path.Combine(outputDir, baseName + ".cs");

        using StreamWriter writer = new StreamWriter(path, false, System.Text.Encoding.UTF8);

        //Keep this namespace the same as the one AstPrinter uses
        writer.WriteLine("using Ileto;");
        writer.WriteLine("namespace Ileto.Took;");
        writer.WriteLine();
        writer.WriteLine($"public abstract class {baseName}");
        writer.WriteLine("{");

        DefineVisitor(writer, baseName, types);

        foreach (string type in types)
        {
            string[] parts = type.Split(':', 2);
            string className = parts[0].Trim();
            string fields = parts[1].Trim();

            DefineType(writer, baseName, className, fields);
        }

        writer.WriteLine();
        writer.WriteLine("      public abstract R Accept<R>(IVisitor<R> visitor);");
        writer. WriteLine("}");

       

    }

    private static void DefineVisitor(
        StreamWriter writer, 
        string baseName, 
        List<String> types)
    {
        writer.WriteLine("      public interface IVisitor<R>");
        writer.WriteLine("      {");

        foreach (string type in types)
        {
            string typeName = type.Split(':', 2)[0].Trim();

            writer.WriteLine(
                $"      R Visit{typeName}{baseName}({typeName} expr);");
        }

        writer.WriteLine("      }");
    }

    private static void DefineType(
        StreamWriter writer, 
        String baseName,
        String className, 
        String fieldList)
    {
        string[] fields = fieldList.Split(
            ',',
            StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

        writer.WriteLine($"     public sealed class {className} : {baseName}");
        writer.WriteLine("   {");

        //Generate read-only properties for the node's fields
        foreach (string field in fields)
        {
            string[] parts = field.Split(
                ' ',
                StringSplitOptions.RemoveEmptyEntries);
            
            string fieldType = parts[0] == "Object" ? "Object" : parts[0];
            string fieldName = parts[1];
            string propertyName = 
                char.ToUpperInvariant(fieldName[0]) + fieldName[1..];

            writer.WriteLine(
                $"        public {fieldType} {propertyName} {{ get; }}"); 
        }

        // Generate the constructor parameters
        string[] parameters = new string[fields.Length];

        for (int i = 0; i < fields.Length; i++)
        {
            string[] parts = fields[i].Split(
                ' ',
                StringSplitOptions.RemoveEmptyEntries);
            
            string fieldType = parts[0] == "Object" ? "object" : parts[0];
            string fieldName = parts[1];
            string parameterName = 
                fieldName == "operator" ? "@operator" : fieldName;
            
            parameters[i] = $"{fieldType} {parameterName}";
        }

        writer.WriteLine();
        writer.WriteLine(
            $"        public {className}({string.Join(", ", parameters)})");
    writer.WriteLine("        {");

    // Assign constructor parameters to properties.
    foreach (string field in fields)
    {
        string[] parts = field.Split(
            ' ',
            StringSplitOptions.RemoveEmptyEntries);

        string fieldName = parts[1];
        string propertyName =
            char.ToUpperInvariant(fieldName[0]) + fieldName[1..];
        string parameterName =
            fieldName == "operator" ? "@operator" : fieldName;

        writer.WriteLine(
            $"            {propertyName} = {parameterName};");
    }

    writer.WriteLine("        }");

    // Generate the visitor dispatch method.
    writer.WriteLine();
    writer.WriteLine(
        $"        public override R Accept<R>(IVisitor<R> visitor)");
    writer.WriteLine("        {");
    writer.WriteLine(
        $"            return visitor.Visit{className}{baseName}(this);");
    writer.WriteLine("        }");

    writer.WriteLine("    }");
    }

}