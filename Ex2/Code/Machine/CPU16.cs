using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SimpleComponents;

namespace Machine
{
    public class CPU16 
    {
        //this "enum" defines the different control bits names
        public const int J3 = 0, J2 = 1, J1 = 2, D3 = 3, D2 = 4, D1 = 5, C6 = 6, C5 = 7, C4 = 8, C3 = 9, C2 = 10, C1 = 11, A = 12, X2 = 13, X1 = 14, Type = 15;

        public int Size { get; private set; }

        //CPU inputs
        public WireSet Instruction { get; private set; }
        public WireSet MemoryInput { get; private set; }
        public Wire Reset { get; private set; }

        //CPU outputs
        public WireSet MemoryOutput { get; private set; }
        public Wire MemoryWrite { get; private set; }
        public WireSet MemoryAddress { get; private set; }
        public WireSet InstructionAddress { get; private set; }

        //CPU components
        private ALU m_gALU;
        private Counter m_rPC;
        private MultiBitRegister m_rA, m_rD;
        private BitwiseMux m_gAMux, m_gMAMux;

        //here we initialize and connect all the components, as in Figure 5.9 in the book
        public CPU16()
        {
            Size =  16;

            Instruction = new WireSet(Size);
            MemoryInput = new WireSet(Size);
            MemoryOutput = new WireSet(Size);
            MemoryAddress = new WireSet(Size);
            InstructionAddress = new WireSet(Size);
            MemoryWrite = new Wire();
            Reset = new Wire();

            m_gALU = new ALU(Size);
            m_rPC = new Counter(Size);
            m_rA = new MultiBitRegister(Size);
            m_rD = new MultiBitRegister(Size);

            m_gAMux = new BitwiseMux(Size);
            m_gMAMux = new BitwiseMux(Size);

            m_gAMux.ConnectInput1(Instruction);
            m_gAMux.ConnectInput2(m_gALU.Output);

            m_rA.ConnectInput(m_gAMux.Output);

            m_gMAMux.ConnectInput1(m_rA.Output);
            m_gMAMux.ConnectInput2(MemoryInput);
            m_gALU.InputY.ConnectInput(m_gMAMux.Output);

            m_gALU.InputX.ConnectInput(m_rD.Output);

            m_rD.ConnectInput(m_gALU.Output);

            MemoryOutput.ConnectInput(m_gALU.Output);
            MemoryAddress.ConnectInput(m_rA.Output);

            InstructionAddress.ConnectInput(m_rPC.Output);
            m_rPC.ConnectInput(m_rA.Output);
            m_rPC.ConnectReset(Reset);

            //now, we call the code that creates the control unit
            ConnectControls();
        }

        //add here components to implement the control unit 
        private BitwiseMultiwayMux m_gJumpMux;//an example of a control unit compnent - a mux that controls whether a jump is made
        private MuxGate[] muxArray;
        private OrGate or;
        private AndGate andForPCLoad;
        private WireSet jump;
        private AndGate JGT;
        private OrGate JGE;
        private OrGate JLE;
        private NotGate JNE;
        private NotGate[] notArray;

