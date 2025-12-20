// Checks
// - [x] If register C contains 9, the program 2,6 would set register B to 1.
// - [x] If register A contains 10, the program 5,0,5,1,5,4 would output 0,1,2.
// - [x] If register A contains 2024, the program 0,1,5,4,3,0 would output 4,2,5,6,7,7,7,7,3,1,0 and leave 0 in register A.
// - [x] If register B contains 29, the program 1,7 would set register B to 26.
// - [x] If register B contains 2024 and register C contains 43690, the program 4,0 would set register B to 44354.

using System.Diagnostics;

ulong A = 0;
var watch = new Stopwatch();
watch.Start();

var computer = new CachedComputer
{
    State = new(A: 22_571_680),
    Program = new int[] { 2, 4, 1, 3, 7, 5, 0, 3, 4, 3, 1, 5, 5, 5, 3, 0 },
};

// var digit = computer.Program.Count() - 1;
// bool interactive = false;
// int attempts = 0;

// // START:
// A = interactive ? 216133739406848L : (ulong)Math.Pow(8, digit);

// // A = 216133739380736L;
// while (digit >= 0)
// {
//     computer.Reset(new State(A: A));
//     computer.Execute(false);
//     if (interactive)
//     {
//         Console.Clear();
//         Console.WriteLine($"A: {A} / {Convert.ToString((long)A, 8)}");
//         Console.WriteLine($"val: {string.Join(',', computer.Output)}");
//         Console.WriteLine($"exp: {string.Join(',', computer.Program)}");
//         var cursorPosition = 5 + (2 * digit);
//         Console.WriteLine(new string(' ', cursorPosition) + '*');

//         var key = Console.ReadKey().Key;
//         if (key == ConsoleKey.UpArrow)
//         {
//             A += (ulong)Math.Pow(8, digit);
//         }
//         else if (key == ConsoleKey.DownArrow)
//         {
//             A -= (ulong)Math.Pow(8, digit);
//         }
//         else if (key == ConsoleKey.RightArrow)
//         {
//             digit++;
//         }
//         else if (key == ConsoleKey.LeftArrow)
//         {
//             digit--;
//             digit = Math.Max(digit, 0);
//         }
//         else if (key != ConsoleKey.Y && key != ConsoleKey.Enter)
//         {
//             break;
//         }

//         continue;
//     }

// #if false
//     var expected = computer.Program.Skip(digit);
//     var found = computer.Output.Skip(digit);
//     if (expected.SequenceEqual(found) == false)
// #else
//     var expected = computer.Program[digit];
//     var found = computer.Output[digit];

//     if (expected != found)
// #endif
//     {
//         A += (ulong)Math.Pow(8, digit);
//         attempts++;
//     }
//     else
//     {
//         Console.WriteLine(
//             $"found index {digit.ToString().PadLeft(2)} {expected} after {attempts} attempts!\nCurrent A: {A} (hex: {Convert.ToString((long)A, 8)})"
//         );

//         if (digit < computer.Program.Count() - 1)
//         {
//             var otherExpected = computer.Program.Skip(digit + 1).First();
//             var otherFound = computer.Output.Skip(digit + 1).First();
//             // did the next one break?
//             if (otherExpected != otherFound)
//             {
//                 Console.WriteLine($"it broke the others, took {attempts} attempts");
//                 // Console.WriteLine( $"OUTPUT:\nval: {string.Join(',', computer.Output.Skip(digit))}\nexp: {string.Join(',', computer.Program.Skip(digit))}");
//                 Console.WriteLine(
//                     $"OUTPUT:\nval: {string.Join(',', computer.Output)}\nexp: {string.Join(',', computer.Program)}"
//                 );
//                 // return;
//                 A -= (ulong)attempts * (ulong)Math.Pow(8, digit);
//                 // break;
//                 A -= (ulong)attempts * (ulong)Math.Pow(8, digit + 1);
//                 continue;
//             }
//         }

//         attempts = 0;
//         digit--;
//     }

//     if (digit == -1)
//     {
//         Console.WriteLine(
//             $"FINAL:\nval: {string.Join(',', computer.Output)}\nexp: {string.Join(',', computer.Program)}"
//         );
//         digit = computer.Program.Count() - 1;
//     }
//     // continue;
// }

// computer.Reset(new(A));
// computer.Execute();
// Console.WriteLine(
//     $"FINAL:\nval: {string.Join(',', computer.Output)}\nexp: {string.Join(',', computer.Program)}"
// );

TreeSearch(computer.Program, 15, 0);

void TreeSearch(int[] Program, int digit, ulong A)
{
    var computer = new CachedComputer { Program = Program, State = new(A) };

    if (digit < 0)
    {
        computer.Execute();
        Console.WriteLine(
            $"FINAL: {A}"
            // $"FINAL:\nA: {A}\nval: {string.Join(',', computer.Output)}\nexp: {string.Join(',', computer.Program)}"
        );

        return;
    }

    var expected = Program[digit];
    for (ulong i = 0; i < 8; i++)
    {
        ulong newA = (ulong)(A + (i * Math.Pow(8, digit)));
        computer.Reset(new(newA));
        computer.Execute();

        if (computer.Output.Count() < Program.Count())
            continue;

        if (computer.Output[digit] == expected)
            TreeSearch(Program, digit - 1, newA);
    }
}
