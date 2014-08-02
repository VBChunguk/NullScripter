using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

#warning under construction. : Analyse, Parse, Generate
namespace NullScripter.Script
{
    class Compiler
    {
        private static List<string> FunctionList;
        private static Dictionary<string, VariableType> VariableTable;
        private static bool firstfunction;
        private static string CR;

        private enum VariableType
        { Double, String }
        private enum TokenType 
        {   StartOfTag, EndOfTag,
            Variable, Numeric, Literal, 
            UnaryOperator, CopyOperator, ArithmeticOperator, LogicalOperator, RelativeOperator,
            LabelDeclear, MacroDeclear,
            Function,
            Text};
        private enum SyntaxType 
        { LabelDeclear, MacroDeclear, LiteralDeclear, NumericDeclear, NumericExpress };
        private enum AutomataStatus 
        { Initial };
        private enum BracketStatus
        { Opened, Closed };

        public Compiler ()
        {
            InitialList();
        }
        private void InitialList ()
        {
            FunctionList = new List<string>();
            FunctionList.Add("layopt");

            VariableTable = new Dictionary<string, VariableType>(); 
        }

        /// <summary>
        /// return compiled JAVA code.
        /// </summary>
        /// <param name="str">NullScript code</param>
        /// <returns>JAVA code</returns>
        public string Compile(string str)
        {
            str += "\r\n";

            firstfunction = true;

            bool crashed = false;
            CompileErrorCollection cec = new CompileErrorCollection();

            string script = null;
            string packet = null;
            string declearment = null;

            string basic = "class NullScript\r\n{\r\n";

            MatchCollection mc;
            Regex CRRegex = new Regex(@"(.*?)\r\n");
            Regex SPRegex = new Regex(@"(.*?)\s+?");
            Regex LiteralRegex = new Regex("\" .*? \"", RegexOptions.IgnorePatternWhitespace);

            int col = -1;

            // Preprocessing : Blank
            str = Regex.Replace(str, @"[\ \t]* [<] [\ \t]*", "< ", RegexOptions.IgnorePatternWhitespace);
            str = Regex.Replace(str, @"[\ \t]* [>] [\ \t]*", " >", RegexOptions.IgnorePatternWhitespace);
            str = Regex.Replace(str, @"[\ \t]* [,] [\ \t]*", " , ", RegexOptions.IgnorePatternWhitespace);
            str = Regex.Replace(str, @"[\ \t]* [+] [\ \t]*", " + ", RegexOptions.IgnorePatternWhitespace);
            str = Regex.Replace(str, @"[\ \t]* [-] [\ \t]*", " - ", RegexOptions.IgnorePatternWhitespace);
            str = Regex.Replace(str, @"[\ \t]* [*] [\ \t]*", " * ", RegexOptions.IgnorePatternWhitespace);
            str = Regex.Replace(str, @"[\ \t]* [/] [\ \t]*", " / ", RegexOptions.IgnorePatternWhitespace);
            str = Regex.Replace(str, @"[\ \t]* [=] [\ \t]*", " = ", RegexOptions.IgnorePatternWhitespace);
            str = Regex.Replace(str, @"[\ \t]* [&]{2} [\ \t]*", " && ", RegexOptions.IgnorePatternWhitespace);
            str = Regex.Replace(str, @"[\ \t]* [|][2} [\ \t]*", " || ", RegexOptions.IgnorePatternWhitespace);

            mc = CRRegex.Matches(str);
            string chunk = null;
            BracketStatus bracketstatus = BracketStatus.Closed;
            List<string> scriptchunk = new List<string>();

            // Reforming scripts
            foreach (Match element in mc)
            {
                string temp = element.Groups[1].Value;
                // Checking Blank Column
                if (Regex.IsMatch(temp, @"^\s*$"))
                    continue;

                if (Regex.IsMatch(temp, "<.+?>") && !Regex.IsMatch(temp, "< label"))
                {
                    string testpacket = Regex.Match(temp, "<(.+?)>").Groups[1].Value;

                    //Opening Function
                    if (!Regex.IsMatch(testpacket, "/"))
                    {
                        chunk += temp;

                        if (bracketstatus == BracketStatus.Opened)
                            throw new CompileError(0, CompileError.ErrorType.Syntax_Error);

                        bracketstatus = BracketStatus.Opened;
                    }
                    else
                    {
                        if (bracketstatus == BracketStatus.Closed)
                            throw new CompileError(0, CompileError.ErrorType.Syntax_Error);

                        bracketstatus = BracketStatus.Closed;

                        chunk += temp;
                        scriptchunk.Add(chunk);
                        chunk = null;
                    }

                }
                else
                {
                    if (bracketstatus == BracketStatus.Closed)
                    {
                        scriptchunk.Add(temp);
                        chunk = null;
                    }
                    else
                        chunk += temp;
                }
            }
            scriptchunk.Add(chunk);

            // Compile
            mc = CRRegex.Matches(str);
            foreach (Match element in mc)
            {
                CR = "\r\n";

                try
                {
                    packet = null;
                    col++;

                    // Checking Blank Column
                    if (Regex.IsMatch(element.Groups[0].Value, @"^  \s*  $", RegexOptions.IgnorePatternWhitespace))
                        continue;

                    MatchCollection mc2 = SPRegex.Matches(element.Groups[1].Value + " ");
                    List<TokenType> typelist = new List<TokenType>();

                    foreach (Match e in mc2)
                    {
                        string token = e.Groups[1].Value;
                        typelist.Add(Analyse(token));
                    }

                    // Generate Code
                    packet = GenerateCode(Parse(typelist), typelist, mc2);
                }
                catch (CompileError e)
                {
                    crashed = true;
                    e.col = col;

                    cec.Add(e);
                }

                script += packet;
            }

            if (crashed)
                throw cec;

            foreach (KeyValuePair<string, VariableType> e in VariableTable)
            {
                switch(e.Value)
                {
                    case VariableType.Double:
                        declearment += "double ";
                        break;

                    case VariableType.String:
                        declearment += "string ";
                        break;

                    default:
                        throw new NotImplementedException();
                }

                declearment += e.Key.Replace("$", "_") + ";\r\n";
            }

            // Proprecessing : Blank (For legibillity)
            script = Regex.Replace(script, @"[\ \t]* [,] [\ \t]*", ", ", RegexOptions.IgnorePatternWhitespace);
            script = Regex.Replace(script, @"[\ \t]* [+] [\ \t]*", " + ", RegexOptions.IgnorePatternWhitespace);
            script = Regex.Replace(script, @"[\ \t]* [-] [\ \t]*", " - ", RegexOptions.IgnorePatternWhitespace);
            script = Regex.Replace(script, @"[\ \t]* [*] [\ \t]*", " * ", RegexOptions.IgnorePatternWhitespace);
            script = Regex.Replace(script, @"[\ \t]* [/] [\ \t]*", " / ", RegexOptions.IgnorePatternWhitespace);
            script = Regex.Replace(script, @"[\ \t]* [=] [\ \t]*", " = ", RegexOptions.IgnorePatternWhitespace);
            script = Regex.Replace(script, @"[\ \t]* [&]{2} [\ \t]*", " && ", RegexOptions.IgnorePatternWhitespace);
            script = Regex.Replace(script, @"[\ \t]* [|][2} [\ \t]*", " || ", RegexOptions.IgnorePatternWhitespace);

            script = basic + declearment + "\r\n" + script + "\r\n}\r\n}";

            return script;
        }

