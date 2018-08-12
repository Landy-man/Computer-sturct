using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    class BitwiseNotGate : Gate
    {
        public WireSet Input { get; private set; }
        public WireSet Output { get; private set; }
        public int Size { get; private set; }

        //your code here
        private NotGate[] not; 


        public BitwiseNotGate(int iSize)
        {
            Size = iSize;
            Input = new WireSet(Size);
            Output = new WireSet(Size);
             //your code here
            not = new NotGate[Size];
            for (int i = 0; i < Size; i++)
            {
                not[i] = new NotGate();
                not[i].ConnectInput(Input[i]);
                Output[i].ConnectInput(not[i].Output);
            }
        }

        public void ConnectInput(WireSet ws)
        {
            Input.ConnectInput(ws);
        }


        public override string ToString()
        {
            return "Not " + Input + " -> " + Output;
        }

        public override bool TestGate()
        {
                        Size = 3;
            Input[0].Value = 0; Input[1].Value = 0; Input[2].Value = 0;
            if ((Output[0].Value != 1) || (Output[1].Value != 1) || (Output[2].Value != 1))
                return false;
            Input[0].Value = 1; Input[1].Value = 0; Input[2].Value = 0;
            if ((Output[0].Value != 0) || (Output[1].Value != 1) || (Output[2].Value != 1))
                return false;
            Input[0].Value = 0; Input[1].Value = 1; Input[2].Value = 0;
            if ((Output[0].Value != 1) || (Output[1].Value != 0) || (Output[2].Value != 1))
                return false;
            Input[0].Value = 0; Input[1].Value = 0; Input[2].Value = 1;
            if ((Output[0].Value != 1) || (Output[1].Value != 1) || (Output[2].Value != 0))
                return false;
            Input[0].Value = 1; Input[1].Value = 1; Input[2].Value = 0;
            if ((Output[0].Value != 0) || (Output[1].Value != 0) || (Output[2].Value != 1))
                return false;
            Input[0].Value = 1; Input[1].Value = 0; Input[2].Value = 1;
            if ((Output[0].Value != 0) || (Output[1].Value != 1) || (Output[2].Value != 0))
                return false;
            Input[0].Value = 0; Input[1].Value = 1; Input[2].Value = 1;
            if ((Output[0].Value != 1) || (Output[1].Value != 0) || (Output[2].Value != 0))
                return false;
            Input[0].Value = 1; Input[1].Value = 1; Input[2].Value = 1;
            if ((Output[0].Value != 0) || (Output[1].Value != 0) || (Output[2].Value != 0))
                return false;
            return true;
        }
    }
}
