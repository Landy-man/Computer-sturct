using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    class BitwiseMultiwayDemux : Gate
    {
        public int Size { get; private set; }
        public int ControlBits { get; private set; }
        public WireSet Input { get; private set; }
        public WireSet Control { get; private set; }
        public WireSet[] Outputs { get; private set; }

        //your code here
        private BitwiseDemux[] demux;
        private WireSet[] demuxOutputs;

        public BitwiseMultiwayDemux(int iSize, int cControlBits)
        {
            Size = iSize;
            Input = new WireSet(Size);
            Control = new WireSet(cControlBits);
            Outputs = new WireSet[(int)Math.Pow(2, cControlBits)];
           demux = new BitwiseDemux[Outputs.Length];
            demuxOutputs = new WireSet[2*demux.Length];
            int freeDemux = 1, whereToStart=0, whereToEnd=2;
            for (int i = 0; i < Outputs.Length; i++)
            {
                Outputs[i] = new WireSet(Size);
                demux[i] = new BitwiseDemux(Size);
            }
            for (int a = 0; a < demuxOutputs.Length; a++)
            {
                demuxOutputs[a] = new WireSet(Size);
            }
            demux[0].ConnectControl(Control[Control.Size-1]);//CHANGED
            demux[0].ConnectInput(Input);
            demuxOutputs[0].ConnectInput(demux[0].Output1);
            demuxOutputs[1].ConnectInput(demux[0].Output2);
            
            for (int j=1; j<cControlBits;j++)
            {
                for (int k=whereToStart; k<whereToEnd; k++)
                {
                    demux[freeDemux].ConnectInput(demuxOutputs[whereToStart]);
                    demux[freeDemux].ConnectControl(Control[Control.Size -j-1]); //CHANGED
                    demuxOutputs[2 * freeDemux].ConnectInput(demux[freeDemux].Output1);
                    demuxOutputs[(2 * freeDemux)+1].ConnectInput(demux[freeDemux].Output2);
                    freeDemux++;
                    whereToStart++;
                }
                whereToEnd = 2*freeDemux; 
            }
            for (int l= 0; l<Outputs.Length; l++)
            {
                Outputs[l].ConnectInput(demuxOutputs[l+whereToStart]);
            }

        }


        public void ConnectInput(WireSet wsInput)
        {
            Input.ConnectInput(wsInput);
        }
        public void ConnectControl(WireSet wsControl)
        {
            Control.ConnectInput(wsControl);
        }


        public override bool TestGate()
        {
            Control[0].Value = 0; Control[1].Value = 0;
            Input[0].Value = 0; Input[1].Value = 0; Input[2].Value = 0;
            if (((Outputs[0][0].Value != 0) || (Outputs[0][1].Value != 0) || (Outputs[0][2].Value != 0)) ||
                ((Outputs[1][0].Value != 0) || (Outputs[1][1].Value != 0) || (Outputs[1][2].Value != 0)) ||
                ((Outputs[2][0].Value != 0) || (Outputs[2][1].Value != 0) || (Outputs[2][2].Value != 0)) ||
                ((Outputs[3][0].Value != 0) || (Outputs[3][1].Value != 0) || (Outputs[3][2].Value != 0)))
                return false;
            Control[0].Value = 0; Control[1].Value = 0;
            Input[0].Value = 1; Input[1].Value = 0; Input[2].Value = 0;
            if (((Outputs[0][0].Value != 1) || (Outputs[0][1].Value != 0) || (Outputs[0][2].Value != 0)) ||
                ((Outputs[1][0].Value != 0) || (Outputs[1][1].Value != 0) || (Outputs[1][2].Value != 0)) ||
                ((Outputs[2][0].Value != 0) || (Outputs[2][1].Value != 0) || (Outputs[2][2].Value != 0)) ||
                ((Outputs[3][0].Value != 0) || (Outputs[3][1].Value != 0) || (Outputs[3][2].Value != 0)))
                return false;
            Control[0].Value = 1; Control[1].Value = 1;
            Input[0].Value = 1; Input[1].Value = 1; Input[2].Value = 0;
            if (((Outputs[0][0].Value != 0) || (Outputs[0][1].Value != 0) || (Outputs[0][2].Value != 0)) ||
                ((Outputs[1][0].Value != 0) || (Outputs[1][1].Value != 0) || (Outputs[1][2].Value != 0)) ||
                ((Outputs[2][0].Value != 0) || (Outputs[2][1].Value != 0) || (Outputs[2][2].Value != 0)) ||
                ((Outputs[3][0].Value != 1) || (Outputs[3][1].Value != 1) || (Outputs[3][2].Value != 0)))
                return false;
            Control[0].Value = 1; Control[1].Value = 0;
            Input[0].Value = 1; Input[1].Value = 1; Input[2].Value = 0;
            if (((Outputs[0][0].Value != 0) || (Outputs[0][1].Value != 0) || (Outputs[0][2].Value != 0)) ||
                ((Outputs[1][0].Value != 1) || (Outputs[1][1].Value != 1) || (Outputs[1][2].Value != 0)) ||
                ((Outputs[2][0].Value != 0) || (Outputs[2][1].Value != 0) || (Outputs[2][2].Value != 0)) ||
                ((Outputs[3][0].Value != 0) || (Outputs[3][1].Value != 0) || (Outputs[3][2].Value != 0)))
                return false;
            Control[0].Value = 0; Control[1].Value = 1;
            Input[0].Value = 1; Input[1].Value = 1; Input[2].Value = 1;
            if (((Outputs[0][0].Value != 0) || (Outputs[0][1].Value != 0) || (Outputs[0][2].Value != 0)) ||
                ((Outputs[1][0].Value != 0) || (Outputs[1][1].Value != 0) || (Outputs[1][2].Value != 0)) ||
                ((Outputs[2][0].Value != 1) || (Outputs[2][1].Value != 1) || (Outputs[2][2].Value != 1)) ||
                ((Outputs[3][0].Value != 0) || (Outputs[3][1].Value != 0) || (Outputs[3][2].Value != 0)))
                return false;
            return true;
        }
    }
}
