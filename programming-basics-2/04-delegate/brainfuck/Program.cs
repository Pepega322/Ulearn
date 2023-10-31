using System;
using System.Linq;
using NUnitLite;

namespace func.brainfuck;

public class Program
{
	private const string sierpinskiTriangleBrainfuckProgram = @"
                                >
                               + +
                              +   -
                             [ < + +
                            +       +
                           + +     + +
                          >   -   ]   >
                         + + + + + + + +
                        [               >
                       + +             + +
                      <   -           ]   >
                     > + + >         > > + >
                    >       >       +       <
                   < <     < <     < <     < <
                  <   [   -   [   -   >   +   <
                 ] > [ - < + > > > . < < ] > > >
                [                               [
               - >                             + +
              +   +                           +   +
             + + [ >                         + + + +
            <       -                       ]       >
           . <     < [                     - >     + <
          ]   +   >   [                   -   >   +   +
         + + + + + + + +                 < < + > ] > . [
        -               ]               >               ]
       ] +             < <             < [             - [
      -   >           +   <           ]   +           >   [
     - < + >         > > - [         - > + <         ] + + >
    [       -       <       -       >       ]       <       <
   < ]     < <     < <     ] +     + +     + +     + +     + +
  +   .   +   +   +   .   [   -   ]   <   ]   +   +   +   +   +
";

	public static void Main(string[] args)
	{
		if (args.Contains("test"))
			new AutoRun().Execute(new string[0]); // Запуск тестов
		else
		{
			//Brainfuck.Run(sierpinskiTriangleBrainfuckProgram, Console.Read, Console.Write);
			//Console.WriteLine("Это была демонстрация Brainfuck на примере построения треугольника Серпинского");
            Console.WriteLine("Введите свой код на brainfuck:");
            var instructions = Console.ReadLine();
            Console.Write("Вывод: ");
            Brainfuck.Run(instructions, Console.Read, Console.Write);
        }
		Console.ReadLine();
	}
}