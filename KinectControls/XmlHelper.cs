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

        public List<Story> GetStoryData(int StoryID)
        {

            XElement xmlDoc = XElement.Load("../../../Stories.xml");//"C:/Users/saeed/Documents/GitHub/AUI/KinectControls/Stories.xml");
            var Stories = from story in xmlDoc.Descendants("story")
             where Convert.ToInt32(story.Element("ID").Value) == StoryID
             select new Story
             {
                 StoryID = Convert.ToInt32(story.Element("ID").Value),
                 VidUrl = story.Element("VideoUrl").Value,
                 time = new List<Time>(from time in story.Descendants("Time")
                 select new Time
                 {
                     Min = Convert.ToDouble(time.Element("Min").Value),
                     Sec = Convert.ToDouble(time.Element("Sec").Value)
                 }),
                 duration = Convert.ToInt32(story.Element("Duration").Value),
                 arduinoActions = new List<ArduinoActions>(from arduinoAction in story.Descendants("ArduinoActions")
                                  select new ArduinoActions
                                  {
                                      ListFan = new List<Fan>(from listFan in arduinoAction.Descendants("Fan")
                                                              select new Fan
                                                              {
                                                                  time = new List<Time>(from time in listFan.Descendants("Time")
                                                                  select new Time
                                                                  {
                                                                      Min = Convert.ToDouble(time.Element("Min").Value),
                                                                      Sec = Convert.ToDouble(time.Element("Sec").Value)
                                                                  }),
                                                                  onStatus = Convert.ToBoolean(listFan.Element("ON").Value)
                                                              }),
                                      ListLed = new List<Led>(from listLed in arduinoAction.Descendants("Led")
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
                                                              }),
                                       
                                  }),
                                  choice = new List<Choice>(from choice in story.Descendants("Choice")
                                           select new Choice
                                           {
                                                ListKinectButton = new List<KinectButton>(from kinectButton in choice.Descendants("KinectButton")
                                                                   select new KinectButton
                                                                   {
                                                                            BtnID = Convert.ToInt32(kinectButton.Element("KinecButtonID").Value),
                                                                            time = new List<Time>(from time in kinectButton.Descendants("Time")
                                                                            select new Time
                                                                            {
                                                                                Min = Convert.ToDouble(time.Element("Min").Value),
                                                                                Sec = Convert.ToDouble(time.Element("Sec").Value)
                                                                            }),
                                                                            duration = Convert.ToInt32(kinectButton.Element("Duration").Value),
                                                                            imageURL = kinectButton.Element("ImageURL").Value
                                                                   })

                                           })
             };
            return Stories.ToList();
        }

        
        internal KinectButton GetButtonData(int StoryID, int SectionID, int ButtonID)
        {/*
            XElement xmlDoc = XElement.Load("../../../Stories.xml");
            var Stories =
             from story in xmlDoc.Descendants("story")
             where Convert.ToInt32(story.Element("ID").Value) == StoryID
             select new Story
             {
                 ListOfSections = new List<Sections>(from sections in story.Descendants("Sections")
                                                     select new Sections
                                                     {
                                                         ListSection = new List<Section>(from section in sections.Descendants("Section")
                                                                                         where Convert.ToInt32(section.Element("SectionID").Value) == SectionID
                                                                                         select new Section
                                                                                         {
                                                                                             SectionID = Convert.ToInt32(section.Element("SectionID").Value),
                                                                                             ListKinectButton = new List<KinectButton>(from kinectButton in section.Descendants("KinectButton")
                                                                                                                                       where Convert.ToInt32(kinectButton.Element("KinecButtonID").Value) == ButtonID
                                                                                                                                       select new KinectButton
                                                                                                                                       {
                                                                                                                                           BtnID = Convert.ToInt32(kinectButton.Element("KinecButtonID").Value),
                                                                                                                                           ForSecond = Convert.ToInt32(kinectButton.Element("ForSecond").Value),
                                                                                                                                           Position = Convert.ToInt32(kinectButton.Element("Position").Value)
                                                                                                                                       })

                                                                                         })

                                                     })


             };
            
            return Stories.ToList()[0].ListOfSections[0].ListSection[0].ListKinectButton[0];
            */
            return null;
        }
    }
}
