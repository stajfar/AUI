using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Threading;

namespace KinectControls
{
    public class Util
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
            public static void start(double time, Action action)
            {
                start(Util.timeSpan(0, time), action);
            }
            public static void start(XmlHelper.Time time, Action action)
            {
                start(Util.timeSpan(time), action);
            }
            public static void start(TimeSpan time, Action action)
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
    }
}