        private void ConnectControls()
        {
            or = new OrGate();
            muxArray = new MuxGate[9];
            NotGate not = new NotGate();
            JGT = new AndGate();
            JGE = new OrGate();
            JLE = new OrGate();
            JNE = new NotGate();
            notArray = new NotGate[2];

            for (int i = 0; i < notArray.Length; i++)
            {
                notArray[i] = new NotGate();
            }

            m_gJumpMux = new BitwiseMultiwayMux(1, 3);
            jump = new WireSet(3);

            andForPCLoad = new AndGate();
            Wire PCLoad = new Wire();

            //1.
            m_gAMux.ConnectControl(Instruction[Type]);

            //2. connect control to mux 2 (selects A or M entrance to the ALU)
            m_gMAMux.ConnectControl(Instruction[A]);

            //3. consider all instruction bits only if C type instruction (MSB of instruction is 1)
            for (int i = 0; i < muxArray.Length; i++)
            {
                muxArray[i] = new MuxGate();
                muxArray[i].ConnectInput1(new Wire());
                muxArray[i].ConnectInput2(Instruction[i + 3]);
                muxArray[i].ConnectControl(Instruction[Type]);
            }
                //4. connect ALU control bits
            m_gALU.ZeroX.ConnectInput(muxArray[C1 - 3].Output);
            m_gALU.NotX.ConnectInput(muxArray[C2 - 3].Output);
            m_gALU.ZeroY.ConnectInput(muxArray[C3 - 3].Output);
            m_gALU.NotY.ConnectInput(muxArray[C4 - 3].Output);
            m_gALU.F.ConnectInput(muxArray[C5 - 3].Output);
            m_gALU.NotOutput.ConnectInput(muxArray[C6 - 3].Output);
                
                //5. connect control to register D (very simple)
            m_rD.Load.ConnectInput(muxArray[D2 - 3].Output);

                //6. connect control to register A (a bit more complicated)
            not.ConnectInput(Instruction[Type]);
            or.ConnectInput1(not.Output);
            or.ConnectInput2(muxArray[D1 - 3].Output);
            m_rA.Load.ConnectInput(or.Output);

                //7. connect control to MemoryWrite
            MemoryWrite.ConnectInput(muxArray[D3 - 3].Output);
                //8. create inputs for jump mux
            notArray[0].ConnectInput(m_gALU.Zero);
            notArray[1].ConnectInput(m_gALU.Negative);
            //JGT:
            JGT.ConnectInput1(notArray[0].Output);
            JGT.ConnectInput2(notArray[1].Output);
            //JGE:
            JGE.ConnectInput1(m_gALU.Zero);
            JGE.ConnectInput2(notArray[1].Output);
            //JNE:
            JNE.ConnectInput(notArray[0].Output);
            //JLE:
            JLE.ConnectInput1(m_gALU.Zero);
            JLE.ConnectInput2(m_gALU.Negative);


            WireSet JGTOut = new WireSet(1);
            WireSet JEQOut = new WireSet(1);
            WireSet JLTOut = new WireSet(1);
            WireSet JGEOut = new WireSet(1);
            WireSet JNEOut = new WireSet(1);
            WireSet JLEOut = new WireSet(1);
            JGTOut[0].ConnectInput(JGT.Output);
            JEQOut[0].ConnectInput(m_gALU.Zero);
            JLTOut[0].ConnectInput(m_gALU.Negative);
            JGEOut[0].ConnectInput(JGE.Output);
            JNEOut[0].ConnectInput(JNE.Output);
            JLEOut[0].ConnectInput(JLE.Output);

            //9. connect jump mux (this is the most complicated part)
            jump[0].ConnectInput(Instruction[J3]);
            jump[1].ConnectInput(Instruction[J2]);
            jump[2].ConnectInput(Instruction[J1]);
            m_gJumpMux.ConnectControl(jump);
            m_gJumpMux.ConnectInput(0, new WireSet(1));
            m_gJumpMux.ConnectInput(1, JGTOut);
            m_gJumpMux.ConnectInput(2, JEQOut);
            m_gJumpMux.ConnectInput(3, JGEOut);
            m_gJumpMux.ConnectInput(4, JLTOut);
            m_gJumpMux.ConnectInput(5, JNEOut);
            m_gJumpMux.ConnectInput(6, JLEOut);
            m_gJumpMux.Inputs[7].Value = 1;
            //10. connect PC load control
            andForPCLoad.ConnectInput1(m_gJumpMux.Output[0]);
            andForPCLoad.ConnectInput2(Instruction[Type]);
            m_rPC.ConnectLoad(andForPCLoad.Output);
        }

        public override string ToString()
        {
            return "A=" + m_rA + ", D=" + m_rD + ", PC=" + m_rPC + ",Ins=" + Instruction;
        }

        private string GetInstructionString()
        {
            if (Instruction[Type].Value == 0)
                return "@" + Instruction.GetValue();
            return Instruction[Type].Value + "XX " +
               "a" + Instruction[A] + " " +
               "c" + Instruction[C1] + Instruction[C2] + Instruction[C3] + Instruction[C4] + Instruction[C5] + Instruction[C6] + " " +
               "d" + Instruction[D1] + Instruction[D2] + Instruction[D3] + " " +
               "j" + Instruction[J1] + Instruction[J2] + Instruction[J3];
        }

        //use this function in debugging to print the current status of the ALU. Feel free to add more things for printing.
        public void PrintState()
        {
            Console.WriteLine("CPU state:");
            Console.WriteLine("PC=" + m_rPC + "=" + m_rPC.Output.GetValue());
            Console.WriteLine("A=" + m_rA + "=" + m_rA.Output.GetValue());
            Console.WriteLine("D=" + m_rD + "=" + m_rD.Output.GetValue());
            Console.WriteLine("Ins=" + GetInstructionString());
            Console.WriteLine("ALU=" + m_gALU);
            Console.WriteLine("inM=" + MemoryInput);
            Console.WriteLine("outM=" + MemoryOutput);
            Console.WriteLine("addM=" + MemoryAddress);
        }
    }
}
