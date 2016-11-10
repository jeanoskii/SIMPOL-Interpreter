using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            /*
            int ctr = 0;
            string line;

            StreamReader sr = new StreamReader(txtFile.Text);
            while ((line = sr.ReadLine()) != null)
            {
                txtOutput.Text += line;
                ctr++;
            }
            sr.Close();
            */
            if (!string.IsNullOrWhiteSpace(txtFile.Text))
            {
                Token.tokenStream = "";
                Symbol.symbolStream = "";
                txtOutput.Clear();
                StreamReader sr = new StreamReader(txtFile.Text);
                string result = sr.ReadToEnd();
                Lexer lexer = new Lexer(result);
                Interpreter interpreter = new Interpreter(lexer);
                result = "Sentence: " + result;
                result += "\r\nResult: " + interpreter.Program().ToString();
                result += "\r\nToken Stream:" + Token.tokenStream;
                result += "\r\nSymbol Stream:" + Symbol.symbolStream;
                txtOutput.Text = result.ToString();
                sr.Close();
            }
        }
    }

    public class Token
    {
        public const string TokenInteger = "INT";
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
            { "NON", new Token("NON", "NON") }
        };
        public static string tokenStream { get; set; }
        public string type { get; set; }
        public string value { get; set; }
        public Token(string type, string value)
        {
            this.type = type;
            this.value = value;
        }
        public string TokenString(string type, string value)
        {
            return "\r\nToken(" + type + ", " + value + ")";
        }
    }

    public class Symbol
    {
        public string name { get; set; }
        public string type { get; set; }
        public string value { get; set; }
        public static string symbolStream { get; set; }
        public Symbol(string type, string name, string value)
        {
            this.type = type;
            this.name = name;
            this.value = value;
        //    dt.Columns.Add("Type");
        //    dt.Columns.Add("Name");
        //    dt.Columns.Add("Value");
        //    dt.Rows.Add(new string[] { type, name, value });
        }
        //public DataTable GetSymbolTable()
        //{
        //    return dt;
        //}
        public string SymbolString(string type, string name, string value)
        {
            return "\r\nSymbol(" + type + ", " + name + ", " + value + ")";
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

        private void Advance()
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

        private char Peek()
        {
            int peekPosition = position + 1;
            if (peekPosition > text.Length - 1)
            {
                return '\0';
            }
            else
            {
                return text[peekPosition];
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
                    Token.tokenStream += token.TokenString(token.type, token.value);
                    return token;
                }
                else if (char.IsLetter(currentChar))
                {
                    token = Identifier();
                    Token.tokenStream += token.TokenString(token.type, token.value);
                    if (token.type == Token.TokenId)
                    {
                        Symbol symbol = new Symbol(token.type, token.value, null);
                        Symbol.symbolStream += symbol.SymbolString(token.type, token.value, "null");
                    }
                    return token;
                }
                else if (currentChar == '(')
                {
                    token = new Token(Token.TokenLParen, currentChar.ToString());
                    Token.tokenStream += token.TokenString(token.type, token.value);
                    Advance();
                    return token;
                }
                else if (currentChar == ')')
                {
                    token = new Token(Token.TokenRParen, currentChar.ToString());
                    Token.tokenStream += token.TokenString(token.type, token.value);
                    Advance();
                    return token;
                }
                else if (currentChar == '{')
                {
                    token = new Token(Token.TokenLBrace, currentChar.ToString());
                    Token.tokenStream += token.TokenString(token.type, token.value);
                    Advance();
                    return token;
                }
                else if (currentChar == '}')
                {
                    token = new Token(Token.TokenRBrace, currentChar.ToString());
                    Token.tokenStream += token.TokenString(token.type, token.value);
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

        private int Term()
        {
            token = currentToken;
            if (token.type == Token.TokenInteger ||
                token.type == Token.TokenAdd ||
                token.type == Token.TokenSubtract)
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
             else if (token.type == Token.TokenLParen)
             {
                 Eat(Token.TokenLParen);
                 result = Expr();
                 Eat(Token.TokenRParen);
                 return result;
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
                token.type == Token.TokenInteger)
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
            int leftExpr;
            int rightExpr;
            if (token.type == Token.TokenGreaterThan)
            {
                Eat(Token.TokenGreaterThan);
                leftExpr = Expr();
                rightExpr = Expr();
                if (leftExpr > rightExpr)
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
                leftExpr = Expr();
                rightExpr = Expr();
                if (leftExpr >= rightExpr)
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
                leftExpr = Expr();
                rightExpr = Expr();
                if (leftExpr < rightExpr)
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
                leftExpr = Expr();
                rightExpr = Expr();
                if (leftExpr <= rightExpr)
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
                leftExpr = Expr();
                rightExpr = Expr();
                if (leftExpr == rightExpr)
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
            token = currentToken;
            bool left;
            bool right;
            if (token.type == Token.TokenAnd)
            {
                Eat(Token.TokenAnd);
                left = NumericPredicate();
                right = NumericPredicate();
                if (left == right)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else if (token.type == Token.TokenOr)
            {
                Eat(Token.TokenOr);
                left = NumericPredicate();
                right = NumericPredicate();
                if ((left == true) || (right == true))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                Eat(Token.TokenNon);
                left = NumericPredicate();
                return !left;
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
                result += LogicalOperation().ToString();
                result += "\r\n";
            }
            Eat(Token.TokenRBrace);
            return result;
        }

        public void VariableDeclaration()
        {
            token = currentToken;
            if (token.type == "INT")
            {
                Eat(Token.TokenInteger);
                Eat(Token.TokenId);
            }
            else if (token.type == "BLN")
            {
                Eat("BLN");
                Eat(Token.TokenId);
            }
            else if (token.type == "STG")
            {
                Eat("STG");
                Eat(Token.TokenId);
            }
            else
            {
                Error();
            }
        }

        public void DataType()
        {
            //if INT
            //else if STG
            //else if BLN
            //else return Error
        }

        public void AssignmentStatement()
        {
            //PUT <expr> IN [variable name]
            int variableValue;
            string variableName;
            token = currentToken;
            Eat(Token.TokenPut);
            variableValue = Expr();
            Eat(Token.TokenIn);
            variableName = Variable();
        }

        public string Variable()
        {
            token = currentToken;
            Eat(Token.TokenId);
            return token.value;
        }
    }
}