using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SimpleCompiler
{
    class Program
    {
        static void Test1()
        {
            Console.WriteLine("test1");
            Compiler c = new Compiler();
            string s = "let x = 5;";
            Stack<Token> sTokens = c.Tokenize(s);
            string[] aTokens = new string[] { "let", "x", "=", "5", ";" };
            List<Token> answer = new List<Token>();
            for (int i = 0; i < aTokens.Length; i++)
            {
                Token sToken = sTokens.Pop();
                answer.Add(sToken);
                bool print = false;
                if (sToken.ToString() != aTokens[i])
                    print = true;
                if (print) Console.WriteLine("tokens should be: " + aTokens + " but your answer is: " + answer);
            }
            sTokens = c.Tokenize(s);
            AssignmentStatement assignment = c.Parse(sTokens);
            if (assignment.ToString() != s)
                Console.WriteLine("BUGBUG");

            List<AssignmentStatement> lSimple = c.SimplifyExpressions(assignment);
            if (lSimple.Count != 1 || lSimple[0].ToString() != assignment.ToString())
                Console.WriteLine("BUGBUG");

            //List<string> lAssembly = c.GenerateCode(lSimple);
            c.Compile(s);
        }

        static void Test2()
        {
            Console.WriteLine("test2");
            Compiler c = new Compiler();
            string s = "let x = (+ (+ x 5) (- y z));";
            Stack<Token> sTokens = c.Tokenize(s);
            string[] aTokens = new string[] { "let", "x", "=", "(", "+", "(", "+", "x", "5", ")", "(", "-", "y", "z", ")", ")", ";" };
            for (int i = 0; i < aTokens.Length; i++)
            {
                Token sToken = sTokens.Pop();
                if (sToken.ToString() != aTokens[i])
                    Console.WriteLine("BUGBUG");
            }
            sTokens = c.Tokenize(s);
            AssignmentStatement assignment = c.Parse(sTokens);
            if (assignment.ToString() != s)
                Console.WriteLine("BUGBUG");
            List<AssignmentStatement> lSimple = c.SimplifyExpressions(assignment);
            string[] aSimple = new string[] { "let _3 = (+ x 5);", "let _4 = (- y z);", "let x = (+ _3 _4);" };
            for (int i = 0; i < aSimple.Length; i++)
                if (lSimple[i].ToString() != aSimple[i])
                    Console.WriteLine("BUGBUG");
            c.Compile(s);
        }

        static void Test3()
        {
            Console.WriteLine("test3");
            Compiler c = new Compiler();
            List<string> lExpressions = new List<string>();
            lExpressions.Add("let x = (+ (- 53 12) (- 467 3));");
            lExpressions.Add("let y = 3;");
            lExpressions.Add("let z = (+ (- x 12) (+ y 3));");
            c.Compile(lExpressions);
        }


        static void TestCompile()
        {
            Compiler c = new Compiler();
            string wrongToken;
            Console.WriteLine("");

            string s = "let x == 1;";

            Stack<Token> sTokens = c.Tokenize(s);
            wrongToken = "="; bool answerWasWrong; bool testPerfect;
            Console.WriteLine("testing: " + s);
            answerWasWrong = true; testPerfect = true;
            try
            {
                sTokens = c.Tokenize(s);
                AssignmentStatement assignment = c.Parse(sTokens);
            }
            catch (SyntaxErrorException e)
            {
                answerWasWrong = false;
                if (e.Token == null)
                {
                    Console.WriteLine("you throwed an Exception as needed, but your Token is NULL");
                    testPerfect = false;
                }
                else
                    if (e.Token.Name != wrongToken)
                    {
                        Console.WriteLine("token in Exception should be " + wrongToken + " but you sent: " + e.Token.Name);
                        testPerfect = false;
                    }
            }
            if (answerWasWrong) Console.WriteLine("you didnot throw Exception on: " + wrongToken);
            else if (testPerfect) Console.WriteLine("good work, this test was perfect");

            //________________________________________________________________________________________________
            //------------------------------------------------------------------------------------------------
            Console.WriteLine("");

            s = "let x = = 2;;";
            wrongToken = "=";
            Console.WriteLine("testing: " + s);
            answerWasWrong = true; testPerfect = true;

            try
            {
                sTokens = c.Tokenize(s);

                AssignmentStatement assignment = c.Parse(sTokens);
            }
            catch (SyntaxErrorException e)
            {
                answerWasWrong = false;
                if (e.Token == null)
                {
                    Console.WriteLine("you throwed an Exception as needed, but your Token is NULL");
                    testPerfect = false;
                }
                else

                    if (e.Token.Name != wrongToken)
                    {
                        Console.WriteLine("token in Exception should be " + wrongToken + " but you sent: " + e.Token.Name);
                        testPerfect = false;
                    }
            }
            if (answerWasWrong) Console.WriteLine("you didnot throw Exception on: " + wrongToken);
            else if (testPerfect) Console.WriteLine("good work, this test was perfect");


            //________________________________________________________________________________________________
            //------------------------------------------------------------------------------------------------

            Console.WriteLine("");

            s = "let 1 = 3;";
            wrongToken = "1";
            Console.WriteLine("testing: " + s);
            answerWasWrong = true; testPerfect = true;
            try
            {
                sTokens = c.Tokenize(s);

                AssignmentStatement assignment = c.Parse(sTokens);
            }
            catch (SyntaxErrorException e)
            {
                answerWasWrong = false;
                if (e.Token == null)
                {
                    Console.WriteLine("you throwed an Exception as needed, but your Token is NULL");
                    testPerfect = false;
                }
                else

                    if (e.Token.Name != wrongToken)
                    {
                        Console.WriteLine("token in Exception should be " + wrongToken + " but you sent: " + e.Token.Name);
                        testPerfect = false;
                    }
            }
            if (answerWasWrong) Console.WriteLine("you didnot throw Exception on: " + wrongToken);
            else if (testPerfect) Console.WriteLine("good work, this test was perfect");

            //________________________________________________________________________________________________
            //------------------------------------------------------------------------------------------------
            Console.WriteLine("");

            s = "let x = 4;;";
            wrongToken = ";";
            Console.WriteLine("testing: " + s);
            answerWasWrong = true; testPerfect = true;
            try
            {
                sTokens = c.Tokenize(s);

                AssignmentStatement assignment = c.Parse(sTokens);
            }
            catch (SyntaxErrorException e)
            {
                answerWasWrong = false;
                if (e.Token == null)
                {
                    Console.WriteLine("you throwed an Exception as needed, but your Token is NULL");
                    testPerfect = false;
                }
                else

                    if (e.Token.Name != wrongToken)
                    {
                        Console.WriteLine("token in Exception should be " + wrongToken + " but you sent: " + e.Token.Name);
                        testPerfect = false;
                    }
            }
            if (answerWasWrong) Console.WriteLine("you didnot throw Exception on: " + wrongToken);
            else if (testPerfect) Console.WriteLine("good work, this test was perfect");
            //________________________________________________________________________________________________
            //------------------------------------------------------------------------------------------------
            Console.WriteLine("");

            s = "let let x = 5;";
            wrongToken = "let";
            Console.WriteLine("testing: " + s);
            answerWasWrong = true; testPerfect = true;
            try
            {
                sTokens = c.Tokenize(s);

                AssignmentStatement assignment = c.Parse(sTokens);
            }
            catch (SyntaxErrorException e)
            {
                answerWasWrong = false;
                if (e.Token == null)
                {
                    Console.WriteLine("you throwed an Exception as needed, but your Token is NULL");
                    testPerfect = false;
                }
                else

                    if (e.Token.Name != wrongToken)
                    {
                        Console.WriteLine("token in Exception should be " + wrongToken + " but you sent: " + e.Token.Name);
                        testPerfect = false;
                    }
            }
            if (answerWasWrong) Console.WriteLine("you didnot throw Exception on: " + wrongToken);
            else if (testPerfect) Console.WriteLine("good work, this test was perfect");
            //________________________________________________________________________________________________
            //------------------------------------------------------------------------------------------------
            Console.WriteLine("");

            s = "let 1BadName = 5;";
            wrongToken = "1BadName";
            Console.WriteLine("testing: " + s);
            answerWasWrong = true; testPerfect = true;
            try
            {
                sTokens = c.Tokenize(s);

                AssignmentStatement assignment = c.Parse(sTokens);
            }
            catch (SyntaxErrorException e)
            {
                answerWasWrong = false;
                if (e.Token == null)
                {
                    Console.WriteLine("you throwed an Exception as needed, but your Token is NULL");
                    testPerfect = false;
                }
                else

                    if (e.Token.Name != wrongToken)
                    {
                        Console.WriteLine("token in Exception should be " + wrongToken + " but you sent: " + e.Token.Name);
                        testPerfect = false;
                    }
            }
            if (answerWasWrong) Console.WriteLine("you didnot throw Exception on: " + wrongToken);
            else if (testPerfect) Console.WriteLine("good work, this test was perfect");
            //________________________________________________________________________________________________
            //------------------------------------------------------------------------------------------------
            Console.WriteLine("");

            s = "let x = ( (+ x 5) (- y z));";
            wrongToken = "(";
            Console.WriteLine("testing: " + s);
            answerWasWrong = true; testPerfect = true;
            try
            {
                sTokens = c.Tokenize(s);

                AssignmentStatement assignment = c.Parse(sTokens);
            }
            catch (SyntaxErrorException e)
            {
                answerWasWrong = false;
                if (e.Token == null)
                {
                    Console.WriteLine("you throwed an Exception as needed, but your Token is NULL");
                    testPerfect = false;
                }
                else

                    if (e.Token.Name != wrongToken)
                    {
                        Console.WriteLine("token in Exception should be " + wrongToken + " but you sent: " + e.Token.Name);
                        testPerfect = false;
                    }
            }
            if (answerWasWrong) Console.WriteLine("you didnot throw Exception on: " + wrongToken);
            else if (testPerfect) Console.WriteLine("good work, this test was perfect");
            //________________________________________________________________________________________________
            //------------------------------------------------------------------------------------------------
            Console.WriteLine("");

            s = "let x = (+ (- (+ x 5) (- y z));";
            wrongToken = "(";
            Console.WriteLine("testing: " + s);
            answerWasWrong = true; testPerfect = true;
            try
            {
                sTokens = c.Tokenize(s);

                AssignmentStatement assignment = c.Parse(sTokens);
            }
            catch (SyntaxErrorException e)
            {
                answerWasWrong = false;
                if (e.Token == null)
                {
                    Console.WriteLine("you throwed an Exception as needed, but your Token is NULL");
                    testPerfect = false;
                }
                else

                    if (e.Token.Name != wrongToken)
                    {
                        Console.WriteLine("token in Exception should be " + wrongToken + " but you sent: " + e.Token.Name);
                        testPerfect = false;
                    }
            }
            if (answerWasWrong) Console.WriteLine("you didnot catch that there are more '(' then ')'");
            else if (testPerfect) Console.WriteLine("good work, this test was perfect");

            //________________________________________________________________________________________________
            //------------------------------------------------------------------------------------------------
            Console.WriteLine("");
            s = "let x = (+ (* x 5) (- y z));";
            wrongToken = "*";
            Console.WriteLine("testing: " + s);
            answerWasWrong = true; testPerfect = true;
            try
            {
                sTokens = c.Tokenize(s);

                AssignmentStatement assignment = c.Parse(sTokens);
            }
            catch (SyntaxErrorException e)
            {
                answerWasWrong = false;
                if (e.Token == null)
                {
                    Console.WriteLine("you throwed an Exception as needed, but your Token is NULL");
                    testPerfect = false;
                }
                else

                    if (e.Token.Name != wrongToken)
                    {
                        Console.WriteLine("token in Exception should be " + wrongToken + " but you sent: " + e.Token.Name);
                        testPerfect = false;
                    }
            }
            if (answerWasWrong) Console.WriteLine("you didnot throw Exception on: " + wrongToken);
            else if (testPerfect) Console.WriteLine("good work, this test was perfect");
            //________________________________________________________________________________________________
            //------------------------------------------------------------------------------------------------
            Console.WriteLine("");
            s = "let x = (+ 5 y z);";
            wrongToken = "z";
            Console.WriteLine("testing: " + s);
            answerWasWrong = true; testPerfect = true;
            try
            {
                sTokens = c.Tokenize(s);

                AssignmentStatement assignment = c.Parse(sTokens);
            }
            catch (SyntaxErrorException e)
            {
                answerWasWrong = false;
                if (e.Token == null)
                {
                    Console.WriteLine("you throwed an Exception as needed, but your Token is NULL");
                    testPerfect = false;
                }
                else

                    if (e.Token.Name != wrongToken)
                    {
                        Console.WriteLine("token in Exception should be " + wrongToken + " but you sent: " + e.Token.Name);
                        testPerfect = false;
                    }
            }
            if (answerWasWrong) Console.WriteLine("you didnot throw Exception on: " + wrongToken);
            else if (testPerfect) Console.WriteLine("good work, this test was perfect");

            //________________________________________________________________________________________________
            //------------------------------------------------------------------------------------------------
            Console.WriteLine("");
            s = "let x = ( 5 + z );";
            wrongToken = "5";
            Console.WriteLine("testing: " + s);
            answerWasWrong = true; testPerfect = true;
            try
            {
                sTokens = c.Tokenize(s);

                AssignmentStatement assignment = c.Parse(sTokens);
            }
            catch (SyntaxErrorException e)
            {
                answerWasWrong = false;
                if (e.Token == null)
                {
                    Console.WriteLine("you throwed an Exception as needed, but your Token is NULL");
                    testPerfect = false;
                }
                else

                    if (e.Token.Name != wrongToken)
                    {
                        Console.WriteLine("token in Exception should be " + wrongToken + " but you sent: " + e.Token.Name);
                        testPerfect = false;
                    }
            }
            if (answerWasWrong) Console.WriteLine("you didnot throw Exception on: " + wrongToken);
            else if (testPerfect) Console.WriteLine("good work, this test was perfect");
            //________________________________________________________________________________________________
            //------------------------------------------------------------------------------------------------
            Console.WriteLine("");
            s = "let x = (+ (; x 5) (- y z));";
            wrongToken = ";";
            Console.WriteLine("testing: " + s);
            answerWasWrong = true; testPerfect = true;
            try
            {
                sTokens = c.Tokenize(s);

                AssignmentStatement assignment = c.Parse(sTokens);
            }
            catch (SyntaxErrorException e)
            {
                answerWasWrong = false;
                if (e.Token == null)
                {
                    Console.WriteLine("you throwed an Exception as needed, but your Token is NULL");
                    testPerfect = false;
                }
                else

                    if (e.Token.Name != wrongToken)
                    {
                        Console.WriteLine("token in Exception should be " + wrongToken + " but you sent: " + e.Token.Name);
                        testPerfect = false;
                    }
            }
            if (answerWasWrong) Console.WriteLine("you didnot throw Exception on: " + wrongToken);
            else if (testPerfect) Console.WriteLine("good work, this test was perfect");
            //________________________________________________________________________________________________
            //------------------------------------------------------------------------------------------------
            Console.WriteLine("");
            s = "let x = (+ (+ x 5); (- y z));";
            wrongToken = ";";
            Console.WriteLine("testing: " + s);
            answerWasWrong = true; testPerfect = true;
            try
            {
                sTokens = c.Tokenize(s);

                AssignmentStatement assignment = c.Parse(sTokens);
            }
            catch (SyntaxErrorException e)
            {
                answerWasWrong = false;
                if (e.Token == null)
                {
                    Console.WriteLine("you throwed an Exception as needed, but your Token is NULL");
                    testPerfect = false;
                }
                else

                    if (e.Token.Name != wrongToken)
                    {
                        Console.WriteLine("token in Exception should be " + wrongToken + " but you sent: " + e.Token.Name);
                        testPerfect = false;
                    }
            }
            if (answerWasWrong) Console.WriteLine("you didnot throw Exception on: " + wrongToken);
            else if (testPerfect) Console.WriteLine("good work, this test was perfect");
            //________________________________________________________________________________________________
            //------------------------------------------------------------------------------------------------
            Console.WriteLine("");
            s = "x = (+ (+ x 5) (- y z));";
            wrongToken = "x";
            Console.WriteLine("testing: " + s);
            answerWasWrong = true; testPerfect = true;
            try
            {
                sTokens = c.Tokenize(s);

                AssignmentStatement assignment = c.Parse(sTokens);
            }
            catch (SyntaxErrorException e)
            {
                answerWasWrong = false;
                if (e.Token == null)
                {
                    Console.WriteLine("you throwed an Exception as needed, but your Token is NULL");
                    testPerfect = false;
                }
                else

                    if (e.Token.Name != wrongToken)
                    {
                        Console.WriteLine("token in Exception should be " + wrongToken + " but you sent: " + e.Token.Name);
                        testPerfect = false;
                    }
            }
            if (answerWasWrong) Console.WriteLine("you didnot throw Exception on: " + wrongToken);
            else if (testPerfect) Console.WriteLine("good work, this test was perfect");
            //________________________________________________________________________________________________
            //------------------------------------------------------------------------------------------------
            Console.WriteLine("");
            s = "let x = (+ (+ x 5) WOW (- y z));";
            wrongToken = ")";
            Console.WriteLine("testing: " + s);
            answerWasWrong = true; testPerfect = true;
            try
            {
                sTokens = c.Tokenize(s);

                AssignmentStatement assignment = c.Parse(sTokens);
            }
            catch (SyntaxErrorException e)
            {
                answerWasWrong = false;
                if (e.Token == null)
                {
                    Console.WriteLine("you throwed an Exception as needed, but your Token is NULL");
                    testPerfect = false;
                }
                else

                    if (e.Token.Name != wrongToken)
                    {
                        Console.WriteLine("token in Exception should be " + wrongToken + " but you sent: " + e.Token.Name);
                        testPerfect = false;
                    }
            }
            if (answerWasWrong) Console.WriteLine("you didnot throw Exception on: " + wrongToken);
            else if (testPerfect) Console.WriteLine("good work, this test was perfect");
            //________________________________________________________________________________________________
            //------------------------------------------------------------------------------------------------
            Console.WriteLine("");
            s = "let x = (+(-(+ (+ x 5) (- y z));";
            wrongToken = "(";
            Console.WriteLine("testing: " + s);
            answerWasWrong = true; testPerfect = true;
            try
            {
                sTokens = c.Tokenize(s);

                AssignmentStatement assignment = c.Parse(sTokens);
            }
            catch (SyntaxErrorException e)
            {
                answerWasWrong = false;
                if (e.Token == null)
                {
                    Console.WriteLine("you throwed an Exception as needed, but your Token is NULL");
                    testPerfect = false;
                }
                else

                    if (e.Token.Name != wrongToken)
                    {
                        Console.WriteLine("token in Exception should be " + wrongToken + " but you sent: " + e.Token.Name);
                        testPerfect = false;
                    }
            }
            if (answerWasWrong) Console.WriteLine("you didnot throw Exception on: " + wrongToken);
            else if (testPerfect) Console.WriteLine("good work, this test was perfect");
            //________________________________________________________________________________________________
            //------------------------------------------------------------------------------------------------
            Console.WriteLine("");
            s = "let x = (+ (+ x 5)-)+) (- y z));";
            wrongToken = ")";
            Console.WriteLine("testing: " + s);
            answerWasWrong = true; testPerfect = true;
            try
            {
                sTokens = c.Tokenize(s);

                AssignmentStatement assignment = c.Parse(sTokens);
            }
            catch (SyntaxErrorException e)
            {
                answerWasWrong = false;
                if (e.Token == null)
                {
                    Console.WriteLine("you throwed an Exception as needed, but your Token is NULL");
                    testPerfect = false;
                }
                else

                    if (e.Token.Name != wrongToken)
                    {
                        Console.WriteLine("token in Exception should be " + wrongToken + " but you sent: " + e.Token.Name);
                        testPerfect = false;
                    }
            }
            if (answerWasWrong) Console.WriteLine("you didnot throw Exception on: " + wrongToken);
            else if (testPerfect) Console.WriteLine("good work, this test was perfect");


        }

        static void TestGenerate()
        {
            Console.WriteLine("");
            Compiler c = new Compiler();
            List<string> lExpressions = new List<string>();
            Console.WriteLine("testing: let x = y;");
            lExpressions.Add("let x = y;");
            try
            {
                c.Compile(lExpressions);
                Console.WriteLine("test failed, you didnot throwed an Exception");
            }
            catch (SyntaxErrorException e)
            {
                Console.WriteLine("you throwed Exception as needed, just make sure it came from Generate-code and not before");
                Console.WriteLine("your Exception message is: " + e.Message + "\n it should be: symbol was not declared");
            }

            //________________________________________________________________________________________________
            //------------------------------------------------------------------------------------------------
            Console.WriteLine("");
            Compiler c1 = new Compiler();
            List<string> l1Expressions = new List<string>();
            string s = "let z = (+ (- x 12) (+ y 3));";
            Console.WriteLine("testing: " + s);
            l1Expressions.Add("s");
            try
            {
                c1.Compile(lExpressions);
                Console.WriteLine("test failed, you didnot throwed an Exception");
            }
            catch (SyntaxErrorException e)
            {
                Console.WriteLine("you throwed Exception as needed, just make sure it came from Generate-code and not before");
                Console.WriteLine("your Exception message is: " + e.Message + "\n it should be: symbol was not declared");
            }

            //________________________________________________________________________________________________
            //------------------------------------------------------------------------------------------------
            Console.WriteLine("");
            Compiler c2 = new Compiler();
            List<string> l2Expressions = new List<string>();
            s = "let x = x;";
            Console.WriteLine("testing: " + s);
            l2Expressions.Add("s");
            try
            {
                c2.Compile(lExpressions);
                Console.WriteLine("test failed, you didnot throwed an Exception");
            }
            catch (SyntaxErrorException e)
            {
                Console.WriteLine("you throwed Exception as needed, just make sure it came from Generate-code and not before");
                Console.WriteLine("your Exception message is: " + e.Message + "\n it should be: symbol was not declared");
            }

            //________________________________________________________________________________________________
            //------------------------------------------------------------------------------------------------
            Console.WriteLine("");
            Compiler c3 = new Compiler();
            List<string> l3Expressions = new List<string>();
            s = "let x = (+ x 3);";
            Console.WriteLine("testing: " + s);
            l3Expressions.Add("s");
            try
            {
                c3.Compile(lExpressions);
                Console.WriteLine("test failed, you didnot throwed an Exception");
            }
            catch (SyntaxErrorException e)
            {
                Console.WriteLine("you throwed Exception as needed, just make sure it came from Generate-code and not before");
                Console.WriteLine("your Exception message is: " + e.Message + "\n it should be: symbol was not declared");
            }

            //________________________________________________________________________________________________
            //------------------------------------------------------------------------------------------------

            /*           Compiler c = new Compiler();
                       List<string> lExpressions = new List<string>();
                       lExpressions.Add("let x = (+ (- 53 12) (- 467 3));");
                       lExpressions.Add("let y = 3;");
                   lExpressions.Add("let z = (+ (- x 12) (+ y 3));");
                   */
        }
        static void Main(string[] args)
        {
            //       Test1();
            //              Test2();
            //              Test3();

            TestCompile();
            Console.WriteLine("--------------------------------- \n lets test Generate Code");
            TestGenerate();
            Console.ReadLine();

        }
    }
}