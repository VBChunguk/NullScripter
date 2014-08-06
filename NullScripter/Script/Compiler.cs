using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

using NullScripter;

#warning under construction. : Analyse, Parse, Generate
namespace NullScripter.Script
{
    class Compiler
    {
        #region Declarement
        private List<string> FunctionList;
        private Dictionary<string, VariableType> VariableTable;
        private bool firstfunction;
        private const string CR = "\r\n";

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
        { LabelDeclear, MacroDeclear, LiteralDeclear, NumericDeclear, NumericExpress, FunctionCall };
        private enum AutomataStatus 
        { Initial };
        private enum BracketStatus
        { Opened, Closed };
        #endregion
        #region Initializing
        private void InitialList ()
        {
            FunctionList = new List<string>();
            FunctionList.Add("layopt");

            VariableTable = new Dictionary<string, VariableType>(); 
        }
        #endregion
        #region .ctor
        public Compiler()
        {
            InitialList();
        }
        #endregion
        
        public string Compile(string str)
        {
            #region Initializing
            Debugger.CarriageReturn();
            Debugger.WriteLine("Compiling...");
            firstfunction = true;

            bool crashed = false;
            CompileErrorCollection cec = new CompileErrorCollection();

            string script = null;
            string packet = null;
            string declearment = null;

            string basic = "class NullScript" + CR + "{" + CR;

            Regex CRRegex = new Regex(@"(.*?)\r\n");
            Regex SPRegex = new Regex(@"(.*?)\s+?");
            Regex LiteralRegex = new Regex("\" .*? \"", RegexOptions.IgnorePatternWhitespace);

            int col = -1;

            string chunk = null;
            BracketStatus bracketstatus = BracketStatus.Closed;
            List<string> scriptchunk = new List<string>();
            #endregion

            #region Preprocessing Scripts
            // Preprocessing : Blank
            str += CR;
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
            #endregion
            #region Reforming Scripts
            foreach (Match element in CRRegex.Matches(str))
            {
                string temp = element.Groups[1].Value;
                // Checking Blank Column
                if (string.IsNullOrEmpty(temp))
                    continue;

                if (Regex.IsMatch(temp, "<.+?>") && !temp.StartsWith("< label"))
                {
                    string testpacket = Regex.Match(temp, "<(.+?)>").Groups[1].Value.Trim();

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

                        chunk += " " + temp;
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
                        chunk += " " + temp;
                }
            }
            if (chunk != null)
                scriptchunk.Add(chunk);

            Debugger.CarriageReturn();
            Debugger.WriteLine("Reformed Script :");
            foreach (string e in scriptchunk)
                Debugger.WriteLine(e);
            #endregion

            #region Compiling
            StringBuilder scriptBuilder = new StringBuilder();
            foreach (string element in scriptchunk)
            {
                try
                {
                    packet = null;
                    col++;

                    // Checking Blank Column
                    if (string.IsNullOrWhiteSpace(element))
                        continue;

                    MatchCollection mc = SPRegex.Matches(element + " ");
                    List<TokenType> typelist = new List<TokenType>();

                    for (int i = 0; i < mc.Count; i++)
                    {
#warning Scaffolding
                        string token = mc[i].Groups[1].Value;
                        typelist.Add(Analyse(token, i == 0, i == mc.Count - 1));
                    }

                    Debugger.CarriageReturn();
                    Debugger.WriteLine("TypeList of " + element);
                    foreach (TokenType e in typelist)
                        Debugger.WriteLine(e.ToString());

                    // Generate Code
                    packet = GenerateCode(Parse(typelist), typelist, mc);
                    Debugger.WriteLine("Generated Packet : " + packet);
                }
                catch (CompileError e)
                {
                    crashed = true;
                    e.col = col;

                    cec.Add(e);
                }

                scriptBuilder.Append(packet);
            }
            script = scriptBuilder.ToString();
            #endregion
            #region Compile Error Handing
            Debugger.CarriageReturn();
            if (crashed)
            {
                Debugger.WriteLine("Crashed.");
                throw cec;
            }
            else
                Debugger.WriteLine("Compiled.");
            #endregion

            #region Declarement Handling
            StringBuilder declarement = new StringBuilder();
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
                declarement.AppendFormat("{0};{1}", e.Key.Replace("$", "_"), CR);
            }
            #endregion

