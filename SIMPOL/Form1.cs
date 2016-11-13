using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SIMPOL
{

    public partial class simpolInterpreter : Form
    {
        public simpolInterpreter()
        {
            InitializeComponent();
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "SIM File|*.sim";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                if (Path.GetExtension(ofd.FileName) == ".sim")
                {
                    txtFile.Text = ofd.FileName;
                }
                else
                {
                    MessageBox.Show("File extension must be .SIM",
                        "Wrong File Type", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
        }
        
        private void btnInterpret_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtFile.Text))
            {
                //Token.tokenStream = "";
                Token.Clear();
                Symbol.Clear();
                txtOutput.Clear();
                StreamReader sr = new StreamReader(txtFile.Text);
                string result = sr.ReadToEnd();
                Lexer lexer = new Lexer(result);
                Interpreter interpreter = new Interpreter(lexer);
                result = "Sentence: \r\n" + result;
                result += "\r\n\r\nResult: \r\n" + interpreter.Program().ToString();
                //result += "\r\nToken Stream:" + Token.tokenStream;
                dgvLexemeTable.DataSource = Token.GetTable();
                dgvSymbolTable.DataSource = Symbol.GetTable();
                txtOutput.Text = result.ToString();
                sr.Close();
            }
        }
    }

    public class Token
    {
        public const string TokenInteger = "INT";
        public const string TokenStg = "STG";
        public const string TokenDollar = "$";
        public const string TokenLetter = "LETTER";
        public const string TokenAdd = "ADD";
        public const string TokenSubtract = "SUB";
        public const string TokenMultiply = "MUL";
        public const string TokenDivide = "DIV";
        public const string TokenModulo = "MOD";
        public const string TokenAnd = "AND";
        public const string TokenOr = "OHR";
        public const string TokenNon = "NON";
        public const string TokenGreaterThan = "GRT";
        public const string TokenGreaterEqual = "GRE";
        public const string TokenLessThan = "LET";
        public const string TokenLessEqual = "LEE";
        public const string TokenEqual = "EQL";
        public const string TokenExponent = "EXPONENT";
        public const string TokenEOF = "EOF";
        public const string TokenLParen = "LPAREN";
        public const string TokenRParen = "RPAREN";
        public const string TokenLBrace = "LBRACE";
        public const string TokenRBrace = "RBRACE";
        public const string TokenVariable = "VARIABLE";
        public const string TokenCode = "CODE";
        public const string TokenPut = "PUT";
        public const string TokenIn = "IN";
        public const string TokenId = "ID";
        public const string TokenPrint = "PRT";
        public const string TokenAsk = "ASK";
        public const string TokenKeyword = "KEYWORD";
        public static Dictionary<string, Token> ReservedKeywords = new Dictionary<string, Token>
        {
            { "variable", new Token("VARIABLE", "variable") },
            { "code", new Token("CODE", "code") },
            { "INT", new Token("INT", "INT") },
            { "BLN", new Token("BLN", "BLN") },
            { "STG", new Token("STG", "STG") },
            { "PRT", new Token("PRT", "PRT") },
            { "ASK", new Token("ASK", "ASK") },
            { "PUT", new Token("PUT", "PUT") },
            { "IN", new Token("IN", "IN") },
            { "ADD", new Token("ADD", "ADD") },
            { "SUB", new Token("SUB", "SUB") },
            { "MUL", new Token("MUL", "MUL") },
            { "DIV", new Token("DIV", "DIV") },
            { "MOD", new Token("MOD", "MOD") },
            { "GRT", new Token("GRT", "GRT") },
            { "GRE", new Token("GRE", "GRE") },
            { "LET", new Token("LET", "LET") },
            { "LEE", new Token("LEE", "LEE") },
            { "EQL", new Token("EQL", "EQL") },
            { "AND", new Token("AND", "AND") },
            { "OHR", new Token("OHR", "OHR") },
            { "NON", new Token("NON", "NON") },
            { "true", new Token("KEYWORD", "true") },
            { "false", new Token("KEYWORD", "false") },
        };
        public static string tokenStream { get; set; }
        public string type { get; set; }
        public string value { get; set; }
        public static DataTable dt= CreateTable();
        public Token(string type, string value)
        {
            this.type = type;
            this.value = value;
        }
        //public string TokenString(string type, string value)
        //{
        //    return "\r\nToken(" + type + ", " + value + ")";
        //}
        private static DataTable CreateTable()
        {
            dt = new DataTable("Tokens");
            dt.Columns.Add("Type", typeof(string));
            //dt.PrimaryKey = new DataColumn[] { dt.Columns["Name"] };
            dt.Columns.Add("Value", typeof(string));
            return dt;
        }
        public static DataTable GetTable()
        {
            return dt;
        }
        public static void AddRecord(string type, string value)
        {
            dt.Rows.Add(new string[] { type, value });
        }
        public static void Clear()
        {
            dt.Clear();
        }
    }

    public class Symbol
    {
        public string name { get; set; }
        public string type { get; set; }
        public string value { get; set; }
        private static DataTable dt = CreateTable();
        public Symbol(string type, string name, string value)
        {
            this.type = type;
            this.name = name;
            this.value = value;
            DataRow[] dr = Lookup(name);
            if (dr.Length == 0)
            {
                dt.Rows.Add(new string[] { type, name, value });
            }
            else
            {
                MessageBox.Show("Variable '" + name + "' already exists.",
                        "Duplicate Variable", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
            }
        }
        public static DataTable CreateTable()
        {
            dt = new DataTable("Symbols");
            dt.CaseSensitive = true;
            dt.Columns.Add("Type", typeof(string));
            dt.Columns.Add("Name", typeof(string));
            //dt.PrimaryKey = new DataColumn[] { dt.Columns["Name"] };
            dt.Columns.Add("Value", typeof(string));
            return dt;
        }
        public static DataTable GetTable()
        {
            return dt;
        }
        public static DataRow[] Lookup(string name)
        {
            DataRow[] foundRows = dt.Select("Name = '" + name + "'");
            return foundRows;
        }
        public static void EditSymbol(DataRow[] dr, string newValue)
        {
            if (dr.Length > 0)
            {
                string type = dr[0]["Type"].ToString();
                string name = dr[0]["Name"].ToString();
                if (type == Token.TokenInteger)
                {
                    int integerValue; bool isInteger;
                    isInteger = int.TryParse(newValue, out integerValue);
                    if (isInteger)
                    {
                        dr[0]["Value"] = integerValue;
                    }
                    else
                    {
                        MessageBox.Show("Variable value must be of type integer.",
                            "Type Mismatch", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
                else if (type == "BLN")
                {
                    if (newValue == "true" ||
                        newValue == "false")
                    {
                        dr[0]["Value"] = newValue;
                    }
                    else
                    {
                        MessageBox.Show("Variable value must be of type boolean.",
                            "Type Mismatch", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
                else if (type == "STG")
                {
                    if (newValue[0] == '$' ||
                        newValue[newValue.Length - 1] == '$')
                    {
                        newValue = newValue.TrimStart('$');
                        newValue = newValue.TrimEnd('$');
                        //newValue = newValue.Remove(newValue.Length - 1);
                        dr[0]["Value"] = newValue;
                    }
                    else
                    {
                        MessageBox.Show("Variable value must be of type string, enclosed with dollar signs \'$\'.",
                            "Type Mismatch", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Variable does not exist.",
                        "Undeclared Variable", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
            }
        }
        public static void Clear()
        {
            dt.Clear();
        }
    }

    public class Lexer
    {
        string text;
        int position;
        char currentChar;

        public Lexer(string inputText)
        {
            text = inputText;
            position = 0;
            currentChar = text[position];
        }

        private void Error()
        {
            MessageBox.Show("Error parsing input.",
                        "Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
        }

        private void EraseSpaces()
        {
            while (currentChar != '\0' && char.IsWhiteSpace(currentChar))
            {
                if (currentChar.ToString() == Environment.NewLine) { break; }
                Advance();
            }
        }

        public void Advance()
        {
            position += 1;
            if (position > text.Length - 1)
            {
                currentChar = '\0';
            }
            else
            {
                currentChar = text[position];
            }
        }

        private Token Identifier()
        {
            string result = "";
            Token token;
            while (currentChar != '\0' && char.IsLetterOrDigit(currentChar))
            {
                result += currentChar;
                Advance();
            }
            if (Token.ReservedKeywords.TryGetValue(result, out token))
            {
                return token;
            }
            else
            {
                token = new Token(Token.TokenId, result);
                return token;
            }

        }

        private string Integer()
        {
            string result = "";
            while (currentChar != '\0' && char.IsDigit(currentChar) == true)
            {
                result += currentChar;
                Advance();
            }
            return result;
        }

        public Token GetNextToken()
        {
            Token token;
            while (currentChar != '\0')
            {
                if (char.IsWhiteSpace(currentChar))
                {
                    EraseSpaces();
                    continue;
                }
                else if (char.IsDigit(currentChar))
                {
                    token = new Token(Token.TokenInteger, Integer());
                    //Token.tokenStream += token.TokenString(token.type, token.value);
                    Token.AddRecord(token.type, token.value);
                    return token;
                }
                else if (char.IsLetter(currentChar))
                {
                    token = Identifier();
                    //Token.tokenStream += token.TokenString(token.type, token.value);
                    Token.AddRecord(token.type, token.value);
                    return token;
                }
                else if (currentChar == '$')
                {
                    Token startDollar = new Token(Token.TokenDollar, currentChar.ToString());
                    //Token.tokenStream += startDollar.TokenString(startDollar.type, startDollar.value);
                    Token.AddRecord(startDollar.type, startDollar.value);
                    token = ParseString();
                    //Token.tokenStream += token.TokenString(token.type, token.value);
                    Token endDollar = new Token(Token.TokenDollar, currentChar.ToString());
                    //Token.tokenStream += endDollar.TokenString(endDollar.type, endDollar.value);
                    Token.AddRecord(endDollar.type, endDollar.value);
                    Advance();
                    return token;
                }
                else if (currentChar == '(')
                {
                    token = new Token(Token.TokenLParen, currentChar.ToString());
                    //Token.tokenStream += token.TokenString(token.type, token.value);
                    Token.AddRecord(token.type, token.value);
                    Advance();
                    return token;
                }
                else if (currentChar == ')')
                {
                    token = new Token(Token.TokenRParen, currentChar.ToString());
                    //Token.tokenStream += token.TokenString(token.type, token.value);
                    Token.AddRecord(token.type, token.value);
                    Advance();
                    return token;
                }
                else if (currentChar == '{')
                {
                    token = new Token(Token.TokenLBrace, currentChar.ToString());
                    //Token.tokenStream += token.TokenString(token.type, token.value);
                    Token.AddRecord(token.type, token.value);
                    Advance();
                    return token;
                }
                else if (currentChar == '}')
                {
                    token = new Token(Token.TokenRBrace, currentChar.ToString());
                    //Token.tokenStream += token.TokenString(token.type, token.value);
                    Token.AddRecord(token.type, token.value);
                    Advance();
                    return token;
                }
                else
                {
                    Error();
                    break;
                }
            }
            token = new Token(Token.TokenEOF, null);
            return token;
        }

        public Token ParseString()
        {
            string result = "";
            while (currentChar != '\0')
            {
                result += currentChar;
                Advance();
                if (currentChar == '\r' || currentChar == '\n')
                {
                    break;
                }
                else if (currentChar == '$')
                {
                    result += currentChar;
                    break;
                }
            }
            return new Token(Token.TokenStg, result);
        }
    }

    public class Interpreter
    {
        Token currentToken;
        Token token;
        Lexer lexer;
        int result;

        public Interpreter(Lexer inputLexer)
        {
            lexer = inputLexer;
            currentToken = lexer.GetNextToken();
        }
        
        private void Error()
        {
            MessageBox.Show("Invalid syntax.",
                        "Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
        }

        private void Eat(string tokenType)
        {
            if (currentToken.type == tokenType)
            {
                currentToken = lexer.GetNextToken();
            }
            else
            {
                Error();
            }
        }

        public string Program()
        {
            string result = "";
            Eat(Token.TokenVariable);
            Eat(Token.TokenLBrace);
            while (currentToken.type != Token.TokenRBrace)
            {
                VariableDeclaration();
            }
            Eat(Token.TokenRBrace);
            Eat(Token.TokenCode);
            Eat(Token.TokenLBrace);
            while (currentToken.type != Token.TokenRBrace)
            {
                if (currentToken.type == Token.TokenPrint)
                {
                    result += Print();
                    result += "\r\n";
                }
                else if (currentToken.type == Token.TokenAsk)
                {
                    Eat(Token.TokenAsk);
                    Ask();
                }
                else if (currentToken.type == Token.TokenPut)
                {
                    AssignmentStatement();
                }
                else
                {
                    Error();
                    break;
                }
            }
            Eat(Token.TokenRBrace);
            return result;
        }

        public string Print()
        {
            Eat(Token.TokenPrint);
            string output = null;
            if (currentToken.type == Token.TokenAdd ||
                currentToken.type == Token.TokenSubtract ||
                currentToken.type == Token.TokenMultiply ||
                currentToken.type == Token.TokenDivide ||
                currentToken.type == Token.TokenModulo ||
                currentToken.type == Token.TokenInteger)
            {
                output = Expr().ToString();
            }
            else if (currentToken.type == Token.TokenGreaterThan ||
                currentToken.type == Token.TokenGreaterEqual ||
                currentToken.type == Token.TokenLessThan ||
                currentToken.type == Token.TokenLessEqual ||
                currentToken.type == Token.TokenEqual)
            {
                output = NumericPredicate().ToString().ToLower();
            }
            else if (currentToken.type == Token.TokenAnd ||
                currentToken.type == Token.TokenOr ||
                currentToken.type == Token.TokenNon ||
                currentToken.type == Token.TokenKeyword)
            {
                output = LogicalOperation().ToString().ToLower();
            }
            else if (currentToken.type == Token.TokenStg)
            {
                output = currentToken.value;
                output = output.TrimStart('$');
                output = output.TrimEnd('$');
                Eat(Token.TokenStg);
            }
            else if (currentToken.type == Token.TokenId)
            {
                DataRow[] dr = Symbol.Lookup(currentToken.value);
                output = dr[0]["Value"].ToString();
                Eat(Token.TokenId);
            }
            return output;
        }

        public void Ask()
        {
            DataRow[] dr = Symbol.Lookup(currentToken.value);
            string variableType = dr[0]["Type"].ToString();
            string variableName = dr[0]["Name"].ToString();

            string labelText = string.Format("Please input \'{0}\' value for variable \'{1}\'.", variableType, variableName);
            string promptValue = Prompt.ShowDialog(labelText, "Input Value");

            if (promptValue.Length == 0)
            {
                Ask();
            }
            else
            {
                Symbol.EditSymbol(dr, promptValue);
                Eat(Token.TokenId);
            }
        }

        public void VariableDeclaration()
        {
            Symbol symbol;
            if (currentToken.type == Token.TokenInteger)
            {
                Eat(Token.TokenInteger);
                symbol = new Symbol(Token.TokenInteger, currentToken.value, null);
                Eat(Token.TokenId);
            }
            else if (currentToken.type == "BLN")
            {
                Eat("BLN");
                symbol = new Symbol("BLN", currentToken.value, null);
                Eat(Token.TokenId);
            }
            else if (currentToken.type == "STG")
            {
                Eat("STG");
                symbol = new Symbol("STG", currentToken.value, null);
                Eat(Token.TokenId);
            }
            else
            {
                Error();
            }
        }

        private int Term()
        {
            token = currentToken;
            if (token.type == Token.TokenInteger ||
                token.type == Token.TokenAdd ||
                token.type == Token.TokenSubtract ||
                token.type == Token.TokenId)
            {
                result = Factor();
            }
            else
            {
                while (token.type == Token.TokenMultiply ||
                  token.type == Token.TokenDivide ||
                  token.type == Token.TokenModulo)
                {
                    if (token.type == Token.TokenMultiply)
                    {
                        Eat(Token.TokenMultiply);
                        result = Factor();
                        result = result * Factor();
                    }
                    else if (token.type == Token.TokenDivide)
                    {
                        Eat(Token.TokenDivide);
                        result = Factor();
                        result = result / Factor();
                    }
                    else if (token.type == Token.TokenModulo)
                    {
                        Eat(Token.TokenModulo);
                        result = Factor();
                        result = result % Factor();
                    }
                }
            }
            return result;
        }

        private int Factor()
        {
            token = currentToken;
            if (token.type == Token.TokenInteger)
            {
                Eat(Token.TokenInteger);
                return int.Parse(token.value);
            }
            else if (token.type == Token.TokenId)
            {
                DataRow[] dr = Symbol.Lookup(currentToken.value);
                string symbolValue = dr[0]["Value"].ToString();
                Symbol.EditSymbol(dr, symbolValue);
                Eat(Token.TokenId);
                return int.Parse(symbolValue);
            }
            else if (token.type == Token.TokenAdd ||
               token.type == Token.TokenSubtract ||
               token.type == Token.TokenMultiply ||
               token.type == Token.TokenDivide ||
               token.type == Token.TokenModulo)
            {
                result = Expr();
                return result;
            }
            else
            {
                Error();
                return 0;
            }
        }

        public int Expr()
        {
            token = currentToken;
            if (token.type == Token.TokenMultiply ||
                token.type == Token.TokenDivide ||
                token.type == Token.TokenModulo ||
                token.type == Token.TokenInteger ||
                token.type == Token.TokenId)
            {
                result = Term();
            }
            else
            {
                while (token.type == Token.TokenAdd ||
                  token.type == Token.TokenSubtract)
                {
                    if (token.type == Token.TokenAdd)
                    {
                        Eat(Token.TokenAdd);
                        result = Term();
                        result = result + Term();
                    }
                    else if (token.type == Token.TokenSubtract)
                    {
                        Eat(Token.TokenSubtract);
                        result = Term();
                        result = result - Term();
                    }
                }
            }

            return result;
        }

        public bool NumericPredicate()
        {
            token = currentToken;
            int left;
            int right;
            if (token.type == Token.TokenGreaterThan)
            {
                Eat(Token.TokenGreaterThan);
                left = Expr();
                right = Expr();
                if (left > right)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (token.type == Token.TokenGreaterEqual)
            {
                Eat(Token.TokenGreaterEqual);
                left = Expr();
                right = Expr();
                if (left >= right)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (token.type == Token.TokenLessThan)
            {
                Eat(Token.TokenLessThan);
                left = Expr();
                right = Expr();
                if (left < right)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (token.type == Token.TokenLessEqual)
            {
                Eat(Token.TokenLessEqual);
                left = Expr();
                right = Expr();
                if (left <= right)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else  //EQL
            {
                Eat(Token.TokenEqual);

                left = Expr();
                right = Expr();
                if (left == right)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public bool LogicalOperation()
        {
            bool left;
            bool right;
            if (currentToken.type == Token.TokenAnd)
            {
                Eat(Token.TokenAnd);
                left = ParseLogical();
                right = ParseLogical();
                if (left && right)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (currentToken.type == Token.TokenOr)
            {
                Eat(Token.TokenOr);
                left = ParseLogical();
                right = ParseLogical();
                if (left || right)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (currentToken.type == Token.TokenNon)
            {
                Eat(Token.TokenNon);
                return !ParseLogical();
            }
            else
            {
                return ParseLogical();
            }
        }

        public bool ParseLogical()
        {
            bool result;
            if (currentToken.type == Token.TokenGreaterThan ||
            currentToken.type == Token.TokenGreaterEqual ||
            currentToken.type == Token.TokenLessThan ||
            currentToken.type == Token.TokenLessEqual ||
            currentToken.type == Token.TokenEqual)
            {
                result = NumericPredicate();
            }
            else if (currentToken.type == Token.TokenAnd ||
            currentToken.type == Token.TokenOr ||
            currentToken.type == Token.TokenNon)
            {
                result = LogicalOperation();
            }
            else if (currentToken.type == Token.TokenId)
            {
                DataRow[] dr = Symbol.Lookup(currentToken.value);
                string symbolValue = dr[0]["Value"].ToString();
                Symbol.EditSymbol(dr, symbolValue);
                result = Boolean.Parse(symbolValue);
                Eat(Token.TokenId);
            }
            else
            {
                result = Boolean.Parse(currentToken.value);
                Eat(Token.TokenKeyword);
            }
            return result;
        }

        public void AssignmentStatement()
        {
            string variableValue = null;
            Eat(Token.TokenPut);
            if (currentToken.type == Token.TokenAdd ||
                currentToken.type == Token.TokenSubtract ||
                currentToken.type == Token.TokenMultiply ||
                currentToken.type == Token.TokenDivide ||
                currentToken.type == Token.TokenModulo ||
                currentToken.type == Token.TokenInteger)
            {
                variableValue = Expr().ToString();
            }
            else if (currentToken.type == Token.TokenGreaterThan ||
                currentToken.type == Token.TokenGreaterEqual ||
                currentToken.type == Token.TokenLessThan ||
                currentToken.type == Token.TokenLessEqual ||
                currentToken.type == Token.TokenEqual)
            {
                variableValue = NumericPredicate().ToString().ToLower();
            }
            else if (currentToken.type == Token.TokenAnd ||
                currentToken.type == Token.TokenOr ||
                currentToken.type == Token.TokenNon)
            {
                variableValue = LogicalOperation().ToString().ToLower();
            }
            else if (currentToken.type == Token.TokenKeyword)
            {
                variableValue = currentToken.value;
                Eat(Token.TokenKeyword);
            }
            else
            {
                variableValue = currentToken.value;
                Eat(Token.TokenStg);
            }
            Eat(Token.TokenIn);
            DataRow[] dr = Symbol.Lookup(currentToken.value);
            Symbol.EditSymbol(dr, variableValue);
            Eat(Token.TokenId);
        }
    }

    public static class Prompt
    {
        public static string ShowDialog(string text, string caption)
        {
            Form prompt = new Form()
            {
                Width = 415,
                Height = 100,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                Text = caption,
                StartPosition = FormStartPosition.CenterScreen
            };
            Label textLabel = new Label() { Left = 10, Top = 10, Width = 300, Text = text };
            TextBox textBox = new TextBox() { Left = 10, Top = 30, Width = 300 };
            Button confirmation = new Button() { Text = "Submit", Left = 315, Top = 29, DialogResult = DialogResult.OK };
            confirmation.Click += (sender, e) => { prompt.Close(); };
            prompt.Controls.Add(textBox);
            prompt.Controls.Add(confirmation);
            prompt.Controls.Add(textLabel);
            prompt.AcceptButton = confirmation;

            return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
        }
    }
}