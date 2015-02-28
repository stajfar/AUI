using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Threading;
using Microsoft.Speech.Synthesis;
using Arduino1;
using System.Diagnostics;

namespace KinectControls
{
    public class Util
    {
        public static void setPCSpeaker()
        {
            Process notePad = new Process();
            notePad.StartInfo.FileName = "..\\..\\..\\..\\changeAudio.exe";
            notePad.StartInfo.Arguments = "0";
            notePad.StartInfo.UseShellExecute = false;
            notePad.StartInfo.CreateNoWindow = true;
            notePad.Start();
        }
        public static void setBTSpeaker()
        {
            Process notePad = new Process();
            notePad.StartInfo.FileName = "..\\..\\..\\..\\changeAudio.exe";
            notePad.StartInfo.Arguments = "1";
            notePad.StartInfo.UseShellExecute = false;
            notePad.StartInfo.CreateNoWindow = true;
            notePad.Start();
        }
        public static void speak(XmlHelper.Speech speech, XmlHelper.Time beginTime)
        {
            XmlHelper.Time time = speech.time[0];
            String text = speech.text;
            Util.Runner.Start(Util.timeSpanDiff(time, beginTime), () => Util.speak(text));
        }


        public static void speak(String text)
        {
            SpeechSynthesizer synth = new SpeechSynthesizer();
            synth.GetInstalledVoices();
            // Configure the audio output.
            synth.SetOutputToWaveFile(@"C:\test\Rate.wav");
            synth.SelectVoice("Microsoft Server Speech Text to Speech Voice (en-US, Helen)");
            synth.Rate = 0;
            synth.Volume = 100;
            PromptBuilder prbuilder = new PromptBuilder();
            // Create a SoundPlayer instance to play the output audio file.
            System.Media.SoundPlayer m_SoundPlayer =
              new System.Media.SoundPlayer(@"C:\test\Rate.wav");
            synth.Speak(text);
            m_SoundPlayer.Play();
            synth.Dispose();
        }

        public static void arduinoActions(XmlHelper.ArduinoActions arduinoActions, XmlHelper.Time beginTime)
        {
            List<XmlHelper.Fan> listFan = arduinoActions.listFan;
            foreach (XmlHelper.Fan fan in listFan)
            {
                XmlHelper.Time startFanTime = fan.time[0];
                Boolean fanStatus = fan.onStatus;
                Util.Runner.Start(Util.timeSpanDiff(startFanTime, beginTime), () => Util.arduinoFanBody(fanStatus));
            }

            List<XmlHelper.Led> listLed = arduinoActions.listLed;
            foreach (XmlHelper.Led led in listLed)
            {
                XmlHelper.Time startLedTime = led.time[0];
                Int32 red = led.red;
                Int32 green = led.green;
                Int32 blue = led.blue;
                Util.Runner.Start(Util.timeSpanDiff(startLedTime, beginTime), () => Util.arduinoLedLFace(red, green, blue));
                Util.Runner.Start(Util.timeSpanDiff(startLedTime, beginTime), () => Util.arduinoLedRFace(red, green, blue));
            }
        }

        public static void arduinoColor(Action<int, Action> Chosen, XmlHelper.Choice choice)
        {
            double red = 0;
            double green = 0;
            double blue = 0;
            double clear = 0;
            Boolean statusOk = false;
            Util.arduinoColor(ref red, ref green, ref blue, ref clear);
            List<XmlHelper.KinectButton> listKinectButton = choice.listKinectButton;
            Console.WriteLine("color: ");
            Console.WriteLine(clear);
            Console.WriteLine(red);
            Console.WriteLine(green);
            Console.WriteLine(blue);
            for (int i = 0; i < listKinectButton.Count; i++)
            {
               XmlHelper.KinectButton kinectButton = listKinectButton[i];
               if (clear > 0 && red > kinectButton.listColor[0].red && green > kinectButton.listColor[0].green && blue > kinectButton.listColor[0].blue)
                ///////if(true)
                {
                    kinectButton.btnID = i;
                    Chosen(kinectButton.btnID, () => { statusOk = true; });
                }
            }
            if ( statusOk == false )
            {
                Util.Runner.Start(0.1, () => arduinoColor(Chosen, choice));

            }
        }

