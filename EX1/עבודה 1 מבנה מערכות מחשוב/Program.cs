using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Components
{
    class Program
    {
        static void Main(string[] args)
        {

            OrGate or = new OrGate();
            XorGate xor = new XorGate();
            AndGate and = new AndGate();
            MuxGate mux = new MuxGate();
            Demux demux = new Demux();
            HalfAdder halfAdder = new HalfAdder();
            FullAdder fullAdder = new FullAdder();
            WireSet wireSet = new WireSet(9);
            BitwiseAndGate bwag = new BitwiseAndGate(2);
            BitwiseNotGate bwng = new BitwiseNotGate(3);
            BitwiseOrGate bwog = new BitwiseOrGate(2);
            BitwiseMux bwm = new BitwiseMux(2);
            BitwiseDemux bwd = new BitwiseDemux(2);
            MultiBitAndGate mbag = new MultiBitAndGate(4);
            MultiBitAdder mba = new MultiBitAdder(3);
            BitwiseMultiwayMux bwmwm = new BitwiseMultiwayMux(5, 4);
            BitwiseMultiwayDemux bwmwd = new BitwiseMultiwayDemux(4, 4);
            SingleBitRegister sbr = new SingleBitRegister();
            MultiBitRegister mbr = new MultiBitRegister(4);
            if (!sbr.TestGate())
                 Console.WriteLine("SingleBitRegisterbugbug");
            if (!mbr.TestGate())
                 Console.WriteLine("MultiBitRegisterbugbug");
            ALU alu = new ALU(4);
            if (!alu.TestGate())
                Console.WriteLine("ALUbugbug");
            if (!bwmwd.TestGate())
                Console.WriteLine("BitwiseMultiwayDemuxbugbug");
            if (!bwmwm.TestGate())
                Console.WriteLine("BitwiseMultiwayMuxbugbug");
            if (!mba.TestGate())
                Console.WriteLine("MultiBitAdderbugbug");
            if (!mbag.TestGate())
                Console.WriteLine("MultiBitAndGatebugbug");
            if (!bwd.TestGate())
                Console.WriteLine("BitWiseDemuxbugbug");
            if (!bwm.TestGate())
                Console.WriteLine("BitWiseMuxbugbug");
             if (!bwog.TestGate())
                Console.WriteLine("BitWiseOrGatebugbug");
             if (!bwng.TestGate())
                Console.WriteLine("BitWiseNotGatebugbug");
             if (!bwag.TestGate())
                Console.WriteLine("BitWiseAndGatebugbug");
            wireSet.SetValue(137);
            wireSet.Set2sComplement(-32);
            if (!and.TestGate())
                Console.WriteLine("andbugbug");
            if (!or.TestGate ())
                Console.WriteLine("orbugbug");
            if (!xor.TestGate())
                Console.WriteLine("xorbugbug");
            if (!mux.TestGate())
                Console.WriteLine("muxbugbug");
            if (!demux.TestGate())
                Console.WriteLine("demuxbugbug");
            if (!halfAdder.TestGate())
                Console.WriteLine("HAbugbug");
            if (!fullAdder.TestGate())
                Console.WriteLine("FAbugbug");
            Memory memory = new Memory(2,6);
            if (!memory.TestGate())
                Console.WriteLine("Membugbug");
            Console.WriteLine("done");
            Console.ReadLine();
        }
    }
}