        /// <summary>
        /// return type of token
        /// </summary>
        /// <param name="token">token string</param>
        /// <returns>type of token</returns>
        private static TokenType Analyse(string token)
        {
            

            if (token == "<")
                return TokenType.StartOfTag;
            if (token == ">")
                return TokenType.EndOfTag;

            if (token == "label")
                return TokenType.LabelDeclear;
            if (token == "macro")
                return TokenType.MacroDeclear;

            if (Regex.IsMatch(token, "[+]{2} | [-]{2}", RegexOptions.IgnorePatternWhitespace))
                return TokenType.UnaryOperator;
            if (Regex.IsMatch(token, "[*/+-]"))
                return TokenType.ArithmeticOperator;
            if (Regex.IsMatch(token, "[&]{2} | [|]{2}", RegexOptions.IgnorePatternWhitespace))
                return TokenType.LogicalOperator;
            if (Regex.IsMatch(token, "([><] =?) | != | ==", RegexOptions.IgnorePatternWhitespace))
                return TokenType.RelativeOperator;
            if (token == "=")
                return TokenType.CopyOperator;

            if (FunctionList.Contains(token))
                return TokenType.Function;

            if (Regex.IsMatch(token, @"^ [a-zA-Z_][a-zA-Z0-9_]* $", RegexOptions.IgnorePatternWhitespace))
                return TokenType.Literal;
            if (Regex.IsMatch(token, @"^ [$][a-zA-Z_][a-zA-Z0-9_]* $", RegexOptions.IgnorePatternWhitespace))
                return TokenType.Variable;
            if (Regex.IsMatch(token, @"^ [+-]?[0-9]*[.]?[0-9]* $", RegexOptions.IgnorePatternWhitespace))
                return TokenType.Numeric;
            
            return TokenType.Text;
        }

