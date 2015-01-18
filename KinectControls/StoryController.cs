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
        private IMainWindow imw;

        public StoryController()
        {
            listStory = XmlHelper.GetStoryData();
        }

        private void Play(XmlHelper.Time time, double duration)
        {

            //Player1.Close();
            //Player1.Source = uri;
            this.Play();
            Util.Runner.Start(0.01, () => this.Position = Util.timeSpan(time) );
            Util.Runner.Start(duration + 0.02, this.Pause);
        }

        public void StartStory(int storyID, IMainWindow imw)
        {
            this.imw = imw;
            XmlHelper.Choice choise = listStory[storyID].choice[0];
            switch (choise.type)
            {
                case "KinectButton":
                    String img1 = listStory[storyID].choice[0].listKinectButton[0].imageURL;
                    String img2 = listStory[storyID].choice[0].listKinectButton[1].imageURL;
                    String img3 = listStory[storyID].choice[0].listKinectButton[2].imageURL;
                    StartStory(storyID, () => imw.setButtonsBackground(img1, img2, img3));
                    break;
                case "Arduino":
                    StartStory(storyID, () => Util.arduinoColor(Chosen, choise));
                    break;
                case "KinectGesture":
                    StartStory(storyID, () => { });
                    break;
            }
        }

        private void StartStory(int storyID, Action after)
        {
            XmlHelper.Time time = listStory[storyID].time[0];

            this.storyID = storyID;
            this.Position = Util.timeSpan(time);
            double duration = listStory[storyID].duration;
            this.Play(time, duration);

            Util.Runner.Start(duration, () => after.Invoke());
            Util.Runner.Start(duration, () => isEnableChoise = true );
            timeEnable = true;

            Util.speak(listStory[storyID].choice[0].listSpeech[0], time);
            //Util.arduinoActions(listStory[storyID].arduinoActions[0], time);
        }

        Boolean timeEnable, isEnableChoise;
        public Boolean rightChoice;
        // react based on the chosen Hover Button
        public void Chosen(int p, Action after)
        {
            if (timeEnable)
            {
                timeEnable = false;
                Util.Runner.Start(3, () => timeEnable = true );
            } else 
            {
                return;
            }
            rightChoice = listStory[storyID].choice[0].listKinectButton[p].rightChoice;
            if (!isEnableChoise)
            {
                return;
            } else if (rightChoice)
            {
                isEnableChoise = false;
            }
            XmlHelper.Time time = listStory[storyID].choice[0].listKinectButton[p].time[0];
            double duration = listStory[storyID].choice[0].listKinectButton[p].duration;
            this.Play(time, duration);

            Util.speak(listStory[storyID].choice[0].listKinectButton[p].listSpeech[0], time);
           // Util.arduinoActions(listStory[storyID].choice[0].listKinectButton[p].arduinoActions[0], time);
            if (rightChoice)
            {
                //Util.Runner.Start(duration + 5, after);
            }
        }

        public void foo() // color debug
        {
            double red = 0;
            double green = 0;
            double blue = 0;
            double clear = 0;
            Util.arduinoColor(ref red, ref green, ref blue, ref clear);
            Console.Write("(");
            Console.Write("{0:F2}", red);
            Console.Write(",");
            Console.Write("{0:F2}", green);
            Console.Write(",");
            Console.Write("{0:F2}", blue);
            Console.Write(",");
            Console.Write("{0:F2}", clear);
            Console.WriteLine(")");
            Util.Runner.Start(0.5, foo);
        }
    }
}
