using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    class MultiBitAdder : Gate
    {
        public int Size { get; private set; }
        public WireSet Input1 { get; private set; }
        public WireSet Input2 { get; private set; }
        public WireSet Output { get; private set; }
        public Wire Overflow { get; private set; }
        private FullAdder[] FA;


        public MultiBitAdder(int iSize)
        {
            Size = iSize;
            Input1 = new WireSet(Size);
            Input2 = new WireSet(Size);
            Output = new WireSet(Size);
            //your code here
            FA = new FullAdder[iSize];
            Overflow = new Wire();
            FA[0] = new FullAdder();
            FA[0].ConnectInput1(Input1[0]);
            FA[0].ConnectInput2(Input2[0]);
            FA[0].CarryInput.Value = 0;
            Output[0].ConnectInput(FA[0].Output);
            for (int i=1; i<iSize; i++)
            {
                FA[i] = new FullAdder();
                FA[i].ConnectInput1(Input1[i]);
                FA[i].ConnectInput2(Input2[i]);
                FA[i].CarryInput.ConnectInput(FA[i-1].CarryOutput) ;
                Output[i].ConnectInput(FA[i].Output);
            }
            Overflow.ConnectInput(FA[Size - 1].CarryOutput);

        }

        public override string ToString()
        {
            return Input1 + "(" + Input1.Get2sComplement() + ")" + " + " + Input2 + "(" + Input2.Get2sComplement() + ")" + " = " + Output + "(" + Output.Get2sComplement() + ")";
        }

        public void ConnectInput1(WireSet wInput)
        {
            Input1.ConnectInput(wInput);
        }
        public void ConnectInput2(WireSet wInput)
        {
            Input2.ConnectInput(wInput);
        }


        public override bool TestGate()
        {
            Input1[0].Value = 0; Input1[1].Value = 0; Input1[2].Value = 0;
            Input2[0].Value = 0; Input2[1].Value = 0; Input2[2].Value = 0;
            if ((Output[0].Value != 0) || (Output[1].Value != 0) || (Output[2].Value != 0)||(Overflow.Value!=0))
                return false;
            Input1[0].Value = 1; Input1[1].Value = 0; Input1[2].Value = 1;
            Input2[0].Value = 0; Input2[1].Value = 0; Input2[2].Value = 0;
            if ((Output[0].Value != 1) || (Output[1].Value != 0) || (Output[2].Value != 1) || (Overflow.Value != 0))
                return false;
            Input1[0].Value = 1; Input1[1].Value = 0; Input1[2].Value = 1;
            Input2[0].Value = 1; Input2[1].Value = 1; Input2[2].Value = 0;
            if ((Output[0].Value != 0) || (Output[1].Value != 0) || (Output[2].Value != 0) || (Overflow.Value != 1))
                return false;
            Input1[0].Value = 1; Input1[1].Value = 0; Input1[2].Value = 1;
            Input2[0].Value = 0; Input2[1].Value = 0; Input2[2].Value = 1;
            if ((Output[0].Value != 1) || (Output[1].Value != 0) || (Output[2].Value != 0) || (Overflow.Value != 1))
                return false;
            Input1[0].Value = 0; Input1[1].Value = 1; Input1[2].Value = 1;
            Input2[0].Value = 0; Input2[1].Value = 1; Input2[2].Value = 0;
            if ((Output[0].Value != 0) || (Output[1].Value != 0) || (Output[2].Value != 0) || (Overflow.Value != 1))
                return false;
            Input1[0].Value = 1; Input1[1].Value = 1; Input1[2].Value = 1;
            Input2[0].Value = 1; Input2[1].Value = 1; Input2[2].Value = 1;
            if ((Output[0].Value != 0) || (Output[1].Value != 1) || (Output[2].Value != 1) || (Overflow.Value != 1))
                return false;
            return true;
        }
    }
}
