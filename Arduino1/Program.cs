using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Arduino1
{
    class Program
    {
        static void Main(string[] args)
        {
            ArduinoSerialComm arduinoSerial = new ArduinoSerialComm();
            arduinoSerial.SetComPort();
            Console.WriteLine(arduinoSerial.portFound);

            int red = 0;
            int green = 0;
            int blu = 0;
            int clear = 0;
            while (true)
            {/*
                arduinoSerial.arduinoOut(4, 0);
                Thread.Sleep(5000);
                arduinoSerial.arduinoOut(4, 255);
                Thread.Sleep(5000);
                arduinoSerial.arduinoOut(3, 255);
                Thread.Sleep(5000);
                arduinoSerial.arduinoOut(3, 255, 0, 0);
                Thread.Sleep(5000);
                arduinoSerial.arduinoOut(3, 0, 255, 0);
                Thread.Sleep(5000);
                arduinoSerial.arduinoOut(3, 0, 0, 255);
                Thread.Sleep(5000);*/
                arduinoSerial.arduinoReadColor(ref red, ref green, ref blu, ref clear);
                Thread.Sleep(5000);
            }
#if DEBUG
            Console.WriteLine("Press enter to close...");
            Console.ReadLine();
#endif
        }
    }
}
