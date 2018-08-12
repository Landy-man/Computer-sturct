using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SimpleCompiler
{
    public class Compiler
    {
        //Add class members here (e.g. symbol table). 
        public int currentLine;
        public Dictionary<string, int> symbolTable;
        public int locals;
        public int tempLocals;

        public Compiler()
        {
            symbolTable = new Dictionary<string, int>();
            locals = 0;
            tempLocals = 0;
            currentLine = 0;
        }

        //This method is designed to be the only public method of the class. It launches the compilation process.
        //The rest of the methods are public just to make the debugging easier.
        public List<string> Compile(List<string> lLines)
        {

            List<string> lCompiledCode = new List<string>();
            foreach (string sExpression in lLines)
            {
                List<string> lAssembly = Compile(sExpression);
                lCompiledCode.Add("// " + sExpression);
                lCompiledCode.AddRange(lAssembly);
                currentLine++;
            }
            return lCompiledCode;
        }

        //Compile a single line containing only an assignment of the form "let <var> = <expression>;"
        public List<string> Compile(string sAssignment)
        {
            //Tokenize the string into a stack of tokens
            Stack<Token> sTokens = Tokenize(sAssignment);
            //Parse the tokens into objects representing the meaning of the statement
            AssignmentStatement s = Parse(sTokens);
            //Simplify complex expressions in order for the code generation to be simpler
            List<AssignmentStatement> lSimpleAssignments = SimplifyExpressions(s);
            //Compute the symbol table here
            ComputeSymbolTable(lSimpleAssignments);
            //Generate the actual code
            List<string> lAssembly = GenerateCode(lSimpleAssignments);
            return lAssembly;
        }

        //Computes the symbol table.
        //For each variable we keep its index (offset from LOCAL).
        //No need to keep type (we use inly int), and kind (we use only local variables).
        private void ComputeSymbolTable(List<AssignmentStatement> lSimpleAssignments)
        {
            for(int i=0; i<lSimpleAssignments.Count;i++)
            {
                string varName = lSimpleAssignments[i].Variable.Name;
                if (!symbolTable.ContainsKey(varName))
                {
                    symbolTable.Add(varName, locals);
                    locals++;
                }
            }
            
        }

        //Generates assembly code for a simple assignment statement. Can accept only the following:
        //let <var> = <number>; e.g. let x = 5;
        //let <var> = <var>; e.g. let x = y;
        //let <var> = (<operator> <operand1> <operand2>); where operand1 and operand2 can only by either numbers or variables, but not nested expressions. e.g. let x = (- y 5);
        public List<string> GenerateCode(AssignmentStatement aSimple)
        {
            List<string> AssemblyList = new List<string>();
            if (aSimple.Value is NumericExpression)
                AssemblyList=isNumaricAssigment(AssemblyList,aSimple);
            else if (aSimple.Value is VariableExpression)
            {
                VariableExpression var =(VariableExpression)aSimple.Value;
                if (!symbolTable.ContainsKey(var.Name))
                    throw new SyntaxErrorException("Undefined variable, please define the variable " + var.Name + " first.", new Token());
                else
                    AssemblyList = isVariableAssigment(AssemblyList, aSimple);
            }
            else
            {
                BinaryOperationExpression temp = new BinaryOperationExpression();
                temp = (BinaryOperationExpression)aSimple.Value;                
                if (temp.Operand1 is NumericExpression)
                {
                    AssemblyList.Add("@" + ((NumericExpression)temp.Operand1).Value);
                    AssemblyList.Add("D = A");
                    AssemblyList.Add("@OPERAND1");
                    AssemblyList.Add(" M = D");
                }
                else
                {
                    AssemblyList.Add("@" + symbolTable[((VariableExpression)temp.Operand1).Name]);
                    AssemblyList.Add("D = A");
                    AssemblyList.Add("@LOCAL");
                    AssemblyList.Add("D = D + M");
                    AssemblyList.Add("A = D");
                    AssemblyList.Add("D = M");
                    AssemblyList.Add("@OPERAND1");
                    AssemblyList.Add("M = D");
                }
                if (temp.Operand2 is NumericExpression)
                {
                    AssemblyList.Add("@" + ((NumericExpression)temp.Operand2).Value);
                    AssemblyList.Add("D = A");
                    AssemblyList.Add("@OPERAND2");
                    AssemblyList.Add("M = D");
                }
                else
                {
                    AssemblyList.Add("@" + symbolTable[((VariableExpression)temp.Operand2).Name]);
                    AssemblyList.Add("D = A");
                    AssemblyList.Add("@LOCAL");
                    AssemblyList.Add("D = D + M");
                    AssemblyList.Add("A = D");
                    AssemblyList.Add("D = M");
                    AssemblyList.Add("@OPERAND2");
                    AssemblyList.Add("M = D");
                }
                AssemblyList.Add("@OPERAND1");
                AssemblyList.Add(" D = M");
                AssemblyList.Add("@OPERAND2");
                if (temp.Operator == "+")
                    AssemblyList.Add("D = D + M");
                else
                    AssemblyList.Add("D = D - M");
                AssemblyList.Add("@RESULT");
                AssemblyList.Add("M = D");

                computeVariabletoR0(AssemblyList, aSimple.Variable.Name);
                updateDestinationWithResult(AssemblyList);
            }


            return AssemblyList;
        }

        private List<string> isVariableAssigment(List<string> AssemblyList, AssignmentStatement aSimple)
        {
            AssemblyList.Add("@" + symbolTable[((VariableExpression)aSimple.Value).Name]);
            AssemblyList.Add("D = A");
            AssemblyList.Add("@LOCAL");
            AssemblyList.Add("A = D + M");
            AssemblyList.Add("D = M");
            AssemblyList.Add("@RESULT");
            AssemblyList.Add("M = D");
            computeVariabletoR0(AssemblyList, ((VariableExpression)aSimple.Variable).Name);
            updateDestinationWithResult(AssemblyList);
            return AssemblyList;

        }

        private List<string> isNumaricAssigment(List<string>AssemblyList,AssignmentStatement aSimple)
        {
            NumericExpression Num = (NumericExpression)(aSimple.Value);
            int value = Num.Value;
            AssemblyList.Add("@" + value);
            AssemblyList.Add("D = A");
            AssemblyList.Add("@RETURN");
            AssemblyList.Add("M = D");
            computeVariabletoR0(AssemblyList, aSimple.Variable.Name);
            updateDestinationWithResult(AssemblyList);
            return AssemblyList;
        }

        private void computeVariabletoR0(List<string> AssemblyList, string p)
        {
            AssemblyList.Add("@" + symbolTable[p]);
            AssemblyList.Add("D = A");
            AssemblyList.Add("@LOCAL");
            AssemblyList.Add("D = D + M");
            AssemblyList.Add("@R0");
            AssemblyList.Add("M = D");
        }

        private void updateDestinationWithResult(List<string> AssemblyList)
        {
            AssemblyList.Add("@RESULT");
            AssemblyList.Add("D = M");
            AssemblyList.Add("@R0");
            AssemblyList.Add("A = M");
            AssemblyList.Add("M = D");
        }



        //Generates assembly code for a list of simple assignment statements
        public List<string> GenerateCode(List<AssignmentStatement> lSimpleAssignments)
        {
            List<string> lAssembly = new List<string>();
            foreach (AssignmentStatement aSimple in lSimpleAssignments)
                lAssembly.AddRange(GenerateCode(aSimple));
            return lAssembly;
        }

        //Simplify an expression by creating artificial local variables, and using them for intermidiate computations.
        //For example, let x = (+ (- y 2) (- 5 z)); will be simplified into:
        //let _1 = (- y 2);
        //let _2 = (- 5 z);
        //let x = (+ _1 _2);
        public List<AssignmentStatement> SimplifyExpressions(AssignmentStatement s)
        {
            //your code here
            List<AssignmentStatement> ListToReturn= new List<AssignmentStatement>();
            if ((!(s.Value is BinaryOperationExpression)) || (!(((BinaryOperationExpression)s.Value).Operand1 is BinaryOperationExpression) &&
                    !(((BinaryOperationExpression)s.Value).Operand2 is BinaryOperationExpression)))
                ListToReturn.Add(s);
            else
            {
                SplitTheExp(ref ListToReturn, (BinaryOperationExpression)s.Value);
                VariableExpression temp= new VariableExpression();
                temp.Name = "_" + tempLocals;
                s.Value = temp;
                ListToReturn.Add(s);
            }
            return ListToReturn;
        }

        private void SplitTheExp(ref List<AssignmentStatement> l, BinaryOperationExpression binaryOp)
        {
            string replaceLeft = "", replaceRight = "";
            if (binaryOp.Operand1 is BinaryOperationExpression)
            {
                SplitTheExp(ref l, (BinaryOperationExpression)binaryOp.Operand1);
                replaceLeft = ((AssignmentStatement)l.Last()).Variable.Name;
            }
            if (binaryOp.Operand2 is BinaryOperationExpression)
            {
                SplitTheExp(ref l, (BinaryOperationExpression)binaryOp.Operand2);
                replaceRight = ((AssignmentStatement)l.Last()).Variable.Name;
            }
            tempLocals++;
            string s = "_" + tempLocals;
            VariableExpression var = new VariableExpression();
            var.Name = s;
            AssignmentStatement toAdd = new AssignmentStatement();
            toAdd.Variable = var;
            if (replaceLeft == "" && replaceRight == "")
                toAdd.Value = binaryOp;
            else
            {
                BinaryOperationExpression change = new BinaryOperationExpression();
                change.Operator = binaryOp.Operator;
                if (replaceLeft != "")
                {
                    VariableExpression var2 = new VariableExpression();
                    var2.Name = replaceLeft;
                    change.Operand1 = var2;
                }
                else
                    change.Operand1 = binaryOp.Operand1;
                if (replaceRight != "")
                {
                    VariableExpression var2 = new VariableExpression();
                    var2.Name = replaceRight;
                    change.Operand2 = var2;
                }
                else
                    change.Operand2 = binaryOp.Operand1;
                toAdd.Value = change;
            }
            l.Add(toAdd);
        }


        //Tokenizes a string into tokens. Possible token delimiters are white spaces and also ;()+-
        //Tokens are pushed on the stack in reversed order, that is, the first token is pushed last.
        public Stack<Token> Tokenize(string sExpression)
        {
            string[] splitExpression = Regex.Split(sExpression, @"\s+|(?<=[;()+-])|(?=[;()+-])");
            Stack<Token> StackToReturn = new Stack<Token>();
            int position = sExpression.Length - 1;
            for (int i = splitExpression.Length - 1; i >= 0; i--)
            {
                if (!splitExpression[i].Equals(""))
                {
                    Token currentToken = new Token();
                    currentToken.Name = splitExpression[i];
                    currentToken.Position = sExpression.LastIndexOf(splitExpression[i]);
                    sExpression = sExpression.Substring(0, sExpression.LastIndexOf(splitExpression[i]));
                    currentToken.Line = currentLine;
                    int num;
                    if (Int32.TryParse(splitExpression[i], out num))
                        currentToken.Type = Token.TokenType.Number;
                    else if (splitExpression[i].Equals("-") | splitExpression[i].Equals("+"))
                        currentToken.Type = Token.TokenType.Operator;
                    else if (splitExpression[i].Equals("(") | splitExpression[i].Equals(")") | splitExpression[i].Equals(";") | splitExpression[i].Equals("=") | splitExpression[i].Equals(",") | splitExpression[i].Equals("[") | splitExpression[i].Equals("]"))
                    {
                        currentToken.Type = Token.TokenType.Symbol;
                    }
                    else if (splitExpression[i].Equals("let"))
                        currentToken.Type = Token.TokenType.Keyword;
                    else
                        currentToken.Type = Token.TokenType.ID;
                    StackToReturn.Push(currentToken);
                }
            }

            return StackToReturn;



        }

        //Parses a stack of tokens, containing a single assignment statement. 
        //The structure must be "let <var> = <expression>;" where expression can be of an arbitrary complexity, i.e., any complex expression is allowed.
        //Parsing must detect syntax problems (e.g. "let" or "=" are missing, opened parantheses are not closed, sentence does not end with a ;, and so forth).
        //When syntax errors are detected, a SyntaxErrorException must be thrown, with an appropriate message explaining the problem.
        public AssignmentStatement Parse(Stack<Token> sTokens)
        {
            Token popedToken = new Token();
            popedToken = sTokens.Pop();
            AssignmentStatement StatmentToReturn = new AssignmentStatement();
            VariableExpression variableWorkingOn = new VariableExpression(),
                SecondVariable = new VariableExpression();
            NumericExpression num = new NumericExpression();
            if (popedToken.Type != Token.TokenType.Keyword)
                throw new SyntaxErrorException("We accept only LL(0) type, Unrecogmized KeyWord", popedToken);
            popedToken = sTokens.Pop();
            if ((popedToken.Type == Token.TokenType.ID) && (popedToken.Name.ElementAt(0) >= '0') && (popedToken.Name.ElementAt(0) <= '9'))
                throw new SyntaxErrorException("All variables can not start with a number", popedToken);
            else if (popedToken.Name.Contains("?") || popedToken.Name.Contains("#") || popedToken.Name.Contains("@"))
                throw new SyntaxErrorException("Variable can not contain the '?,#,@' signs", popedToken);
            else if ((popedToken.Type != Token.TokenType.ID))
                throw new SyntaxErrorException("Expecting a variable name", popedToken);
            else
            {
                variableWorkingOn.Name = popedToken.Name;
                StatmentToReturn.Variable = variableWorkingOn;
            }
            popedToken = sTokens.Pop();
            if (!popedToken.Name.Equals("="))
                throw new SyntaxErrorException("Expecting a '=' sign", popedToken);
            popedToken = sTokens.Pop();
            if (Char.IsDigit(popedToken.Name, 0))
            {
                bool success = false;
                success = Int32.TryParse(popedToken.Name, out num.Value);
                if (!success)
                    throw new SyntaxErrorException("Invalid number", popedToken);
                StatmentToReturn.Value = num;
            }
            else if (Char.IsLetter(popedToken.Name, 0))
            {
                SecondVariable.Name = popedToken.Name;
                StatmentToReturn.Value = SecondVariable;
            }
            else if (popedToken.Name == "(")
            {
                StatmentToReturn.Value = BinaryExpressionCreate(new BinaryOperationExpression(), ref sTokens);
                if ((sTokens.Count > 0))
                {
                    if (sTokens.Pop().Name != ")")
                        throw new SyntaxErrorException("Missing ')' symbol", popedToken);
                }
            }
            else
                throw new SyntaxErrorException("Invalid value", popedToken);
            if (sTokens.Count > 1)
                throw new SyntaxErrorException("Invalid value", popedToken);
            if (sTokens.Count == 0 || (sTokens.Count == 1 && (sTokens.Pop().Name != ";")))
                throw new SyntaxErrorException("Expecting ';' at the end of the statement", popedToken);
            if (sTokens.Count > 0)
                throw new SyntaxErrorException("No code after ';'", popedToken);
            return StatmentToReturn;
        }

        private Expression BinaryExpressionCreate(BinaryOperationExpression exp, ref Stack<Token> sTokens)
        {
            if (sTokens.Peek().Name == "-" || sTokens.Peek().Name == "+")
                exp.Operator = sTokens.Pop().Name;
            else
                throw new SyntaxErrorException("Operator '+' or '-' expected", sTokens.Pop());
            if (sTokens.Peek().Name == "(")
            {
                sTokens.Pop();
                exp.Operand1 = BinaryExpressionCreate(new BinaryOperationExpression(), ref sTokens);
                if (sTokens.Pop().Name != ")")
                    throw new SyntaxErrorException("Missing ')' symbol", sTokens.Pop());
            }
            else
            {
                if (Char.IsDigit(sTokens.Peek().Name, 0))
                {
                    bool success = false;
                    NumericExpression num = new NumericExpression();
                    success = Int32.TryParse(sTokens.Pop().Name, out num.Value);
                    if (!success)
                        throw new SyntaxErrorException("Invalid number", sTokens.Pop());
                    exp.Operand1 = num;
                }
                else if (Char.IsLetter(sTokens.Peek().Name, 0))
                {
                    VariableExpression var2 = new VariableExpression();
                    var2.Name = sTokens.Pop().Name;
                    exp.Operand1 = var2;
                }
                else
                    throw new SyntaxErrorException("Invalid variable or const", sTokens.Pop());
            }
            if (sTokens.Peek().Name == "(")
            {
                sTokens.Pop();
                exp.Operand2 = BinaryExpressionCreate(new BinaryOperationExpression(), ref sTokens);
                if (sTokens.Pop().Name != ")")
                    throw new SyntaxErrorException("Missing ')' symbol", sTokens.Pop());
            }
            else
            {
                if (Char.IsDigit(sTokens.Peek().Name, 0))
                {
                    bool success = false;
                    NumericExpression num = new NumericExpression();
                    success = Int32.TryParse(sTokens.Pop().Name, out num.Value);
                    if (!success)
                        throw new SyntaxErrorException("Invalid number", sTokens.Pop());
                    exp.Operand2 = num;
                }
                else if (Char.IsLetter(sTokens.Peek().Name, 0))
                {
                    VariableExpression var2 = new VariableExpression();
                    var2.Name = sTokens.Pop().Name;
                    exp.Operand2 = var2;
                }
                else
                    throw new SyntaxErrorException("Invalid variable or const", sTokens.Pop());
            }
            return exp;
        }
    }
}




        
