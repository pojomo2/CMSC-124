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
        String outputDir, String baseName, List<String> types) 
    {
        String path = outputDir + "/" + baseName + " .cs";
        
        using (StreamWriter writer = new StreamWriter(path, false, System.Text.Encoding.UTF8))
        {
            writer.WriteLine("namespace CraftingInterpreters.Lox;");
            writer.WriteLine();
            writer.WriteLine("using System.Collections.Generic;");
            writer.WriteLine();
            writer.WriteLine("abstract class " + baseName);
            writer.WriteLine("{");

            DefineVisitor(writer, baseName, types);


            foreach (string type in types)
            {
                string[] parts = type.Split(':');
                string className = parts[0].Trim();
                string fields = parts[1].Trim();

                DefineType(writer, baseName, className, fields); 
            }

            //The base accept() method
            writer.WriteLine();
            writer.WriteLine("  abstract <R> R accept(Visitor<R> visitor);");

            writer.WriteLine("}");    

            writer.Close();    
        }


    }

    private static void DefineType(
        StreamWriter writer, String baseName,
        String className, String fieldList
    )
    {
        writer.WriteLine("  static class " + className + " extends " + baseName + " {");

        //Constructor 
        writer.WriteLine("  " + className + "(" + fieldList + ") {");

        //Store parameters in fields
        String[] fields = fieldList.Split(", ");
        foreach (string field in fields)
        {
            String name = field.Split(" ")[1];
            writer.WriteLine("  this." + name + " = " + name + ";");
        }

        writer.WriteLine("  }");

        //Fields
        writer.WriteLine();
        foreach (string field in fields)
        {
            writer.WriteLine("  final " + field + ";");
        }

        writer.WriteLine("  }");
    }

    private static void DefineVisitor(
        StreamWriter writer, String baseName, List<String> types)
    {
        writer.WriteLine("  interface Visitor<R>");

        foreach (String type in types)
        {
            String typeName = type.Split(":")[0].Trim();
            writer.WriteLine("  R visit" + typeName + baseName + "(" + typeName + " " + baseName.ToLower() + ");");
        }

        writer.WriteLine("  }");
    }

    private static void DefineType(
        StreamWriter writer, String baseName,
        String className, String fieldList
    )
    {
        writer.WriteLine(" static class " + className + " extends " + baseName + " {");

        //Constructor 
        writer.WriteLine("  " + className + "(" + fieldList + ") {");

        //Store parameters in fields
        String[] fields = fieldList.Split(", ");
        foreach (string field in fields)
        {
            String name = field.Split(" ")[1];
            writer.WriteLine("  this." + name + " = " + name + ";");
        }

        writer.WriteLine("  }");

        //Fields
        writer.WriteLine();
        foreach (string field in fields)
        {
            writer.WriteLine("  final " + field + ";");
        }

        writer.WriteLine("  }");

        // Visitor pattern
        writer.WriteLine();
        writer.WriteLine("  @Override");
        writer.WriteLine("  <R> R accept(Visitor<R> visitor) {");
        writer.WriteLine("  return visitor.visit" + className + baseName + "(this);");
        writer.WriteLine("  }");
    }

}