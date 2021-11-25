using Compiler.IO;
using Compiler.Tokenization;
using System.Collections.Generic;
using static Compiler.Tokenization.TokenType;

namespace Compiler.SyntacticAnalysis
{
    /// <summary>
    /// A recursive descent parser
    /// </summary>
    public class Parser
    {
        /// <summary>
        /// The error reporter
        /// </summary>
        public ErrorReporter Reporter { get; }

        /// <summary>
        /// The tokens to be parsed
        /// </summary>
        private List<Token> tokens;

        /// <summary>
        /// The index of the current token in tokens
        /// </summary>
        private int currentIndex;

        /// <summary>
        /// The current token
        /// </summary>
        private Token CurrentToken { get { return tokens[currentIndex]; } }

        /// <summary>
        /// Advances the current token to the next one to be parsed
        /// </summary>
        private void MoveNext()
        {
            if (currentIndex < tokens.Count - 1)
                currentIndex += 1;
        }

        /// <summary>
        /// Creates a new parser
        /// </summary>
        /// <param name="reporter">The error reporter to use</param>
        public Parser(ErrorReporter reporter)
        {
            Reporter = reporter;
        }

        /// <summary>
        /// Checks the current token is the expected kind and moves to the next token
        /// </summary>
        /// <param name="expectedType">The expected token type</param>
        private void Accept(TokenType expectedType)
        {
            if (CurrentToken.Type == expectedType)
            {
                Debugger.Write($"Accepted {CurrentToken}");
                MoveNext();
            }
            else
            {
                Reporter.ReportError(CurrentToken.Position, $"Unexpected token '{CurrentToken.Spelling}'");
            }
        }

        /// <summary>
        /// Parses a program
        /// </summary>
        /// <param name="tokens">The tokens to parse</param>
        public void Parse(List<Token> tokens)
        {
            this.tokens = tokens;
            ParseProgram();
            if (currentIndex < tokens.Count - 1)
                Reporter.ReportError(CurrentToken.Position, "Program parsed but tokens remain");
        }



        /// <summary>
        /// Parses a program
        /// </summary>
        private void ParseProgram()
        {
            Debugger.Write("Parsing program");
            ParseSingleCommand();
        }



        /// <summary>
        /// Parses a command
        /// </summary>
        private void ParseCommand()
        {
            Debugger.Write("Parsing command");
            ParseSingleCommand();
            while (CurrentToken.Type == Comma)
            {
                Accept(Comma);
                ParseSingleCommand();
            }
        }

        /// <summary>
        /// Parses a single command
        /// </summary>
        private void ParseSingleCommand()
        {
            Debugger.Write("Parsing Single Command");
            switch (CurrentToken.Type)
            {
                // There are missing cases here - you'll need to fill them all in
                case Nothing:
                    ParseBlankCommand();
                    break;
                case Identifier:
                    ParseAssignmentOrCallCommand();
                    break;
                case Begin:
                    ParseBeginCommand();
                    break;
                case Let:
                    ParseLetCommand();
                    break;
                case If:
                    ParseIfCommand();
                    break;
                case While:
                    ParseWhileCommand();
                    break;
                default:
                    Reporter.ReportError(CurrentToken.Position, $"Unexpected token type {CurrentToken.Type}");
                    break;
            }
        }

        /// <summary>
        /// Parses a blank command
        /// </summary>
        private void ParseBlankCommand()
        {
            Debugger.Write("Parsing Blank Command");
            Accept(Nothing);
        }

        /// <summary>
        /// Parses an assignment or call command
        /// </summary>
        private void ParseAssignmentOrCallCommand()
        {
            Debugger.Write("Parsing Assignment Command or Call Command");
            ParseIdentifier();
            if (CurrentToken.Type == LeftBracket)
            {
                Debugger.Write("Parsing Call Command");
                Accept(LeftBracket);
                ParseParameter();
                Accept(RightBracket);
            }
            else if (CurrentToken.Type == Becomes)
            {
                Debugger.Write("Parsing Assignment Command");
                Accept(Becomes);
                ParseExpression();
            }
            else
            {
                Reporter.ReportError(CurrentToken.Position, $"Unexpected token type {CurrentToken.Type}");
            }
        }

        /// <summary>
        /// Parses a begin command
        /// </summary>
        private void ParseBeginCommand()
        {
            Debugger.Write("Parsing Begin Command");
            Accept(Begin);
            ParseCommand();
            Accept(End);
        }

        /// <summary>
        /// Parses a while command
        /// </summary>
        private void ParseWhileCommand()
        {
            Debugger.Write("Parsing While Command");
            Accept(While);
            if (CurrentToken.Type == Forever)
            {
                Debugger.Write("Parsing Forever Command");
                Accept(Forever);
                Accept(Do);
                ParseSingleCommand();
            }
            else
            {
                ParseExpression();
                Accept(Do);
                ParseSingleCommand();
            }
        }

        /// <summary>
        /// Parses an if command
        /// </summary>
        private void ParseIfCommand()
        {
            Debugger.Write("Parsing If Command");
            Accept(If);
            ParseExpression();
            Accept(Then);
            ParseSingleCommand();
            Accept(Else);
            ParseSingleCommand();
        }

        /// <summary>
        /// Parses a let command
        /// </summary>
        private void ParseLetCommand()
        {
            Debugger.Write("Parsing Let Command");
            Accept(Let);
            ParseDeclaration();
            Accept(In);
            ParseSingleCommand();
        }



