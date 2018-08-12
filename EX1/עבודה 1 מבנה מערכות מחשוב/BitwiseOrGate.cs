using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    class BitwiseOrGate : BitwiseTwoInputGate
    {
        //your code here
        private OrGate[] or;


        public BitwiseOrGate(int iSize)
            : base(iSize)
        {
            //your code here
            or = new OrGate[iSize];
            for (int i = 0; i < iSize; i++)
            {
                or[i] = new OrGate();
                or[i].ConnectInput1(Input1[i]);
                or[i].ConnectInput2(Input2[i]);
                Output[i].ConnectInput(or[i].Output);
            }
        }


        public override string ToString()
        {
            return "Or " + Input1 + ", " + Input2 + " -> " + Output;
        }

        public override bool TestGate()
        {
            Input1[0].Value = 0; Input1[1].Value = 0;
            Input2[0].Value = 0; Input2[1].Value = 0;
            if ((Output[0].Value != 0) || (Output[1].Value != 0))
                return false;
            Input1[0].Value = 1; Input1[1].Value = 0;
            Input2[0].Value = 0; Input2[1].Value = 0;
            if ((Output[0].Value != 1) || (Output[1].Value != 0))
                return false;
            Input1[0].Value = 1; Input1[1].Value = 0;
            Input2[0].Value = 1; Input2[1].Value = 0;
            if ((Output[0].Value != 1) || (Output[1].Value != 0))
                return false;
            Input1[0].Value = 1; Input1[1].Value = 0;
            Input2[0].Value = 1; Input2[1].Value = 1;
            if ((Output[0].Value != 1) || (Output[1].Value != 1))
                return false;
            Input1[0].Value = 0; Input1[1].Value = 1;
            Input2[0].Value = 0; Input2[1].Value = 0;
            if ((Output[0].Value != 0) || (Output[1].Value != 1))
                return false;
            Input1[0].Value = 0; Input1[1].Value = 1;
            Input2[0].Value = 1; Input2[1].Value = 0;
            if ((Output[0].Value != 1) || (Output[1].Value != 1))
                return false;
            Input1[0].Value = 0; Input1[1].Value = 1;
            Input2[0].Value = 0; Input2[1].Value = 1;
            if ((Output[0].Value != 0) || (Output[1].Value != 1))
                return false;
            Input1[0].Value = 0; Input1[1].Value = 1;
            Input2[0].Value = 1; Input2[1].Value = 1;
            if ((Output[0].Value != 1) || (Output[1].Value != 1))
                return false;
            Input1[0].Value = 1; Input1[1].Value = 1;
            Input2[0].Value = 1; Input2[1].Value = 1;
            if ((Output[0].Value != 1) || (Output[1].Value != 1))
                return false;
            return true;
        }
    }
}
