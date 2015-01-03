using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;

namespace KinectControls
{
    public class StoryController : MediaElement
    {
        //create a timer dispacher to call the given funtion "after" and also "dispatcherTimer_Tick" Events
        private void forSeconds(double s, Action<object, EventArgs> after)
        {
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Tick += new EventHandler(after);
            dispatcherTimer.Interval = TimeSpan.FromSeconds(s); //In seconds on how much time to play the video and then Pause
            dispatcherTimer.Start();
        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            //pause playback and stop the timer
            this.Pause();
            (sender as DispatcherTimer).Stop();
        }
        //is called to start the story with a given function "after" which will be called when the timer ticks
        public void startStory1(Action<object, EventArgs> after)
        {
            this.Position = new TimeSpan(0, 2, 30);
            forSeconds(19, after);
            this.Play();
        }
        // react based on the chosen Hover Button
        public void chosen(int p, Action<object, EventArgs> after, Action<object, EventArgs> before)
        {
            after.Invoke(null, null);
            if (p == 1)
            {
                this.Position = TimeSpan.FromSeconds(169);
                forSeconds(15, before);
            }
            if (p == 2)
            {
                this.Position = TimeSpan.FromSeconds(179);
                forSeconds(5, before);
            }
            if (p == 3)
            {
            }
            this.Play();
        }
    }
}
