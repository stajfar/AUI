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
        private void forSeconds(double s, Action<object, EventArgs> after)
        {
            System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
            dispatcherTimer.Tick += new EventHandler(after);
            dispatcherTimer.Interval = Util.TimeSpanFromMinSec(0, s); //In seconds on how much time to play the video and then Pause
            dispatcherTimer.Start();
        }
        private void dispatcherTimer_Tick(object sender, EventArgs e)
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
            XmlHelper.KinectButton ButtonData = xmlhelper.GetButtonData(StoryID, 1, p);//SectionID==1
            List<XmlHelper.Story> Liststory = xmlhelper.GetStoryData(StoryID);
            if (Liststory.Count > 0)
            {
                double startMin = Liststory[0].choice[0].ListKinectButton[p].time[0].Min;
                double startSec = Liststory[0].choice[0].ListKinectButton[p].time[0].Sec;
                this.Position = TimeSpan.FromSeconds(ButtonData.Position);
                forSeconds(ButtonData.duration, before);
                this.Play();
            }


            /*
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
             */
        }
    }
}
