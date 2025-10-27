using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using InTheHand.Net.Sockets;
using InTheHand.Net.Bluetooth;

class MorseProgram
{
    static int unit = 200; // ms per Morse unit
    static Dictionary<int, string[]> program = new();
    static BluetoothClient btClient;
    static System.IO.Stream btStream;

    static void Main()
    {
        Console.WriteLine("Enter Arduino Bluetooth MAC (like 001122334455):");
        string mac = Console.ReadLine();
        SetupBluetooth(mac);

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
            bool jumped = false;

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
                        line = int.Parse(tokens[++i]);
                        jumped = true;
                        break;
                }

                if (jumped) break;
            }

            if (!jumped)
            {
                line += 10;
            }
        }

        btStream?.Close();
        btClient?.Close();
    }

    static void SetupBluetooth(string macAddress)
    {
        BluetoothAddress address = BluetoothAddress.Parse(macAddress);
        Guid serviceClass = BluetoothService.SerialPort;

        btClient = new BluetoothClient();
        btClient.Connect(new BluetoothEndPoint(address, serviceClass));
        btStream = btClient.GetStream();

        Console.WriteLine("Connected to Arduino via Bluetooth.");
    }

    static void SendSignal(int units)
    {
        Console.WriteLine($"Signal ON for {units} units");
        if (btStream != null)
        {
            byte[] on = Encoding.ASCII.GetBytes("1");
            btStream.Write(on, 0, on.Length);
        }

        Thread.Sleep(units * unit);

        Console.WriteLine("Signal OFF");
        if (btStream != null)
        {
            byte[] off = Encoding.ASCII.GetBytes("0");
            btStream.Write(off, 0, off.Length);
        }
    }

    static void Pause(int units)
    {
        Console.WriteLine($"Pause {units} units");
        Thread.Sleep(units * unit);
    }
}
