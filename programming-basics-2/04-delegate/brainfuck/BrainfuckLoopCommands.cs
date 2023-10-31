using System;
using System.Collections.Generic;
using System.Reflection;

namespace func.brainfuck
{
    public class BrainfuckLoopCommands
    {
        public static void RegisterTo(IVirtualMachine vm)
        {
            var brackets = new Brackets(vm.Instructions);
            vm.RegisterCommand('[', f =>
            {
                var b = vm.Memory[vm.MemoryPointer];
                if (b == byte.MinValue)
                    vm.InstructionPointer = brackets.GetCloseBracketPosition(vm.InstructionPointer);
            });
            vm.RegisterCommand(']', f =>
            {
                var b = vm.Memory[vm.MemoryPointer];
                if (b != byte.MinValue)
                    vm.InstructionPointer = brackets.GetOpenBracketPosition(vm.InstructionPointer);
            });
        }
    }

    public class Brackets
    {
        private Dictionary<int, int> openToClose;
        private Dictionary<int, int> closeToOpen;

        public Brackets(string programm)
        {
            openToClose = new Dictionary<int, int>();
            closeToOpen = new Dictionary<int, int>();
            Initialize(programm);
        }

        public int GetOpenBracketPosition(int closeBracket) => closeToOpen[closeBracket];
        public int GetCloseBracketPosition(int openBracket) => openToClose[openBracket];

        private void Initialize(string programm)
        {
            var stack = new Stack<int>();
            for (var i = 0; i < programm.Length; i++)
            {
                switch (programm[i])
                {
                    case '[':
                        stack.Push(i);
                        break;
                    case ']':
                        if (stack.Count == 0)
                            throw new Exception("Missing open bracket");
                        var open = stack.Pop();
                        openToClose.Add(open, i);
                        closeToOpen.Add(i, open);
                        break;
                    default:
                        continue;
                }
            }
            if (stack.Count != 0)
                throw new Exception("Missing close bracket");
        }
    }
}