using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    class XorGate : TwoInputGate
    {
        //your code here
        private OrGate m_gOr;
        private AndGate m_gAnd;
        private NAndGate m_gNand;

        public XorGate()
        {
            //your code here
            m_gAnd = new AndGate();
            m_gNand = new NAndGate();
            m_gOr = new OrGate();

            m_gAnd.ConnectInput1(m_gNand.Output);
            m_gAnd.ConnectInput2(m_gOr.Output);


            Input1 = m_gOr.Input1;
            Input2 = m_gOr.Input2;

            m_gNand.ConnectInput1 (Input1);
            m_gNand.ConnectInput2 (Input2);

            Output = m_gAnd.Output;

        }

        public override string ToString()
        {
            return "Xor " + Input1.Value + "," + Input2.Value + " -> " + Output.Value;
        }



        public override bool TestGate()
        {
            Input1.Value = 0;
            Input2.Value = 0;
            if (Output.Value != 0)
                return false;
            Input1.Value = 0;
            Input2.Value = 1;
            if (Output.Value != 1)
                return false;
            Input1.Value = 1;
            Input2.Value = 0;
            if (Output.Value != 1)
                return false;
            Input1.Value = 1;
            Input2.Value = 1;
            if (Output.Value != 0)
                return false;
            return true;
        }
    }
}