        /// <summary>
        /// Parse with list
        /// </summary>
        /// <param name="list">list of TokenType</param>
        /// <returns>type of syntax</returns>
        private static SyntaxType Parse(List<TokenType> list)
        {
            if (list[0] == TokenType.StartOfTag &&
                list[1] == TokenType.LabelDeclear &&
                list[2] == TokenType.Literal &&
                list[3] == TokenType.EndOfTag)
                return SyntaxType.LabelDeclear;

            if (list[0] == TokenType.Variable &&
                list[1] == TokenType.CopyOperator)
            {
                for (int i = 2; i < list.Count; i++)
                {
                    if (list[i] == TokenType.Literal && list.Count == 3)
                        return SyntaxType.LiteralDeclear;

                    if (list[i] == list[i - 1] ||
                        (list[i] != TokenType.Numeric && list[i] != TokenType.ArithmeticOperator && list[i] != TokenType.Variable))
                        throw new CompileError(CompileError.ErrorType.Syntax_Error);
                }
                return SyntaxType.NumericDeclear;
            }


            throw new CompileError(CompileError.ErrorType.Syntax_Error);
        }

        /// <summary>
        /// Generates JAVA code
        /// </summary>
        /// <param name="type">Type of syntax</param>
        /// <param name="list">List of token type</param>
        /// <param name="mc">Original string packet</param>
        /// <returns>JAVA code</returns>
        private static string GenerateCode (SyntaxType type, List<TokenType> list, MatchCollection mc)
        {
            string packet = null;

            switch (type)
            {
                case SyntaxType.LabelDeclear:
                    if (firstfunction)
                    {
                        packet = CR + "void " + mc[2].Value + "()" + CR + "{" + CR;
                        firstfunction = false;
                    }
                    else
                       packet = CR + "}" + CR + "void " + mc[2].Value + "()" + CR + "{" + CR;

                    break;

                case SyntaxType.NumericDeclear:
                    if (VariableTable.ContainsKey(mc[0].Groups[1].Value) && VariableTable[mc[0].Groups[1].Value] == VariableType.String)
                        throw new CompileError(CompileError.ErrorType.Duplicated_Declear);

                    if (!VariableTable.ContainsKey(mc[0].Groups[1].Value))
                        VariableTable.Add(mc[0].Groups[1].Value, VariableType.Double);

                    for (int i = 2; i < list.Count; i++)
                        if (list[i] == TokenType.Variable && VariableTable.ContainsKey(mc[i].Groups[1].Value) == false)
                            throw new CompileError(CompileError.ErrorType.Unknown_Variable);

                    foreach (Match e in mc)
                        packet += e.Value;

                    packet = CR + packet.Replace("$", "_");
                    packet += ";" + CR;
                    break;

                case SyntaxType.LiteralDeclear:
                    if (VariableTable.ContainsKey(mc[0].Groups[1].Value) && VariableTable[mc[0].Groups[1].Value] == VariableType.Double)
                        throw new CompileError(CompileError.ErrorType.Duplicated_Declear);

                    if (!VariableTable.ContainsKey(mc[0].Groups[1].Value))
                        VariableTable.Add(mc[0].Groups[1].Value, VariableType.String);

                    packet = mc[0].Value.Replace("$", "_") + "=\"" + mc[2].Groups[1].Value + "\";\r\n";
                    break;

                default:
                    throw new NotImplementedException();
            }

            return packet;
        }
    }

    class CompileError : Exception
    {
        public int col;
        public ErrorType errortype;
        public enum ErrorType { Type_Error, Syntax_Error, Invalid_Declear, Duplicated_Declear, Unknown_Variable };

        public CompileError (ErrorType errortype)
        {
            this.errortype = errortype;
        }
        public CompileError (int col, ErrorType errortype)
        {
            this.col = col;
            this.errortype = errortype;
        }
        public override string Message
        {
            get
            {
                return errortype.ToString();
            }
        }
    }

    class CompileErrorCollection : Exception
    {
        public List<CompileError> list;
        public int count
        {
            get
            {
                return list.Count;
            }
        }

        public CompileErrorCollection()
        {
            list = new List<CompileError>();
        }
        public CompileError this[int idx]
        {
            get
            {
                return list[idx];
            }
        }

        public void Add(CompileError ce)
        {
            list.Add(ce);
        }
    }
}
