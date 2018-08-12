using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    class BitwiseAndGate : BitwiseTwoInputGate
    {
        private AndGate[] BitwiseAnd;

        public BitwiseAndGate(int iSize)
            : base(iSize)
        {
            BitwiseAnd = new AndGate[iSize];
            for (int i=0; i<iSize; i++)
            {
                BitwiseAnd[i] = new AndGate();
                BitwiseAnd[i].ConnectInput1(Input1[i]);
                BitwiseAnd[i].ConnectInput2(Input2[i]);
                Output[i].ConnectInput (BitwiseAnd[i].Output);
            }
        }


        public override string ToString()
        {
            return "And " + Input1 + ", " + Input2 + " -> " + Output;
        }

        public override bool TestGate()
        {

            Input1[0].Value = 0; Input1[1].Value = 1;
            Input2[0].Value = 0; Input2[1].Value = 0;
            if ((Output[0].Value != 0) || (Output[1].Value != 0))
                return false;

            Input1[0].Value = 1; Input1[1].Value = 0;
            Input2[0].Value = 0; Input2[1].Value = 0;
            if ((Output[0].Value != 0) || (Output[1].Value != 0))
                return false;

            Input1[0].Value = 1; Input1[1].Value = 1;
            Input2[0].Value = 1; Input2[1].Value = 1;
            if ((Output[0].Value != 1) || (Output[1].Value != 1))
                return false;

            Input1[0].Value = 1; Input1[1].Value = 0;
            Input2[0].Value = 1; Input2[1].Value = 0;
            if ((Output[0].Value != 1) || (Output[1].Value != 0))
                return false;

            Input1[0].Value = 1; Input1[1].Value = 0;
            Input2[0].Value = 1; Input2[1].Value = 1;
            if ((Output[0].Value != 1) || (Output[1].Value != 0))
                return false;

            Input1[0].Value=0; Input1[1].Value=0;
            Input2[0].Value = 0; Input2[1].Value = 0;
            if ((Output[0].Value != 0) || (Output[1].Value != 0))
                return false;

            Input1[0].Value = 0; Input1[1].Value = 1;
            Input2[0].Value = 1; Input2[1].Value = 0;
            if ((Output[0].Value != 0) || (Output[1].Value != 0))
                return false;

            Input1[0].Value = 0; Input1[1].Value = 1;
            Input2[0].Value = 0; Input2[1].Value = 1;
            if ((Output[0].Value != 0) || (Output[1].Value != 1))
                return false;

            Input1[0].Value = 0; Input1[1].Value = 1;
            Input2[0].Value = 1; Input2[1].Value = 1;
            if ((Output[0].Value != 0) || (Output[1].Value != 1))
                return false;



            return true;
        }
    }
}