            #region Proprecessing
            // Proprecessing : Blank (For legibillity)
            if (script != null)
            {
                script = Regex.Replace(script, @"[\ \t]* [,] [\ \t]*", ", ", RegexOptions.IgnorePatternWhitespace);
                script = Regex.Replace(script, @"[\ \t]* [+] [\ \t]*", " + ", RegexOptions.IgnorePatternWhitespace);
                script = Regex.Replace(script, @"[\ \t]* [-] [\ \t]*", " - ", RegexOptions.IgnorePatternWhitespace);
                script = Regex.Replace(script, @"[\ \t]* [*] [\ \t]*", " * ", RegexOptions.IgnorePatternWhitespace);
                script = Regex.Replace(script, @"[\ \t]* [/] [\ \t]*", " / ", RegexOptions.IgnorePatternWhitespace);
                script = Regex.Replace(script, @"[\ \t]* [=] [\ \t]*", " = ", RegexOptions.IgnorePatternWhitespace);
                script = Regex.Replace(script, @"[\ \t]* [&]{2} [\ \t]*", " && ", RegexOptions.IgnorePatternWhitespace);
                script = Regex.Replace(script, @"[\ \t]* [|][2} [\ \t]*", " || ", RegexOptions.IgnorePatternWhitespace);
            }

            script = string.Format("{0}{1}{3}{2}{3}\\}{3}\\}", basic, declarement.ToString(), script, CR);
            #endregion

            #region Debugging
            Debugger.WriteLine("Result :\r\n" + script);
            Debugger.CarriageReturn();
            #endregion

            return script;
        }

        private TokenType Analyse(string token, bool first, bool last)
        {
            #region Token Type Matching
            if (first)
            {
                if (token == "<")
                    return TokenType.StartOfTag;
            }
            if (last)
            {
                if (token == ">")
                    return TokenType.EndOfTag;
            }

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
            #endregion
        }
        private static SyntaxType Parse(List<TokenType> list)
        {
            #region Parsing
#warning Scaffoldings....
            if (list.Count == 4 &&
                list[0] == TokenType.StartOfTag &&
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

            if (list.Count > 7 && 
                list[0] == TokenType.StartOfTag && list[1] == TokenType.Function && list[2] == TokenType.RelativeOperator &&
                list[list.Count - 4] == TokenType.RelativeOperator && list[list.Count - 2] == TokenType.Function && list[list.Count - 1] == TokenType.EndOfTag)
                return SyntaxType.FunctionCall;
            #endregion
            #region Exception
            throw new CompileError(CompileError.ErrorType.Syntax_Error);
            #endregion
        }
        private string GenerateCode (SyntaxType type, List<TokenType> list, MatchCollection mc)
        {
            string value = null;
            string packet = null;
            StringBuilder packetBuilder = new StringBuilder();
            switch (type)
            {
                #region Label Declarement
                case SyntaxType.LabelDeclear:
                    if (firstfunction)
                    {
                        packet = string.Format("{1}void {0}(){1}\\{{1}", mc[2].Value, CR);
                        firstfunction = false;
                    }
                    else
                        packet = string.Format("{1}\\}{1}void {0}(){1}\\{{1}", mc[2].Value, CR);

                    break;
                #endregion
                #region Numeric Variable declarement
                case SyntaxType.NumericDeclear:
                    if (VariableTable.ContainsKey(mc[0].Groups[1].Value) && VariableTable[mc[0].Groups[1].Value] == VariableType.String)
                        throw new CompileError(CompileError.ErrorType.Duplicated_Declear);

                    for (int i = 2; i < list.Count; i++)
                        if (list[i] == TokenType.Variable && VariableTable.ContainsKey(mc[i].Groups[1].Value) == false)
                            throw new CompileError(CompileError.ErrorType.Unknown_Variable);

                    if (!VariableTable.ContainsKey(mc[0].Groups[1].Value))
                        VariableTable.Add(mc[0].Groups[1].Value, VariableType.Double);

                    foreach (Match e in mc)
                        packet += e.Value;

                    packet = string.Format("{1}{0};{1}", packet.Replace('$', '_'), CR);
                    break;
                #endregion
                #region Literal Declarement
                case SyntaxType.LiteralDeclear:
                    if (VariableTable.ContainsKey(mc[0].Groups[1].Value) && VariableTable[mc[0].Groups[1].Value] == VariableType.Double)
                        throw new CompileError(CompileError.ErrorType.Duplicated_Declear);

                    if (!VariableTable.ContainsKey(mc[0].Groups[1].Value))
                        VariableTable.Add(mc[0].Groups[1].Value, VariableType.String);

                    packet = string.Format("{0}=\"{1}\";{2}", mc[0].Value.Replace("$", "_"), mc[2].Groups[1].Value, CR);
                    break;
                #endregion
                #region Function Call
                case SyntaxType.FunctionCall:
                    for (int i = 3; i < mc.Count - 4; i++ )
                    {
                        if (mc[i].Groups[0].Value == "$value ")
                            value = mc[i + 2].Groups[0].Value;
                    }

                    if (value != null)
                       packet = string.Format("layopt({0});", value);
                    break;
                #endregion
                #region Exception
                default:
                    throw new NotImplementedException();
                #endregion
            }
            return packet;
        }
    }

    #region Exception Class
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

    [Serializable]
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
#endregion
}
