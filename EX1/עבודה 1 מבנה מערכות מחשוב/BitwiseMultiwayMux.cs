using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    class BitwiseMultiwayMux : Gate
    {
        public int Size { get; private set; }
        public int ControlBits { get; private set; }
        public WireSet Output { get; private set; }
        public WireSet Control { get; private set; }
        public WireSet[] Inputs { get; private set; }

        //your code here
        private WireSet[] muxOutputs;
        private BitwiseMux[] mux;

        public BitwiseMultiwayMux(int iSize, int cControlBits)
        {
            Size = iSize;
            Output = new WireSet(Size);
            Control = new WireSet(cControlBits);
            Inputs = new WireSet[(int)Math.Pow(2, cControlBits)];
            
            muxOutputs = new WireSet[Inputs.Length];
            mux = new BitwiseMux[Inputs.Length];
            int freeMux = 0;
            for (int i = 0; i < Inputs.Length; i++)
            {
                Inputs[i] = new WireSet(Size);
                muxOutputs[i] = new WireSet(Size);
                mux[i] = new BitwiseMux(Size);
                if (i % 2 == 1)
                {
                    mux[freeMux].ConnectInput1(Inputs[i-1]);
                    mux[freeMux].ConnectInput2(Inputs[i]);
                    mux[freeMux].ConnectControl(Control[0]);
                    muxOutputs[freeMux].ConnectInput(mux[freeMux].Output);
                    freeMux++;
                }
            }
            int muxNum = freeMux / 2, whereToStart = 0, whereToEnd= freeMux;
            for (int j=1; j<cControlBits; j++)
            {                
                for (int k= whereToStart; k<whereToEnd; k+=2)
                {
                    mux[freeMux].ConnectInput1(muxOutputs[whereToStart]);
                    mux[freeMux].ConnectInput2(muxOutputs[whereToStart + 1]);
                    mux[freeMux].ConnectControl(Control[j]);
                    muxOutputs[freeMux].ConnectInput(mux[freeMux].Output);
                    freeMux++;
                    whereToStart += 2;                   
                }
                whereToEnd = freeMux;
            }
            Output.ConnectInput(muxOutputs[freeMux-1]);

        }


        public void ConnectInput(int i, WireSet wsInput)
        {
            Inputs[i].ConnectInput(wsInput);
        }
        public void ConnectControl(WireSet wsControl)
        {
            Control.ConnectInput(wsControl);
        }



        public override bool TestGate()
        {
            Control[0].Value = 1; Control[1].Value = 1;
            Inputs[0][0].Value = 0; Inputs[0][1].Value = 0; Inputs[0][2].Value = 0;
            Inputs[1][0].Value = 1; Inputs[1][1].Value = 0; Inputs[1][2].Value = 1;
            Inputs[2][0].Value = 0; Inputs[2][1].Value = 1; Inputs[2][2].Value = 0;
            Inputs[3][0].Value = 0; Inputs[3][1].Value = 1; Inputs[3][2].Value = 1;
            if ((Output[0].Value != 0) || (Output[1].Value != 1) || (Output[2].Value != 1))
                return false;

            Control[0].Value = 0; Control[1].Value = 0;
            Inputs[0][0].Value = 0; Inputs[0][1].Value = 0; Inputs[0][2].Value = 0;
            Inputs[1][0].Value = 1; Inputs[1][1].Value = 0; Inputs[1][2].Value = 1;
            Inputs[2][0].Value = 0; Inputs[2][1].Value = 1; Inputs[2][2].Value = 0;
            Inputs[3][0].Value = 0; Inputs[3][1].Value = 1; Inputs[3][2].Value = 1;
            if ((Output[0].Value != 0) || (Output[1].Value != 0) || (Output[2].Value != 0))
                return false;


            Control[0].Value = 0; Control[1].Value = 1;
            Inputs[0][0].Value = 0; Inputs[0][1].Value = 0; Inputs[0][2].Value = 0;
            Inputs[1][0].Value = 1; Inputs[1][1].Value = 0; Inputs[1][2].Value = 1;
            Inputs[2][0].Value = 0; Inputs[2][1].Value = 1; Inputs[2][2].Value = 0;
            Inputs[3][0].Value = 0; Inputs[3][1].Value = 1; Inputs[3][2].Value = 1;
            if ((Output[0].Value != 0) || (Output[1].Value != 1) || (Output[2].Value != 0))
                return false;

            Control[0].Value = 1; Control[1].Value = 0;
            Inputs[0][0].Value = 0; Inputs[0][1].Value = 0; Inputs[0][2].Value = 0;
            Inputs[1][0].Value = 1; Inputs[1][1].Value = 0; Inputs[1][2].Value = 1;
            Inputs[2][0].Value = 0; Inputs[2][1].Value = 1; Inputs[2][2].Value = 0;
            Inputs[3][0].Value = 0; Inputs[3][1].Value = 1; Inputs[3][2].Value = 1;
            if ((Output[0].Value != 1) || (Output[1].Value != 0) || (Output[2].Value != 1))
                return false;
            return true;
        }
    }
}
