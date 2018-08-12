using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    class MultiBitAndGate : MultiBitGate
    {
        //your code here
        private AndGate[] and;

        public MultiBitAndGate(int iInputCount)
            : base(iInputCount)
        {
            //your code here
            and = new AndGate[iInputCount - 1];
            and[0] = new AndGate();
            and[0].ConnectInput1(m_wsInput[0]);
            and[0].ConnectInput2(m_wsInput[1]);
           for (int i = 2; i<iInputCount; i++)
           {
               and[i-1] = new AndGate();
               and[i-1].ConnectInput1(and[i-2].Output);
               and[i - 1].ConnectInput2(m_wsInput[i]);
           }
           Output.ConnectInput(and[iInputCount-2].Output);
           


        }



        public override bool TestGate()
        {
            m_wsInput[0].Value = 0; m_wsInput[1].Value = 0; m_wsInput[2].Value = 0; m_wsInput[3].Value = 0;
            if (Output.Value != 0)
                return false;
            m_wsInput[0].Value = 1; m_wsInput[1].Value = 0; m_wsInput[2].Value = 0; m_wsInput[3].Value = 0;
            if (Output.Value != 0)
                return false;
            m_wsInput[0].Value = 0; m_wsInput[1].Value = 1; m_wsInput[2].Value = 0; m_wsInput[3].Value = 0;
            if (Output.Value != 0)
                return false;
            m_wsInput[0].Value = 0; m_wsInput[1].Value = 0; m_wsInput[2].Value = 1; m_wsInput[3].Value = 0;
            if (Output.Value != 0)
                return false;
            m_wsInput[0].Value = 1; m_wsInput[1].Value = 1; m_wsInput[2].Value = 0; m_wsInput[3].Value = 0;
            if (Output.Value != 0)
                return false;
            m_wsInput[0].Value = 1; m_wsInput[1].Value = 0; m_wsInput[2].Value = 1; m_wsInput[3].Value = 0;
            if (Output.Value != 0)
                return false;
            m_wsInput[0].Value = 0; m_wsInput[1].Value = 1; m_wsInput[2].Value = 1; m_wsInput[3].Value = 0;
            if (Output.Value != 0)
                return false;
            m_wsInput[0].Value = 1; m_wsInput[1].Value = 1; m_wsInput[2].Value = 1; m_wsInput[3].Value = 0;
            if (Output.Value != 0)
                return false;
            m_wsInput[0].Value = 0; m_wsInput[1].Value = 0; m_wsInput[2].Value = 0; m_wsInput[3].Value = 1;
            if (Output.Value != 0)
                return false;
            m_wsInput[0].Value = 1; m_wsInput[1].Value = 0; m_wsInput[2].Value = 0; m_wsInput[3].Value = 1;
            if (Output.Value != 0)
                return false;
            m_wsInput[0].Value = 0; m_wsInput[1].Value = 1; m_wsInput[2].Value = 0; m_wsInput[3].Value = 1;
            if (Output.Value != 0)
                return false;
            m_wsInput[0].Value = 0; m_wsInput[1].Value = 0; m_wsInput[2].Value = 1; m_wsInput[3].Value = 1;
            if (Output.Value != 0)
                return false;
            m_wsInput[0].Value = 1; m_wsInput[1].Value = 1; m_wsInput[2].Value = 0; m_wsInput[3].Value = 1;
            if (Output.Value != 0)
                return false;
            m_wsInput[0].Value = 1; m_wsInput[1].Value = 0; m_wsInput[2].Value = 1; m_wsInput[3].Value = 1;
            if (Output.Value != 0)
                return false;
            m_wsInput[0].Value = 0; m_wsInput[1].Value = 1; m_wsInput[2].Value = 1; m_wsInput[3].Value = 1;
            if (Output.Value != 0)
                return false;
            m_wsInput[0].Value = 1; m_wsInput[1].Value = 1; m_wsInput[2].Value = 1; m_wsInput[3].Value = 1;
            if (Output.Value != 1)
                return false;
            return true;
        }
    }
}
