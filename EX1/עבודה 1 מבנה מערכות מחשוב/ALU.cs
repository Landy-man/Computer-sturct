using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    class ALU : Gate
    {
        public WireSet InputX { get; private set; }
        public WireSet InputY { get; private set; }
        public WireSet Output { get; private set; }

        public Wire ZeroX { get; private set; }
        public Wire ZeroY { get; private set; }
        public Wire NotX { get; private set; }
        public Wire NotY { get; private set; }
        public Wire F { get; private set; }
        public Wire NotOutput { get; private set; }

        public Wire Zero { get; private set; }
        public Wire Negative { get; private set; }

        public int Size { get; private set; }

        public BitwiseMux zx;
        public BitwiseMux zy;
        public BitwiseMux nx;
        public BitwiseMux ny;
        public BitwiseMux fx;
        public BitwiseMux nOut;

        public BitwiseNotGate notX;
        public BitwiseNotGate notY;
        public BitwiseNotGate notOut;
        public BitwiseNotGate notCheckzero;

        public BitwiseAndGate fAnd;
        public MultiBitAndGate zeroCheck;
        public MultiBitAdder fAdder, negCheck;


        public ALU(int iSize)
        {
            Size = iSize;
            InputX = new WireSet(Size);
            InputY = new WireSet(Size);
            ZeroX = new Wire();
            ZeroY = new Wire();
            NotX = new Wire();
            NotY = new Wire();
            F = new Wire();
            NotOutput = new Wire();
            Negative = new Wire();            
            Zero = new Wire();
            Output = new WireSet(Size);

            zx = new BitwiseMux(Size);
            zy = new BitwiseMux(Size);
            nx = new BitwiseMux(Size);
            ny = new BitwiseMux(Size);
            fx = new BitwiseMux(Size);
            nOut = new BitwiseMux(Size);

            notX = new BitwiseNotGate(Size);
            notY = new BitwiseNotGate(Size);
            notOut = new BitwiseNotGate(Size);
            notCheckzero = new BitwiseNotGate(Size);

            fAdder = new MultiBitAdder(Size);
            fAnd = new BitwiseAndGate(Size);

            negCheck = new MultiBitAdder(Size);
            zeroCheck = new MultiBitAndGate(Size);

            //zx//
            WireSet zeroXWire = new WireSet(Size);
            zx.ConnectInput1(InputX);
            zx.ConnectInput2(zeroXWire);
            zx.ConnectControl(ZeroX);

            //nx//

            nx.ConnectInput1(zx.Output);
            notX.ConnectInput(zx.Output);
            nx.ConnectInput2(notX.Output);
            nx.ConnectControl(NotX);

            //zy//
            WireSet zeroYWire = new WireSet(Size);
            zy.ConnectInput1(InputY);
            zy.ConnectInput2(zeroYWire);
            zy.ConnectControl(ZeroY);

            //ny//

            ny.ConnectInput1(zy.Output);
            notY.ConnectInput(zy.Output);
            ny.ConnectInput2(notY.Output);
            ny.ConnectControl(NotY);

            //f//
            fAnd.ConnectInput1(nx.Output);
            fAnd.ConnectInput2(ny.Output);
            fAdder.ConnectInput1(nx.Output);
            fAdder.ConnectInput2(ny.Output);
            fx.ConnectInput1(fAnd.Output);
            fx.ConnectInput2(fAdder.Output);
            fx.ConnectControl(F);

            //not Outpot
            nOut.ConnectInput1(fx.Output);
            notOut.ConnectInput(fx.Output);
            nOut.ConnectInput2(notOut.Output);
            nOut.ConnectControl(NotOutput);

            
            Output = nOut.Output;
            
            
            //negative number check
            negCheck.ConnectInput1(nOut.Output);
            negCheck.ConnectInput2(nOut.Output);
            Negative = negCheck.Overflow;

            //Zero number check
            notCheckzero.ConnectInput(nOut.Output);
            zeroCheck.ConnectInput(notCheckzero.Output);
            Zero = zeroCheck.Output;


        }

        public override bool TestGate()
        {
            InputX.SetValue(6);
            InputY.SetValue(5);
            //setting ALU bit settings
            ZeroX.Value = 1; 
            ZeroY.Value = 1;
            NotX.Value = 0;
            NotY.Value = 0;
            F.Value = 1;
            NotOutput.Value = 0;
            //
            if (Output.Get2sComplement() != 0 || Zero.Value != 1 || Negative.Value != 0)
                return false;

            //setting ALU bit settings
            ZeroX.Value = 1;
            ZeroY.Value = 1;
            NotX.Value = 1;
            NotY.Value = 1;
            F.Value = 1;
            NotOutput.Value = 1;
            //
            if (Output.Get2sComplement() != 1 || Zero.Value != 0 || Negative.Value != 0)
                return false;

            //setting ALU bit settings
            ZeroX.Value = 1;
            ZeroY.Value = 1;
            NotX.Value = 1;
            NotY.Value = 0;
            F.Value = 1;
            NotOutput.Value = 0;
            //
            if (Output.Get2sComplement() != -1 || Zero.Value != 0 || Negative.Value != 1)
                return false;

            //setting ALU bit settings
            ZeroX.Value = 0;
            ZeroY.Value = 1;
            NotX.Value = 0;
            NotY.Value = 1;
            F.Value = 0;
            NotOutput.Value = 0;
            //
            if (Output.Get2sComplement() != 6 || Zero.Value != 0 || Negative.Value != 0)
                return false;

            //setting ALU bit settings
            ZeroX.Value = 1;
            NotX.Value = 1;
            ZeroY.Value = 0;
            NotY.Value = 0;
            F.Value = 0;
            NotOutput.Value = 0;
            //
            if (Output.Get2sComplement() != 5 || Zero.Value != 0 || Negative.Value != 0)
                return false;

            //setting ALU bit settings
            ZeroX.Value = 0;
            NotX.Value = 0;
            ZeroY.Value = 1;
            NotY.Value = 1;
            F.Value = 0;
            NotOutput.Value = 1;
            //
            if (Output.Get2sComplement() != -7 || Zero.Value != 0 || Negative.Value != 1)
                return false;

            //setting ALU bit settings
            ZeroX.Value = 1;
            NotX.Value = 1;
            ZeroY.Value = 0;
            NotY.Value = 0;
            F.Value = 0;
            NotOutput.Value = 1;
            //
            if (Output.Get2sComplement() != -6 || Zero.Value != 0 || Negative.Value != 1)
                return false;

            //setting ALU bit settings
            ZeroX.Value = 0;
            NotX.Value = 0;
            ZeroY.Value = 1;
            NotY.Value = 1;
            F.Value = 1;
            NotOutput.Value = 1;
            //
            if (Output.Get2sComplement() != -6 || Zero.Value != 0 || Negative.Value != 1)
                return false;

            //setting ALU bit settings
            ZeroX.Value = 1;
            NotX.Value = 1;
            ZeroY.Value = 0;
            NotY.Value = 0;
            F.Value = 1;
            NotOutput.Value = 1;
            //
            if (Output.Get2sComplement() != -5 || Zero.Value != 0 || Negative.Value != 1)
                return false;

            //setting ALU bit settings
            ZeroX.Value = 0;
            NotX.Value = 1;
            ZeroY.Value = 1;
            NotY.Value = 1;
            F.Value = 1;
            NotOutput.Value = 1;
            //
            if (Output.Get2sComplement() != 7 || Zero.Value != 0 || Negative.Value != 0)
                return false;

            //setting ALU bit settings
            ZeroX.Value = 1;
            NotX.Value = 1;
            ZeroY.Value = 0;
            NotY.Value = 1;
            F.Value = 1;
            NotOutput.Value = 1;
            //
            if (Output.Get2sComplement() != 6 || Zero.Value != 0 || Negative.Value != 0)
                return false;

            //setting ALU bit settings
            ZeroX.Value = 0;
            NotX.Value = 0;
            ZeroY.Value = 1;
            NotY.Value = 1;
            F.Value = 1;
            NotOutput.Value = 0;
            //
            if (Output.Get2sComplement() != 5 || Zero.Value != 0 || Negative.Value != 0)
                return false;

            //setting ALU bit settings
            ZeroX.Value = 1;
            NotX.Value = 1;
            ZeroY.Value = 0;
            NotY.Value = 0;
            F.Value = 1;
            NotOutput.Value = 0;
            //
            if (Output.Get2sComplement() != 4 || Zero.Value != 0 || Negative.Value != 0)
                return false;

            //setting ALU bit settings
            ZeroX.Value = 0;
            NotX.Value = 0;
            ZeroY.Value = 0;
            NotY.Value = 0;
            F.Value = 1;
            NotOutput.Value = 0;
            //
            if (Output.Get2sComplement() != -5 || Zero.Value != 0 || Negative.Value != 1)
                return false;

            //setting ALU bit settings
            ZeroX.Value = 0;
            NotX.Value = 1;
            ZeroY.Value = 0;
            NotY.Value = 0;
            F.Value = 1;
            NotOutput.Value = 1;
            //
            if (Output.Get2sComplement() != 1 || Zero.Value != 0 || Negative.Value != 0)
                return false;

            //setting ALU bit settings
            ZeroX.Value = 0;
            NotX.Value = 0;
            ZeroY.Value = 0;
            NotY.Value = 1;
            F.Value = 1;
            NotOutput.Value = 1;
            //
            if (Output.Get2sComplement() != -1 || Zero.Value != 0 || Negative.Value != 1)
                return false;

            return true;

        }
    }
}
