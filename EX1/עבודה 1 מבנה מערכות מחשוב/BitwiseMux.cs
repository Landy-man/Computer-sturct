using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    class BitwiseMux : BitwiseTwoInputGate
    {
        public Wire ControlInput { get; private set; }

        //your code here
        private MuxGate[] mux;
        public BitwiseMux(int iSize)
            : base(iSize)
        {

            ControlInput = new Wire();
            //your code here
            mux = new MuxGate[iSize];
            for (int i=0; i<iSize; i++)
            {                
                mux[i] = new MuxGate();
                mux[i].ConnectControl(ControlInput);
                mux[i].ConnectInput1(Input1[i]);
                mux[i].ConnectInput2(Input2[i]);
                Output[i].ConnectInput(mux[i].Output);
            }

        }

        public void ConnectControl(Wire wControl)
        {
            ControlInput.ConnectInput(wControl);
        }



        public override string ToString()
        {
            return "Mux " + Input1 + "," + Input2 + ",C" + ControlInput.Value + " -> " + Output;
        }




        public override bool TestGate()
        {
            Input1[0].Value = 0; Input1[1].Value = 0;
            Input2[0].Value = 0; Input2[1].Value = 0;
            ControlInput.Value = 0;
            if ((Output[0].Value != 0)||(Output[1].Value != 0))
                return false;
            Input1[0].Value = 1; Input1[1].Value = 0;
            Input2[0].Value = 0; Input2[1].Value = 0;
            ControlInput.Value = 0;
            if ((Output[0].Value != 1) || (Output[1].Value != 0))
                return false;
            Input1[0].Value = 1; Input1[1].Value = 1;
            Input2[0].Value = 0; Input2[1].Value = 0;
            ControlInput.Value = 0;
            if ((Output[0].Value != 1) || (Output[1].Value != 1))
                return false;
            Input1[0].Value = 1; Input1[1].Value = 0;
            Input2[0].Value = 1; Input2[1].Value = 0;
            ControlInput.Value = 0;
            if ((Output[0].Value != 1) || (Output[1].Value != 0))
                return false;
            Input1[0].Value = 1; Input1[1].Value = 0;
            Input2[0].Value = 0; Input2[1].Value = 1;
            ControlInput.Value = 0;
            if ((Output[0].Value != 1) || (Output[1].Value != 0))
                return false;
            Input1[0].Value = 1; Input1[1].Value = 1;
            Input2[0].Value = 0; Input2[1].Value = 1;
            ControlInput.Value = 0;
            if ((Output[0].Value != 1) || (Output[1].Value != 1))
                return false;
            Input1[0].Value = 1; Input1[1].Value = 0;
            Input2[0].Value = 1; Input2[1].Value = 1;
            ControlInput.Value = 0;
            if ((Output[0].Value != 1) || (Output[1].Value != 0))
                return false;
            Input1[0].Value = 0; Input1[1].Value = 1;
            Input2[0].Value = 1; Input2[1].Value = 1;
            ControlInput.Value = 0;
            if ((Output[0].Value != 0) || (Output[1].Value != 1))
                return false;
            Input1[0].Value = 0; Input1[1].Value = 1;
            Input2[0].Value = 0; Input2[1].Value = 0;
            ControlInput.Value = 0;
            if ((Output[0].Value != 0) || (Output[1].Value != 1))
                return false;
            Input1[0].Value = 1; Input1[1].Value = 0;
            Input2[0].Value = 0; Input2[1].Value = 0;
            ControlInput.Value = 1;
            if ((Output[0].Value != 0) || (Output[1].Value != 0))
                return false;
            Input1[0].Value = 1; Input1[1].Value = 1;
            Input2[0].Value = 0; Input2[1].Value = 0;
            ControlInput.Value = 1;
            if ((Output[0].Value != 0) || (Output[1].Value != 0))
                return false;
            Input1[0].Value = 1; Input1[1].Value = 0;
            Input2[0].Value = 1; Input2[1].Value = 0;
            ControlInput.Value = 1;
            if ((Output[0].Value != 1) || (Output[1].Value != 0))
                return false;
            Input1[0].Value = 1; Input1[1].Value = 0;
            Input2[0].Value = 0; Input2[1].Value = 1;
            ControlInput.Value = 1;
            if ((Output[0].Value != 0) || (Output[1].Value != 1))
                return false;
            Input1[0].Value = 1; Input1[1].Value = 0;
            Input2[0].Value = 1; Input2[1].Value = 1;
            ControlInput.Value = 1;
            if ((Output[0].Value != 1) || (Output[1].Value != 1))
                return false;
            return true;
        }
    }
}
