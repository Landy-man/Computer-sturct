using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assembler
{
    public class Assembler
    {
        private const int WORD_SIZE = 16;

        private Dictionary<string, int[]> m_dControl, m_dJmp; //these dictionaries map command mnemonics to machine code - they are initialized at the bottom of the class

        public Dictionary<string, int> symbols;
        private int varNum, labelNum; 

        public Assembler()
        {
            InitCommandDictionaries();
            initSymbols();
        }

        private void initSymbols()
        {
            symbols = new Dictionary<string, int>();
            symbols["SCREEN"] =(int) Math.Pow(2, 14);
            symbols["R0"] = 0;
            symbols["R1"] = 1;
            symbols["R2"] = 2;
            symbols["R3"] = 3;
            symbols["R4"] = 4;
            symbols["R5"] = 5;
            symbols["R6"] = 6;
            symbols["R7"] =7 ;
            symbols["R8"] =8 ;
            symbols["R9"] =9 ;
            symbols["R10"] =10 ;
            symbols["R11"] = 11;
            symbols["R12"] =12 ;
            symbols["R13"] =13 ;
            symbols["R14"] =14 ;
            symbols["R15"] = 15;
        }

        //this method is called from the outside to run the assembler translation
        public void TranslateAssemblyFile(string sInputAssemblyFile, string sOutputMachineCodeFile)
        {
            //read the raw input, including comments, errors, ...
            StreamReader sr = new StreamReader(sInputAssemblyFile);
            List<string> lLines = new List<string>();
           
            while (!sr.EndOfStream)
            {
                lLines.Add(sr.ReadLine());
            }
            sr.Close();
            //translate to machine code
            List<string> lTranslated = TranslateAssemblyFile(lLines);
            //write the output to the machine code file
            StreamWriter sw = new StreamWriter(sOutputMachineCodeFile);
            for (int i =0;i<lTranslated.Count;i++)
            {
                Console.WriteLine(lTranslated.ElementAt(i));
            }
            Console.Read();
            foreach (string sLine in lTranslated)
                sw.WriteLine(sLine);
            sw.Close();
            
        }

        //translate assembly into machine code
        private List<string> TranslateAssemblyFile(List<string> lLines)
        {
            //init data structures here 

            //expand the macros
            List<string> lAfterMacroExpansion = ExpendMacros(lLines);

            //first pass - create symbol table and remove lable lines
            List<string> lAfterFirstPass = FirstPass(lAfterMacroExpansion);

            //second pass - replace symbols with numbers, and translate to machine code
            List<string> lAfterSecondPass = SecondPass(lAfterFirstPass);
            return lAfterSecondPass;
        }

        
        //expand all macros
        private List<string> ExpendMacros(List<string> lLines)
        {
            List<string> lAfterExpansion = new List<string>();
            for (int i = 0; i < lLines.Count; i++)
            {
                //remove all redudant characters
                string sLine = CleanWhiteSpacesAndComments(lLines[i]);
                if (sLine == "")
                    continue;
                //if the line contains a macro, expand it, otherwise the line remains the same
                List<string> lExpanded = ExapndMacro(sLine);
                //we may get multiple lines from a macro expansion
                foreach (string sExpanded in lExpanded)
                {
                    lAfterExpansion.Add(sExpanded);
                }
            }
            return lAfterExpansion;
        }

        //expand a single macro line
        private List<string> ExapndMacro(string sLine)
        {
            List<string> lExpanded = new List<string>();
            string var = null;
            bool pushLine = false;
            if (IsCCommand(sLine))
            {
                string sDest, sCompute, sJmp;
                GetCommandParts(sLine, out sDest, out sCompute, out sJmp);
                if (sDest.Contains("["))
                {
                    int start=sDest.IndexOf("[")
                        , end=sDest.IndexOf("]");
                    var=sDest.Substring(start+1,end-start-1);
                    sDest = sDest.Replace("M["+var+"]","M");
                    pushLine = true;
                }
                if (sCompute.Contains("["))
                {
                    int start = sCompute.IndexOf("["),
                        end = sCompute.IndexOf("]");
                    var=sCompute.Substring(start+1, end-start-1);
                    sCompute = sCompute.Substring(0,start)+sCompute.Substring(end+1);
                    pushLine = true;
                }
                if (sJmp.Contains(":"))
                {
                    var = sJmp.Substring(4);
                    sJmp=sJmp.Substring(0,3);
                }
                
                if (var!=null)
                    lExpanded.Add("@"+var);
                if (sDest!="")
                    sDest=sDest+"=";
                if (sJmp!="")
                    sJmp=";" + sJmp;
                sLine = sDest + sCompute + sJmp;
                if (pushLine)
                {
                    lExpanded.Add("A=M");
                    pushLine = false;
                }
                lExpanded.Add(sLine);
            }
            if (lExpanded.Count == 0)
                lExpanded.Add(sLine);
            return lExpanded;
        }

        //first pass - record all symbols - labels and variables
        private List<string> FirstPass(List<string> lLines)
        {
            int cRealLines = 0;
            string sLine = "";
            bool dontAdd = false;
            List<string> lAfterPass = new List<string>();
            for (int i = 0; i < lLines.Count; i++)
            {
                sLine = lLines[i];
                if (IsLabelLine(sLine))
                {
                    if (symbols.ContainsKey(sLine.Substring(1, sLine.Length - 2)))
                    {
                        symbols[sLine.Substring(1, sLine.Length - 2)] = i  - labelNum;
                    }
                    else
                        symbols.Add(sLine.Substring(1, sLine.Length - 2), i - labelNum);
                   
                    dontAdd = true;
                    labelNum++;

                }
                else if (IsACommand(sLine))
                {
                    short ans;
                    bool result;
                    string first;
                    first = sLine.Substring(1, 1);
                    result = Int16.TryParse(first, out ans);
                    if(!result)
                    {
                        if (!symbols.ContainsKey(sLine.Substring(1)))
                        {
                            symbols.Add(sLine.Substring(1), varNum + 16);
                            varNum++;
                        }

                    }
                }
                else if (IsCCommand(sLine))
                {
                    //do nothing here
                }
                else
                    throw new FormatException("Cannot parse line " + i + ": " + lLines[i]);
                if(!dontAdd)
                {
                    lAfterPass.Add(sLine);

                }
                else
                {
                    dontAdd = false;
                }
                 cRealLines++;
            }
            return lAfterPass;
        }
        
        //second pass - translate lines into machine code, replaicng symbols with numbers
        private List<string> SecondPass(List<string> lLines)
        {
            string sLine = "";
            List<string> lAfterPass = new List<string>();
            for (int i = 0; i < lLines.Count; i++)
            {
                sLine = lLines[i];
                if (IsACommand(sLine))
                {
                    int symVal, ans;
                    string command;
                    command = sLine.Substring(1);
                    bool result = Int32.TryParse(command, out ans);
                    if(!result)
                    {
                        if(Equals(sLine.Substring(0,1),"(" ))
                            symVal=symbols[command.Substring(0,command.Length-1)];
                        else
                            symVal=symbols[command];
                        command=ToBinary(symVal);
                    }
                    else
                        command=ToBinary(ans);
                    lAfterPass.Add(command);
                }
                else if (IsCCommand(sLine))
                {
                    int[] control, jump;
                    string sDest, sControl, sJmp, code;
                    GetCommandParts(sLine, out sDest, out sControl, out sJmp);
                    code = "111";
                    control = m_dControl[sControl];
                    code = code + ToString(control);
                    if (sDest.Contains("A"))
                        code = code + "1";
                    else
                        code = code + "0";
                    if (sDest.Contains("D"))
                        code = code + "1";
                    else
                        code = code + "0";
                    if (sDest.Contains("M"))
                        code = code + "1";
                    else
                        code = code + "0";
                    jump = m_dJmp[sJmp];
                    code = code + this.ToString(jump);
                    lAfterPass.Add(code);

                    //translate an C command into a sequence of bits
                }
                else
                    throw new FormatException("Cannot parse line " + i + ": " + lLines[i]);
            }
            return lAfterPass;
        }

        //helper functions for translating numbers or bits into strings
        private string ToString(int[] aBits)
        {
            string sBinary = "";
            for (int i = 0; i < aBits.Length; i++)
                sBinary += aBits[i];
            return sBinary;
        }

        private string ToBinary(int x)
        {
            string sBinary = "";
            for (int i = 0; i < WORD_SIZE; i++)
            {
                sBinary = (x % 2) + sBinary;
                x = x / 2;
            }
            return sBinary;
        }


        //helper function for splitting the various fields of a C command
        private void GetCommandParts(string sLine, out string sDest, out string sControl, out string sJmp)
        {
            if (sLine.Contains('='))
            {
                int idx = sLine.IndexOf('=');
                sDest = sLine.Substring(0, idx);
                sLine = sLine.Substring(idx + 1);
            }
            else
                sDest = "";
            if (sLine.Contains(';'))
            {
                int idx = sLine.IndexOf(';');
                sControl = sLine.Substring(0, idx);
                sJmp = sLine.Substring(idx + 1);

            }
            else
            {
                sControl = sLine;
                sJmp = "";
            }
        }

        private bool IsCCommand(string sLine)
        {
            return !IsLabelLine(sLine) && sLine[0] != '@';
        }

        private bool IsACommand(string sLine)
        {
            return sLine[0] == '@';
        }

        private bool IsLabelLine(string sLine)
        {
            if (sLine.StartsWith("(") && sLine.EndsWith(")"))
                return true;
            return false;
        }

        private string CleanWhiteSpacesAndComments(string sDirty)
        {
            string sClean = "";
            for (int i = 0 ; i < sDirty.Length ; i++)
            {
                char c = sDirty[i];
                if (c == '/' && i < sDirty.Length - 1 && sDirty[i + 1] == '/') // this is a comment
                    return sClean;
                if (c > ' ' && c <= '~')//ignore white spaces
                    sClean += c;
            }
            return sClean;
        }


        private void InitCommandDictionaries()
        {
            m_dControl = new Dictionary<string, int[]>();

            m_dControl["0"] = new int[] { 0, 1, 0, 1, 0, 1, 0 };
            m_dControl["1"] = new int[] { 0, 1, 1, 1, 1, 1, 1 };
            m_dControl["-1"] = new int[] { 0, 1, 1, 1, 0, 1, 0 };
            m_dControl["D"] = new int[] { 0, 0, 0, 1, 1, 0, 0 };
            m_dControl["A"] = new int[] { 0, 1, 1, 0, 0, 0, 0 };
            m_dControl["!D"] = new int[] { 0, 0, 0, 1, 1, 0, 1 };
            m_dControl["!A"] = new int[] { 0, 1, 1, 0, 0, 0, 1 };
            m_dControl["-D"] = new int[] { 0, 0, 0, 1, 1, 1, 1 };
            m_dControl["-A"] = new int[] { 0, 1, 1, 0, 0,1, 1 };
            m_dControl["D+1"] = new int[] { 0, 0, 1, 1, 1, 1, 1 };
            m_dControl["A+1"] = new int[] { 0, 1, 1, 0, 1, 1, 1 };
            m_dControl["D-1"] = new int[] { 0, 0, 0, 1, 1, 1, 0 };
            m_dControl["A-1"] = new int[] { 1, 1, 0, 0, 0, 1, 0 };
            m_dControl["D+A"] = new int[] { 0, 0, 0, 0, 0, 1, 0 };
            m_dControl["D-A"] = new int[] { 0, 0, 1, 0, 0, 1, 1 };
            m_dControl["A-D"] = new int[] { 0, 0, 0, 0, 1,1, 1 };
            m_dControl["D&A"] = new int[] { 0, 0, 0, 0, 0, 0, 0 };
            m_dControl["D|A"] = new int[] { 0, 0, 1, 0,1, 0, 1 };

            m_dControl["M"] = new int[] { 1, 1, 1, 0, 0, 0, 0 };
            m_dControl["!M"] = new int[] { 1, 1, 1, 0, 0, 0, 1 };
            m_dControl["-M"] = new int[] { 1, 1, 1, 0, 0, 1, 1 };
            m_dControl["M+1"] = new int[] { 1, 1, 1, 0, 1, 1, 1 };
            m_dControl["M-1"] = new int[] { 1, 1, 1, 0, 0, 1, 0 };
            m_dControl["D+M"] = new int[] { 1, 0, 0, 0, 0, 1, 0 };
            m_dControl["D-M"] = new int[] { 1, 0, 1, 0, 0, 1, 1 };
            m_dControl["M-D"] = new int[] { 1, 0, 0, 0, 1, 1, 1 };
            m_dControl["D&M"] = new int[] { 1, 0, 0, 0, 0, 0, 0 };
            m_dControl["D|M"] = new int[] { 1, 0, 1, 0, 1, 0, 1 };


            m_dJmp = new Dictionary<string, int[]>();

            m_dJmp[""] = new int[] { 0, 0, 0 };
            m_dJmp["JGT"] = new int[] { 0, 0, 1 };
            m_dJmp["JEQ"] = new int[] { 0, 1, 0 };
            m_dJmp["JGE"] = new int[] { 0, 1, 1 };
            m_dJmp["JLT"] = new int[] { 1, 0, 0 };
            m_dJmp["JNE"] = new int[] { 1, 0, 1 };
            m_dJmp["JLE"] = new int[] { 1, 1, 0 };
            m_dJmp["JMP"] = new int[] { 1, 1, 1 };
        }
    }
}
