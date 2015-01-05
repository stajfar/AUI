using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace KinectControls
{
    class Util
    {
        public static TimeSpan timeSpan(XmlHelper.Time time)
        {
            return timeSpan(time.Min, time.Sec);
        }
        public static TimeSpan timeSpan(double min, double sec)
        {
            return TimeSpan.FromSeconds(min * 60 + sec);
        }
        public static TimeSpan timeSpanDiff(XmlHelper.Time aTime, XmlHelper.Time btime)
        {
            return timeSpanDiff(aTime.Min, aTime.Sec, btime.Min, btime.Sec);
        }
        public static TimeSpan timeSpanDiff(double aMin, double aSec, double bMin, double bSec)
        {
            return TimeSpan.FromSeconds((aMin - bMin) * 60 + (aSec - bSec));
        }

        public class Runner
        {
            private Action action;
            private TimeSpan time;

            public static void start(double time, Action action)
            {
                new Runner(Util.timeSpan(0, time), action);
            }
            public static void start(XmlHelper.Time time, Action action)
            {
                new Runner(Util.timeSpan(time), action);
            }
            public static void start(TimeSpan time, Action action)
            {
                new Runner(time, action);
            }
            private Runner(TimeSpan time, Action action)
            {
                this.action = action;
                this.time = time;
                Thread newThread = new Thread(new ThreadStart(run));
                newThread.Start();
            }
            private void run()
            {
                Thread.Sleep(time);
                action.Invoke();
            }
        }
    }
}
