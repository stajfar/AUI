using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KinectControls
{
    class Util
    {
        public static TimeSpan TimeSpanFromMinSec(double min, double sec)
        {
            return TimeSpan.FromSeconds(min * 60 + sec);
        }
        public static TimeSpan TimeSpanDiff(double aMin, double aSec, double bMin, double bSec)
        {
            return TimeSpan.FromSeconds((aMin - bMin) * 60 + (aSec - bSec));
        }
    }
}
