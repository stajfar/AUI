using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Linq;

namespace KinectControls
{
    public class XmlHelper
    {
        public class Story
        {
            public int StoryID { get; set; }
            public string VidUrl { get; set; }
            public List<Time> time = new List<Time>();
            public double duration { get; set; }
            public List<ArduinoActions> arduinoActions = new List<ArduinoActions>();
            public List<Choice> choice = new List<Choice>();
        }
        public class ArduinoActions
        {
            public List<Fan> ListFan = new List<Fan>();
            public List<Led> ListLed = new List<Led>();
        }
        public class Time
        {
            public double Min { get; set; }
            public double Sec { get; set; }
        }
        public class Fan
        {
            public List<Time> time = new List<Time>();
            public Boolean onStatus { get; set; }
        }
        public class Led
        {
            public List<Time> time = new List<Time>();
            public int red { get; set; }
            public int green { get; set; }
            public int blue { get; set; }
        }
        public class Choice
        {
            public List<KinectButton> ListKinectButton = new List<KinectButton>();
        }
        public class KinectButton
        {
            public int BtnID { get; set; }
            public List<Time> time = new List<Time>();
            public int duration { get; set; }
            public String imageURL { get; set; }
            public List<ArduinoActions> arduinoActions = new List<ArduinoActions>();
        }

        public List<Story> GetStoryData(int storyID)
        {

            XElement xmlDoc = XElement.Load("../../../Stories.xml");//"C:/Users/saeed/Documents/GitHub/AUI/KinectControls/Stories.xml");
            var Stories = getStories(xmlDoc, storyID);
            return Stories.ToList();
        }


        private List<Story> getStories(XElement root, int StoryID)
        {
            return new List<Story>(from story in root.Descendants("story")
                   where Convert.ToInt32(story.Element("ID").Value) == StoryID
                   select new Story
                   {
                       StoryID = Convert.ToInt32(story.Element("ID").Value),
                       VidUrl = story.Element("VideoUrl").Value,
                       time = getTimes(story),
                       duration = Convert.ToInt32(story.Element("Duration").Value),
                       arduinoActions = getArduinoActions(story),
                       choice = getChoices(story)
                   });
        }

        private List<ArduinoActions> getArduinoActions(XElement root)
        {
            return new List<ArduinoActions>(from arduinoAction in root.Descendants("ArduinoActions")
                                            select new ArduinoActions
                                            {
                                                ListFan = getFans(arduinoAction),
                                                ListLed = getLeds(arduinoAction),

                                            });
        }
        private List<Fan> getFans(XElement root)
        {
            return new List<Fan>(from listFan in root.Descendants("Fan")
                                 select new Fan
                                 {
                                     time = new List<Time>(from time in listFan.Descendants("Time")
                                                           select new Time
                                                           {
                                                               Min = Convert.ToDouble(time.Element("Min").Value),
                                                               Sec = Convert.ToDouble(time.Element("Sec").Value)
                                                           }),
                                     onStatus = Convert.ToBoolean(listFan.Element("ON").Value)
                                 });
        }
        private List<Led> getLeds(XElement root)
        {
            return new List<Led>(from listLed in root.Descendants("Led")
                                 select new Led
                                 {
                                     time = new List<Time>(from time in listLed.Descendants("Time")
                                                           select new Time
                                                           {
                                                               Min = Convert.ToInt32(time.Element("Min").Value),
                                                               Sec = Convert.ToInt32(time.Element("Sec").Value)
                                                           }),
                                     red = Convert.ToInt32(listLed.Element("StatusRed").Value),
                                     green = Convert.ToInt32(listLed.Element("StatusGreen").Value),
                                     blue = Convert.ToInt32(listLed.Element("StatusBlue").Value)
                                 });
        }
        private List<Choice> getChoices(XElement root)
        {
            return new List<Choice>(from choice in root.Descendants("Choice")
                                    select new Choice
                                    {
                                        ListKinectButton = getKinectButtons(choice)
                                    });
        }

        private List<KinectButton> getKinectButtons(XElement root)
        {
            return new List<KinectButton>(from kinectButton in root.Descendants("KinectButton")
                                          select new KinectButton
                                          {
                                              BtnID = Convert.ToInt32(kinectButton.Element("KinecButtonID").Value),
                                              time = getTimes(kinectButton),
                                              duration = Convert.ToInt32(kinectButton.Element("Duration").Value),
                                              imageURL = kinectButton.Element("ImageURL").Value
                                          });
        }

        private List<Time> getTimes(XElement root)
        {
            return new List<Time>(from time in root.Descendants("Time")
                                  select new Time
                                  {
                                      Min = Convert.ToDouble(time.Element("Min").Value),
                                      Sec = Convert.ToDouble(time.Element("Sec").Value)
                                  });
        }
    }
}