        public static void arduinoColor(ref double red, ref double green, ref double blue, ref double clear)
        {
            ArduinoSerialComm.initializeConn();
            Console.WriteLine(ArduinoSerialComm.portFound);
            if (!ArduinoSerialComm.portFound)
            {
                return;
            }
            double r = 0;
            double g = 0;
            double b = 0;
            ArduinoSerialComm.arduinoReadColor(ref r, ref g, ref b, ref clear);
            r += 0;
            g += 0;
            b += 0;
            r *= 1.7;
            g *= 1.1;
            b *= 1.5;
            red = r / (r + g + b) * 100;
            green = g / (r + g + b) * 100;
            blue = b / (r + g + b) * 100;
        }

        public static void arduinoLedLFace(Int32 red, Int32 green, Int32 blue)
        {
            ArduinoSerialComm.initializeConn();
            Console.WriteLine(ArduinoSerialComm.portFound);
            if (!ArduinoSerialComm.portFound)
            {
                return;
            }
            ArduinoSerialComm.arduinoOut(3, red, green, blue);
        }

        public static void arduinoLedRFace(Int32 red, Int32 green, Int32 blue)
        {
            ArduinoSerialComm.initializeConn();
            Console.WriteLine(ArduinoSerialComm.portFound);
            if (!ArduinoSerialComm.portFound)
            {
                return;
            }
            ArduinoSerialComm.arduinoOut(4, red, green, blue);
        }

        public static void arduinoLedBody(Int32 red, Int32 green, Int32 blue)
        {
            ArduinoSerialComm.initializeConn();
            Console.WriteLine(ArduinoSerialComm.portFound);
            if (!ArduinoSerialComm.portFound)
            {
                return;
            }
            ArduinoSerialComm.arduinoOut(5, red, green, blue);
        }

        public static void arduinoFanBody(Boolean statusFan)
        {
            ArduinoSerialComm.initializeConn();
            Console.WriteLine(ArduinoSerialComm.portFound);
            if (!ArduinoSerialComm.portFound)
            {
                return;
            }
            if (statusFan)
            {
                ArduinoSerialComm.arduinoOut(6, 0);
            }
            else
            {
                ArduinoSerialComm.arduinoOut(6, 255);
            }
        }

        public static TimeSpan timeSpan(XmlHelper.Time time)
        {
            return timeSpan(time.min, time.sec);
        }
        public static TimeSpan timeSpan(double min, double sec)
        {
            return TimeSpan.FromSeconds(min * 60 + sec);
        }
        public static TimeSpan timeSpanDiff(XmlHelper.Time aTime, XmlHelper.Time btime)
        {
            return timeSpanDiff(aTime.min, aTime.sec, btime.min, btime.sec);
        }
        public static TimeSpan timeSpanDiff(double aMin, double aSec, double bMin, double bSec)
        {
            return TimeSpan.FromSeconds((aMin - bMin) * 60 + (aSec - bSec));
        }

        public class Runner
        {
            public static void Start(double time, Action action)
            {
                Start(Util.timeSpan(0, time), action);
            }
            public static void Start(XmlHelper.Time time, Action action)
            {
                Start(Util.timeSpan(time), action);
            }
            public static void Start(TimeSpan time, Action action)
            {
                DispatcherTimer timer = new DispatcherTimer();
                timer.Interval = time;
                timer.Start();
                timer.Tick += (o, args) =>
                {
                    timer.Stop();
                    action.Invoke();
                };
            }
        }

        public void KinectGusture()
        {
            
        }
    }
}
