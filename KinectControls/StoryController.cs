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

        private int storyID;
        private List<XmlHelper.Story> listStory;

        public StoryController()
        {
            listStory = XmlHelper.GetStoryData();
        }

        private void Play(XmlHelper.Time time, double duration)
        {
            this.Position = Util.timeSpan(time);
            this.Play();
            Util.Runner.Start(duration, this.Pause);
        }

        public void StartStoryArduino(int storyID, Action<String> after)
        {
            //String img1 = listStory[storyID].choice[0].listKinectButton[0].imageURL;

            //StartStory(storyID, () => after.Invoke(img1));


            while (true)
            {
                double redx = 0;
                double greenx = 0;
                double bluex = 0;
                double clearx = 0;
                Util.arduinoColor(ref redx, ref greenx, ref bluex, ref clearx);
                double red = redx / (redx + greenx + bluex);
                double green = greenx / (redx + greenx + bluex);
                double blue = bluex / (redx + greenx + bluex);
                Console.Write("(");
                Console.Write("{0:F2}", red);
                Console.Write(",");
                Console.Write("{0:F2}", green);
                Console.Write(",");
                Console.Write("{0:F2}", blue);
                Console.Write(",");
                Console.Write("{0:F2}", clearx);
                Console.WriteLine(")");
                Thread.Sleep(2000);
            }
        }

        public void StartStoryKinect(int storyID, Action<String, String, String> after)
        {
            String img1 = listStory[storyID].choice[0].listKinectButton[0].imageURL;
            String img2 = listStory[storyID].choice[0].listKinectButton[1].imageURL;
            String img3 = listStory[storyID].choice[0].listKinectButton[2].imageURL;

            StartStory(storyID, () => after.Invoke(img1, img2, img3));
        }

        private void StartStory(int storyID, Action after)
        {
            XmlHelper.Time time = listStory[storyID].time[0];

            this.storyID = storyID;
            this.Position = Util.timeSpan(time);
            double duration = listStory[0].duration;
            this.Play(time, duration);

            Util.Runner.Start(duration, () => after.Invoke());

            Util.speak(listStory[storyID].choice[0].listSpeech[0], time);
            Util.arduinoActions(listStory[storyID].arduinoActions[0], time);
        }

        // react based on the chosen Hover Button
        public void ChosenKinect(int p, Action after, Action before)
        {
            after.Invoke();
            XmlHelper.Time time = listStory[0].choice[0].listKinectButton[p].time[0];
            double duration = listStory[0].choice[0].listKinectButton[p].duration;
            this.Play(time, duration);

            Util.speak(listStory[0].choice[0].listKinectButton[p].listSpeech[0], time);
            Util.arduinoActions(listStory[0].choice[0].listKinectButton[p].arduinoActions[0], time);
        }

        // react based on the chosen Hover Button
        public void ChosenArduino(Int32 red, Int32 green, Int32 blue, Action after)
        {
            // TODO P != 0
            XmlHelper.Time time = listStory[0].choice[0].listKinectButton[0].time[0];
            double duration = listStory[0].choice[0].listKinectButton[0].duration;
            this.Play(time, duration);

            Util.speak(listStory[0].choice[0].listKinectButton[0].listSpeech[0], time);
            Util.arduinoActions(listStory[0].choice[0].listKinectButton[0].arduinoActions[0], time);
        }
    }
}
