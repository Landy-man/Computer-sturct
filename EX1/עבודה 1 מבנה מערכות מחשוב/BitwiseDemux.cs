using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    class BitwiseDemux : Gate
    {
        public int Size { get; private set; }
        public WireSet Output1 { get; private set; }
        public WireSet Output2 { get; private set; }
        public WireSet Input { get; private set; }
        public Wire Control { get; private set; }

        //your code here
        private Demux[] bitWiseDemux;

        public BitwiseDemux(int iSize)
        {
            Size = iSize;
            Control = new Wire();
            Input = new WireSet(Size);

            //your code here
            Output1 = new WireSet(Size);
            Output2 = new WireSet(Size);
            bitWiseDemux = new Demux[iSize];
            for (int i = 0; i < iSize; i++)
            {
                bitWiseDemux[i] = new Demux();
                bitWiseDemux[i].ConnectControl(Control);
                bitWiseDemux[i].ConnectInput(Input[i]);
                Output1[i].ConnectInput(bitWiseDemux[i].Output1);
                Output2[i].ConnectInput(bitWiseDemux[i].Output2);
            }
        }

        public void ConnectControl(Wire wControl)
        {
            Control.ConnectInput(wControl);
        }
        public void ConnectInput(WireSet wsInput)
        {
            Input.ConnectInput(wsInput);
        }

        public override bool TestGate()
        {
            Input[0].Value = 0; Input[1].Value = 0;
            Control.Value = 0;
            if ((Output1[0].Value != 0)||(Output1[1].Value!=0) ||
                (Output2[0].Value != 0)||(Output2[1].Value!=0))
                return false;

            Input[0].Value = 1; Input[1].Value = 0;
            Control.Value = 0;
            if ((Output1[0].Value != 1) || (Output1[1].Value != 0) ||
                (Output2[0].Value != 0) || (Output2[1].Value != 0))
                return false;

            Input[0].Value = 0; Input[1].Value = 1;
            Control.Value = 0;
            if ((Output1[0].Value != 0) || (Output1[1].Value != 1) ||
                (Output2[0].Value != 0) || (Output2[1].Value != 0))
                return false;

            Input[0].Value = 1; Input[1].Value = 1;
            Control.Value = 0;
            if ((Output1[0].Value != 1) || (Output1[1].Value != 1) ||
                (Output2[0].Value != 0) || (Output2[1].Value != 0))
                return false;

            Input[0].Value = 0; Input[1].Value = 0;
            Control.Value = 1;
            if ((Output1[0].Value != 0) || (Output1[1].Value != 0) ||
                (Output2[0].Value != 0) || (Output2[1].Value != 0))
                return false;

            Input[0].Value = 1; Input[1].Value = 0;
            Control.Value = 1;
            if ((Output1[0].Value != 0) || (Output1[1].Value != 0) ||
                (Output2[0].Value != 1) || (Output2[1].Value != 0))
                return false;

            Input[0].Value = 0; Input[1].Value = 1;
            Control.Value = 1;
            if ((Output1[0].Value != 0) || (Output1[1].Value != 0) ||
                (Output2[0].Value != 0) || (Output2[1].Value != 1))
                return false;

            Input[0].Value = 1; Input[1].Value = 1;
            Control.Value = 1;
            if ((Output1[0].Value != 0) || (Output1[1].Value != 0) ||
                (Output2[0].Value != 1) || (Output2[1].Value != 1))
                return false;

            return true;
        }
    }
}
