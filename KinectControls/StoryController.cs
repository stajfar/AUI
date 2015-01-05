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
        private int StoryID;
        //create a timer dispacher to call the given funtion "after" and also "dispatcherTimer_Tick" Events
        private void forSeconds(double sec, Action<object, EventArgs> after)
        {
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(stopDispatcherAndPauseVideo);
            dispatcherTimer.Tick += new EventHandler(after);
            dispatcherTimer.Interval = Util.TimeSpanFromMinSec(0, sec); //In seconds on how much time to play the video and then Pause
            dispatcherTimer.Start();
        }
        private void stopDispatcherAndPauseVideo(object sender, EventArgs e)
        {
            //pause playback and stop the timer
            this.Pause();
            (sender as DispatcherTimer).Stop();
        }
        //is called to start the story with a given function "after" which will be called when the timer ticks
        public void startStory(int Storyid, Action<object, EventArgs> after)
        {
            StoryID = Storyid;
            XmlHelper xmlhelper = new XmlHelper();
            List<XmlHelper.Story> Liststory = xmlhelper.GetStoryData(StoryID);
            if (Liststory.Count > 0)
            {
                double startMin = Liststory[0].time[0].Min;
                double startSec = Liststory[0].time[0].Sec;
                double duration = Liststory[0].duration;
                this.Position = Util.TimeSpanFromMinSec(startMin, startSec);
                forSeconds(duration, after);
                this.Play();
            }
        }
        // react based on the chosen Hover Button
        public void chosen(int p, Action<object, EventArgs> after, Action<object, EventArgs> before)
        {
            after.Invoke(null, null);

            XmlHelper xmlhelper = new XmlHelper();
            List<XmlHelper.Story> Liststory = xmlhelper.GetStoryData(StoryID);
            if (Liststory.Count > 0)
            {
                double startMin = Liststory[0].choice[0].ListKinectButton[p].time[0].Min;
                double startSec = Liststory[0].choice[0].ListKinectButton[p].time[0].Sec;
                this.Position = Util.TimeSpanFromMinSec(startMin, startSec);
                forSeconds(ButtonData.duration, before);
                this.Play();
            }
        }
    }
}
