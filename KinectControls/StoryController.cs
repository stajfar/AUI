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


        public void startStory(int storyID, Action<String, String, String> after)
        {
            String img1 = listStory[storyID].choice[0].listKinectButton[0].imageURL;
            String img2 = listStory[storyID].choice[0].listKinectButton[1].imageURL;
            String img3 = listStory[storyID].choice[0].listKinectButton[2].imageURL;
            XmlHelper.Time timeBegin = listStory[storyID].time[0];
            XmlHelper.Time time = listStory[storyID].choice[0].listSpeech[0].time[0];
            String text = listStory[storyID].choice[0].listSpeech[0].text;
            Util.Runner.start(Util.timeSpanDiff(time, timeBegin), () => Util.speak(text));

            startStory(storyID, () => after.Invoke(img1, img2, img3));
        }

        public void startStory(int storyID, Action after)
        {
            this.storyID = storyID;
            XmlHelper.Time time = listStory[storyID].time[0];
            this.Stop();
            this.Position = Util.timeSpan(time);
            this.Play();
            double duration = listStory[0].duration;
            Util.Runner.start(duration, () => this.Pause());
            Util.Runner.start(duration, () => after.Invoke());

            Util.arduinoActions(listStory[storyID].arduinoActions[0], time);
        }

        // react based on the chosen Hover Button
        public void chosen(int p, Action after, Action before)
        {
            after.Invoke();
            XmlHelper.Time time = listStory[0].choice[0].listKinectButton[p].time[0];
            this.Position = Util.timeSpan(time);
            this.Play();
            double duration = listStory[0].choice[0].listKinectButton[p].duration;
            Util.arduinoActions(listStory[0].choice[0].listKinectButton[p].arduinoActions[0], time);
            Util.Runner.start(duration, this.Pause);

            String text = listStory[0].choice[0].listKinectButton[p].listSpeech[0].text;
            Util.Runner.start(duration, () => Util.speak(text));
        }
    }
}
