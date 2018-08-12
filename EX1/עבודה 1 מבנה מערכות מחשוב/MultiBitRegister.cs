using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    class MultiBitRegister : Gate
    {
        public WireSet Input { get; private set; }
        public WireSet Output { get; private set; }
        public Wire Load { get; private set; }
        public int Size { get; private set; }
        private SingleBitRegister[] registers;


        public MultiBitRegister(int iSize)
        {
            Size = iSize;
            Input = new WireSet(Size);
            Output = new WireSet(Size);
            Load = new Wire();
            //your code here

            registers = new SingleBitRegister[iSize];
            for (int i = 0; i < iSize; i++)
            {
                registers[i] = new SingleBitRegister();
                registers[i].ConnectInput(Input[i]);
                registers[i].ConnectLoad(Load);
                Output[i].ConnectInput(registers[i].Output);
            }

        }

        public void ConnectInput(WireSet wsInput)
        {
            Input.ConnectInput(wsInput);
        }

        
        public override string ToString()
        {
            return Output.ToString();
        }


        public override bool TestGate()
        {
            Input.SetValue(8);
            Load.Value = 1;
            Clock.ClockDown();
            Clock.ClockUp();
            Input.SetValue(7);
            Load.Value = 1;
            if (Output.GetValue() != 8)
                return false;
            Clock.ClockDown();
            Clock.ClockUp();
            Input.SetValue(6);
            Load.Value = 0;
            if (Output.GetValue() != 7)
                return false;
            Clock.ClockDown();
            Clock.ClockUp();
            Input.SetValue(5);
            Load.Value = 0;
            if (Output.GetValue() != 7)
                return false;
            return true;
        }
    }
}
