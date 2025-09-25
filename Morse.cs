using System;
using System.Collections.Generic;
using System.Threading;

class MorseProgram
{
    static int unit = 200; // ms per Morse unit
    static Dictionary<int, string[]> program = new();

    static void Main()
    {
        // Example "program"
        program[10] = new[] { "sot", "dot", "intra", "dot", "intra", "dash", "short", "dot", "intra", "dot", "intra", "dash", "eot" };
        program[20] = new[] { "goto", "10" };

        int line = 10;

        while (true)
        {
            if (!program.ContainsKey(line))
            {
                Console.WriteLine($"Line {line} not found. Stopping.");
                break;
            }

            string[] tokens = program[line];
            bool jumped = false; // tracks if we did a goto

            for (int i = 0; i < tokens.Length; i++)
            {
                switch (tokens[i])
                {
                    case "sot":
                        Console.WriteLine("Start transmission");
                        break;

                    case "eot":
                        Console.WriteLine("End transmission");
                        break;

                    case "dot":
                        SendSignal(1);
                        break;

                    case "dash":
                        SendSignal(3);
                        break;

                    case "intra":
                        Pause(1);
                        break;

                    case "short":
                        Pause(3);
                        break;

                    case "long":
                        Pause(7);
                        break;

                    case "goto":
                        line = int.Parse(tokens[++i]); // move to target line
                        jumped = true;
                        break;
                }

                if (jumped) break; // stop processing rest of line
            }

            if (!jumped)
            {
                line += 10; // go to next line
            }
        }
    }

    static void SendSignal(int units)
    {
        Console.WriteLine($"Signal ON for {units} units");
        // TODO: Bluetooth send ON
        Thread.Sleep(units * unit);

        Console.WriteLine("Signal OFF");
        // TODO: Bluetooth send OFF
    }

    static void Pause(int units)
    {
        Console.WriteLine($"Pause {units} units");
        Thread.Sleep(units * unit);
    }
}
