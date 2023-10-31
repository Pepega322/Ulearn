using System;
using System.Collections.Generic;

namespace func.brainfuck
{
    public class VirtualMachine : IVirtualMachine
    {
        public string Instructions { get; }
        public int InstructionPointer { get; set; }
        public byte[] Memory { get; }
        public int MemoryPointer { get; set; }
        private Dictionary<char, Action<IVirtualMachine>> commands;


        public VirtualMachine(string program, int memorySize)
        {
            Instructions = program;
            Memory = new byte[memorySize];
            InstructionPointer = 0;
            MemoryPointer = 0;
            commands = new Dictionary<char, Action<IVirtualMachine>>();
        }

        public void RegisterCommand(char symbol, Action<IVirtualMachine> execute)
        {
            if (!commands.ContainsKey(symbol)) commands.Add(symbol, execute);
        }

        public void Run()
        {
            while (InstructionPointer < Instructions.Length)
            {
                var command = Instructions[InstructionPointer];
                if (commands.ContainsKey(command)) commands[command](this);
                InstructionPointer++;
            }
        }
    }
}