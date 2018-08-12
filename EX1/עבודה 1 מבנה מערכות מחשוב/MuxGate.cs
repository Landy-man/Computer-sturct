using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    class MuxGate : TwoInputGate
    {
        public Wire ControlInput { get; private set; }
        private NotGate m_gNot;
        private AndGate m_gAnd1;
        private AndGate m_gAnd2;
        private OrGate m_gOr;


        public MuxGate()
        {
            ControlInput = new Wire();

            m_gNot = new NotGate();
            m_gAnd1 = new AndGate();
            m_gAnd2 = new AndGate();
            m_gOr = new OrGate();
            m_gAnd1.ConnectInput1(m_gNot.Output);
            m_gOr.ConnectInput1(m_gAnd1.Output);
            m_gOr.ConnectInput2(m_gAnd2.Output);
            m_gNot.ConnectInput(ControlInput);
            m_gAnd2.ConnectInput1(ControlInput);
            Input1 = m_gAnd1.Input2;
            Input2 = m_gAnd2.Input2;
            Output = m_gOr.Output;

        }

        public void ConnectControl(Wire wControl)
        {
            ControlInput.ConnectInput(wControl);
        }


        public override string ToString()
        {
            return "Mux " + Input1.Value + "," + Input2.Value + ",C" + ControlInput.Value + " -> " + Output.Value;
        }



        public override bool TestGate()
        {
            Input1.Value = 0;
            Input2.Value = 0;
            ControlInput.Value = 0;
            if (Output.Value != 0)
                return false;
            Input1.Value = 0;
            Input2.Value = 1;
            ControlInput.Value = 0;
            if (Output.Value != 0)
                return false;
            Input1.Value = 1;
            Input2.Value = 0;
            ControlInput.Value = 0;
            if (Output.Value != 1)
                return false;
            Input1.Value = 1;
            Input2.Value = 1;
            ControlInput.Value = 0;
            if (Output.Value != 1)
                return false;
            Input1.Value = 1;
            Input2.Value = 1;
            ControlInput.Value = 1;
            if (Output.Value != 1)
                return false;
            Input1.Value = 0;
            Input2.Value = 0;
            ControlInput.Value = 1;
            if (Output.Value != 0)
                return false;
            Input1.Value = 0;
            Input2.Value = 1;
            ControlInput.Value = 1;
            if (Output.Value != 1)
                return false;
            Input1.Value = 1;
            Input2.Value =0;
            ControlInput.Value = 1;
            if (Output.Value != 0)
                return false;
            return true;
        }
    }
}
