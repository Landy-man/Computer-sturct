using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    class SingleBitRegister : Gate
    {
 
        public Wire Input { get; private set; }
        public Wire Output { get; private set; }
        public Wire Load { get; private set; }
 
        private DFlipFlopGate dff;
        private MuxGate mux;


        public SingleBitRegister()
        {
            
            Input = new Wire();
            Load = new Wire();
            //your code here 
            Output = new Wire();
            mux = new MuxGate();
            dff = new DFlipFlopGate();
            mux.ConnectControl(Load);
            mux.ConnectInput1(dff.Output);
            mux.ConnectInput2(Input);
            dff.ConnectInput(mux.Output);
            Output.ConnectInput(dff.Output);

        }

        public void ConnectInput(Wire wInput)
        {
            Input.ConnectInput(wInput);
        }

      

        public void ConnectLoad(Wire wLoad)
        {
            Load.ConnectInput(wLoad);
        }


        public override bool TestGate()
        {
                        Input.Value = 1;
            Load.Value = 1;
            Clock.ClockDown();
            Clock.ClockUp();
            Input.Value = 0;
            Load.Value = 0;
            if (Output.Value != 1)
                return false;
            Clock.ClockDown();
            Clock.ClockUp();
            Input.Value = 1;
            Load.Value = 0;
            if (Output.Value != 1)
                return false;
            return true;
        }
    }
}
