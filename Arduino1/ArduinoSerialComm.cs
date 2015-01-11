using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Arduino1
{
    public class ArduinoSerialComm
    {
        private ArduinoSerialComm() { }
        private static SerialPort currentPort;

        public static bool portFound { get; private set; }

        public static void initializeConn()
        {
            if (!portFound)
            {
                SetComPort();
            }
        }
        private static void SetComPort()
        {
            string[] ports = SerialPort.GetPortNames();
            foreach (string port in ports)
            {
                currentPort = new SerialPort(port, 9600);
                if (DetectArduino())
                {
                    portFound = true;
                    break;
                }
                else
                {
                    portFound = false;
                }
            }
        }
        private static bool DetectArduino()
        {
            try
            {
                //The below setting are for the Hello handshake
                byte[] buffer = new byte[7];
                buffer[0] = Convert.ToByte(16);
                buffer[1] = Convert.ToByte(128);
                buffer[2] = Convert.ToByte(0);
                buffer[3] = Convert.ToByte(0);
                buffer[4] = Convert.ToByte(0);
                buffer[5] = Convert.ToByte(0);
                buffer[6] = Convert.ToByte(4);
                int intReturnASCII = 0;
                char charReturnValue = (Char)intReturnASCII;
                currentPort.Open();
                currentPort.Write(buffer, 0, 7);
                Thread.Sleep(100);
                int count = currentPort.BytesToRead;
                string returnMessage = "";
                while (count > 0)
                {
                    intReturnASCII = currentPort.ReadByte();
                    returnMessage = returnMessage + Convert.ToChar(intReturnASCII);
                    count--;
                }
                //ComPort.name = returnMessage;
                currentPort.Close();
                return returnMessage.Contains("HELLO FROM ARDUINO");
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public static void arduinoReadColor(ref double red, ref double green, ref double blu, ref double clear)
        {

            byte[] buffer = new byte[7];
            currentPort.Open();
            buffer[0] = Convert.ToByte(16);
            buffer[1] = Convert.ToByte(101);
            buffer[2] = Convert.ToByte(123);
            buffer[3] = Convert.ToByte(0);
            buffer[4] = Convert.ToByte(0);
            buffer[5] = Convert.ToByte(0);
            buffer[6] = Convert.ToByte(4);
            currentPort.Write(buffer, 0, 7);

            Thread.Sleep(20);
            int count = currentPort.BytesToRead;
            if (count > 0)
            {
                red = currentPort.ReadByte();
                green = currentPort.ReadByte();
                blu = currentPort.ReadByte();
                clear = currentPort.ReadByte();
                Console.WriteLine();
            }

            currentPort.Close();
            Thread.Sleep(20);
        }

        public static void arduinoOut(int port, int value)
        {

            byte[] buffer = new byte[7];
            currentPort.Open();
            buffer[0] = Convert.ToByte(16);
            buffer[1] = Convert.ToByte(100);
            buffer[2] = Convert.ToByte(port);
            buffer[3] = Convert.ToByte(value);
            buffer[4] = Convert.ToByte(value);
            buffer[5] = Convert.ToByte(value);
            buffer[6] = Convert.ToByte(4);
            currentPort.Write(buffer, 0, 7);
            currentPort.Close();
            Thread.Sleep(20);
        }

        public static void arduinoOut(int port, int value1, int value2, int value3)
        {

            byte[] buffer = new byte[7];
            currentPort.Open();
            buffer[0] = Convert.ToByte(16);
            buffer[1] = Convert.ToByte(100);
            buffer[2] = Convert.ToByte(port);
            buffer[3] = Convert.ToByte(value1);
            buffer[4] = Convert.ToByte(value2);
            buffer[5] = Convert.ToByte(value3);
            buffer[6] = Convert.ToByte(4);
            currentPort.Write(buffer, 0, 7);
            currentPort.Close();
            Thread.Sleep(20);
        }
    }
}
