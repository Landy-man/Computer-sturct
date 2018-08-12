using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    class Memory : SequentialGate
    {
        public int AddressSize { get; private set; }
        public int WordSize { get; private set; }

        public WireSet Input { get; private set; }
        public WireSet Output { get; private set; }
        public WireSet Address { get; private set; }
        public Wire Load { get; private set; }
        private BitwiseMultiwayDemux demux;
        private BitwiseMultiwayMux mux;
        private MultiBitRegister[] registers;

        public Memory(int iAddressSize, int iWordSize)
        {
            AddressSize = iAddressSize;
            WordSize = iWordSize;
            int registerNum = (int)Math.Pow(2.0,AddressSize);
            Input = new WireSet(WordSize);
            Output = new WireSet(WordSize);
            Address = new WireSet(AddressSize);
            Load = new Wire();
            WireSet newLoad = new WireSet(1);
            newLoad[0].ConnectInput(Load);
            demux = new BitwiseMultiwayDemux(1, AddressSize);
            mux = new BitwiseMultiwayMux(WordSize, AddressSize);
            registers = new MultiBitRegister[registerNum];
            WireSet AddressMirror = new WireSet(AddressSize);
            for (int j = 0; j < AddressSize; j++)
            {
                AddressMirror[j].ConnectInput(Address[AddressSize - 1 - j]);
            }
            demux.ConnectControl(AddressMirror);
            demux.ConnectInput(newLoad);
            for (int i = 0; i < registerNum; i++)
            {
                registers[i] = new MultiBitRegister(WordSize);
                registers[i].ConnectInput(Input);
                registers[i].Load.ConnectInput(demux.Outputs[i][0]);
                mux.ConnectInput(i, registers[i].Output);
            }
            mux.ConnectControl(Address);
            Output.ConnectInput(mux.Output);
        }

        public void ConnectInput(WireSet wsInput)
        {
            Input.ConnectInput(wsInput);
        }
        public void ConnectAddress(WireSet wsAddress)
        {
            Address.ConnectInput(wsAddress);
        }


        public override void OnClockUp()
        {
        }

        public override void OnClockDown()
        {
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }

        public override bool TestGate()
        {
            Load.Value = 0;
            Clock.ClockDown();
            Clock.ClockUp();
            if (Output.GetValue() != 0)
                return false;
            Input.SetValue(1);
            Clock.ClockDown();
            Clock.ClockUp();
            if (Output.GetValue() != 0)
                return false;
            Load.Value = 1;
            Address.SetValue(0);
            Clock.ClockDown();
            Clock.ClockUp();
            if (Output.GetValue() != 1)
                return false;
            Input.SetValue(0);
            Load.Value = 0;
            Address.SetValue(2);
            Clock.ClockDown();
            Clock.ClockUp();
            if (Output.GetValue() != 0)
                return false;
            return true;
        }
    }
}
