using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;

namespace func.brainfuck
{
    public class BrainfuckBasicCommands
    {
        public static void RegisterTo(IVirtualMachine vm, Func<int> read, Action<char> write)
        {
            vm.RegisterCommand('<', f =>
            {
                var pointer = f.MemoryPointer;
                f.MemoryPointer = (pointer == 0) ? f.Memory.Length - 1 : --pointer;
            });
            vm.RegisterCommand('>', f =>
            {
                var pointer = f.MemoryPointer;
                f.MemoryPointer = (pointer == f.Memory.Length - 1) ? 0 : ++pointer;
            });
            vm.RegisterCommand('+', f =>
            {
                var b = f.Memory[f.MemoryPointer];
                f.Memory[f.MemoryPointer] = (b == byte.MaxValue) ? byte.MinValue : ++b;
            });
            vm.RegisterCommand('-', f =>
            {
                var b = f.Memory[f.MemoryPointer];
                f.Memory[f.MemoryPointer] = (b == byte.MinValue) ? byte.MaxValue : --b;
            });
            vm.RegisterCommand('.', f => { write((char)(f.Memory[f.MemoryPointer])); });
            vm.RegisterCommand(',', f => { f.Memory[f.MemoryPointer] = (byte)read(); });
            var symbols = GetSymbols();
            foreach (var symbol in symbols)
                vm.RegisterCommand(symbol, f => f.Memory[f.MemoryPointer] = (byte)symbol);
        }

        public static char[] GetSymbols()
        {
            var list = new List<char>();
            for (var s = 'a'; s <= 'z'; s++)
            {
                list.Add(char.ToLower(s));
                list.Add(char.ToUpper(s));
            }
            for (var s = '0'; s <= '9'; s++)
                list.Add(s);
            return list.ToArray();
        }
    }
}