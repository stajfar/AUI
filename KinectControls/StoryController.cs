using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;
using Arduino1;

namespace KinectControls
{
    public class StoryController : MediaElement
    {

        private int storyID;

        private void arduinoFan(Boolean statusFan)
        {
            ArduinoSerialComm c1 = new ArduinoSerialComm();
            c1.SetComPort();
            Console.WriteLine(c1.portFound);
            Thread.Sleep(1000);
            if (statusFan)
            {
                c1.arduinoOut(4, 0);
            }
            else
            {
                c1.arduinoOut(4, 255);
            }
        }

        //is called to start the story with a given function "after" which will be called when the timer ticks
        public void startStory(int storyID, Action after)
        {
            this.storyID = storyID;
            XmlHelper xmlhelper = new XmlHelper();
            List<XmlHelper.Story> ListStory = xmlhelper.GetStoryData();
            if (ListStory.Count > 0)
            {
                XmlHelper.Time time = ListStory[0].time[0];
                this.Position = Util.timeSpan(time);
                Util.Runner.start(0, () => this.Play());
                double duration = ListStory[0].duration;
                Util.Runner.start(duration, () => this.Pause());
                Util.Runner.start(duration, () => after.Invoke());

                //// arduino
                //XmlHelper.Time startFanTime = ListStory[0].arduinoActions[0].ListFan[0].time[0];
                //Boolean fanStatus = ListStory[0].arduinoActions[0].ListFan[0].onStatus;
                //Util.Runner.start(0, () => arduinoFan(fanStatus));
                //Util.Runner.start(5, () => arduinoFan(false));
            }
        }
        // react based on the chosen Hover Button
        public void chosen(int p, Action after, Action before)
        {
            after.Invoke();
            XmlHelper xmlhelper = new XmlHelper();
            List<XmlHelper.Story> Liststory = xmlhelper.GetStoryData();
            if (Liststory.Count > 0)
            {
                XmlHelper.Time time = Liststory[0].choice[0].ListKinectButton[p].time[0];
                this.Position = Util.timeSpan(time);
                this.Play();
                double duration = Liststory[0].choice[0].ListKinectButton[p].duration;
                Util.Runner.start(duration, this.Pause);
            }
        }
    }
}