        /// <summary>
        /// Parses a parameter
        /// </summary>
        private void ParseParameter()
        {
            Debugger.Write("Parsing Parameter");
            switch (CurrentToken.Type)
            {
                case RightBracket:
                    // Empty parameter list
                    break;
                case Var:
                    ParseVarParameter();
                    break;
                default:
                    ParseValueParameter();
                    break;
            }
        }

        /// <summary>
        /// Parses a value parameter
        /// </summary>
        private void ParseValueParameter()
        {
            Debugger.Write("Parsing Value Parameter");
            ParseExpression();
        }

        /// <summary>
        /// Parses a variable parameter
        /// </summary>
        private void ParseVarParameter()
        {
            Debugger.Write("Parsing Variable Parameter");
            Accept(Var);
            ParseIdentifier();
        }



        /// <summary>
        /// Parses an expression
        /// </summary>
        private void ParseExpression()
        {
            Debugger.Write("Parsing Expression");
            ParsePrimaryExpression();
            while (CurrentToken.Type == Operator)
            {
                ParseOperator();
                ParsePrimaryExpression();
            }
        }

        /// <summary>
        /// Parses a primary expression
        /// </summary>
        private void ParsePrimaryExpression()
        {
            Debugger.Write("Parsing Primary Expression");
            switch (CurrentToken.Type)
            {
                case IntLiteral:
                    ParseIntExpression();
                    break;
                case CharLiteral:
                    ParseCharExpression();
                    break;
                case Identifier:
                    ParseIdOrCallExpression();
                    break;
                case Operator:
                    ParseUnaryExpression();
                    break;
                case LeftBracket:
                    ParseBracketExpression();
                    break;
                default:
                    Reporter.ReportError(CurrentToken.Position, $"Token type mismatch: {CurrentToken.Type}");
                    break;
            }
        }

        /// <summary>
        /// Parses an int expression
        /// </summary>
        private void ParseIntExpression()
        {
            Debugger.Write("Parsing Int Expression");
            ParseIntegerLiteral();
        }

        /// <summary>
        /// Parses a char expression
        /// </summary>
        private void ParseCharExpression()
        {
            Debugger.Write("Parsing Char Expression");
            ParseCharacterLiteral();
        }

        /// <summary>
        /// Parses an ID expression or call expression
        /// </summary>
        private void ParseIdOrCallExpression()
        {
            Debugger.Write("Parsing Call Expression or Identifier Expression");
            ParseIdentifier();
            if (CurrentToken.Type == LeftBracket)
            {
                Debugger.Write("Parsing Call Expression");
                Accept(LeftBracket);
                ParseParameter();
                Accept(LeftBracket);
            }
        }

        /// <summary>
        /// Parses a unary expresion
        /// </summary>
        private void ParseUnaryExpression()
        {
            Debugger.Write("Parsing Unary Expression");
            ParseOperator();
            ParsePrimaryExpression();
        }

        /// <summary>
        /// Parses a bracket expression
        /// </summary>
        private void ParseBracketExpression()
        {
            Debugger.Write("Parsing Bracket Expression");
            Accept(LeftBracket);
            ParseExpression();
            Accept(RightBracket);
        }



        /// <summary>
        /// Parses a declaration
        /// </summary>
        private void ParseDeclaration()
        {
            Debugger.Write("Parsing Declaration");
            ParseSingleDeclaration();
            while (CurrentToken.Type == Comma)
            {
                Accept(Comma);
                ParseSingleDeclaration();
            }
        }

        /// <summary>
        /// Parses a single declaration
        /// </summary>
        private void ParseSingleDeclaration()
        {
            Debugger.Write("Parsing Single Declaration");
            ParseIdentifier();
            switch (CurrentToken.Type)
            {
                case Is:
                    ParseConstDeclaration();
                    break;
                case Colon:
                    ParseVarDeclaration();
                    break;
                default:
                    Reporter.ReportError(CurrentToken.Position, $"Unexpected token type {CurrentToken.Type}");
                    break;
            }
        }

        /// <summary>
        /// Parses a constant declaration
        /// </summary>
        private void ParseConstDeclaration()
        {
            Debugger.Write("Parsing Constant Declaration");
            Accept(Is);
            ParseExpression();
        }

        /// <summary>
        /// Parses a variable declaration
        /// </summary>
        private void ParseVarDeclaration()
        {
            Debugger.Write("Parsing Variable Declaration");
            Accept(Colon);
            ParseTypeDenoter();
        }



        /// <summary>
        /// Parses a type denoter
        /// </summary>
        private void ParseTypeDenoter()
        {
            Debugger.Write("Parsing Type Denoter");
            ParseIdentifier();
        }



        /// <summary>
        /// Parses an integer literal
        /// </summary>
        private void ParseIntegerLiteral()
        {
            Debugger.Write("Parsing integer literal");
            Accept(IntLiteral);
        }

        /// <summary>
        /// Parses a character literal
        /// </summary>
        private void ParseCharacterLiteral()
        {
            Debugger.Write("Parsing character literal");
            Accept(CharLiteral);
        }

        /// <summary>
        /// Parses an identifier
        /// </summary>
        private void ParseIdentifier()
        {
            Debugger.Write("Parsing identifier");
            Accept(Identifier);
        }

        /// <summary>
        /// Parses an operator
        /// </summary>
        private void ParseOperator()
        {
            Debugger.Write("Parsing operator");
            Accept(Operator);
        }
    }
}