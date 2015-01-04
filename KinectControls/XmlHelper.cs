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
            public int InitialMinute { get; set; }
            public int InitialSecond { get; set; }
            public int InitialDurationInSecond { get; set; }
            public string VidUrl { get; set; }
            public List<Sections> ListOfSections = new List<Sections>();
        }
        public class Sections
        {
           public List<Section> ListSection = new List<Section>();
        }

        public class Section
        {
            public int SectionID{ get; set; }
           public List<KinectButton> ListKinectButton = new List<KinectButton>();
        }
        public class KinectButton
        {
            public int BtnID { get; set; }
            public int Position { get; set; }
            public int ForSecond { get; set; }
        }

        public List<Story> GetStoryData(int StoryID)
        {
           
            XElement xmlDoc = XElement.Load("../../../Stories.xml");//"C:/Users/saeed/Documents/GitHub/AUI/KinectControls/Stories.xml");
            var Stories =
             from story in xmlDoc.Descendants("story")
             where Convert.ToInt32(story.Element("ID").Value) == StoryID
             select new Story 
             {
                 StoryID = Convert.ToInt32(story.Element("ID").Value),
                 InitialMinute = Convert.ToInt32(story.Element("InitialMin").Value),
                 InitialSecond = Convert.ToInt32(story.Element("InitialSecond").Value),
                 InitialDurationInSecond = Convert.ToInt32(story.Element("InitialDurationInSecond").Value),
                 VidUrl = story.Element("VideoUrl").Value,
                 ListOfSections = new List<Sections>(from sections in story.Descendants("Sections")
                                                     select new Sections
                                                     {
                                                         ListSection = new List<Section>(from section in sections.Descendants("Section")
                                                                                         select new Section
                                                                                         {
                                                                                             SectionID = Convert.ToInt32(section.Element("SectionID").Value),
                                                                                             ListKinectButton = new List<KinectButton>(from kinectButton in section.Descendants("KinectButton")
                                                                                                                                       select new KinectButton
                                                                                                                                       {
                                                                                                                                           BtnID = Convert.ToInt32(kinectButton.Element("KinecButtonID").Value),
                                                                                                                                           ForSecond = Convert.ToInt32(kinectButton.Element("ForSecond").Value),
                                                                                                                                           Position = Convert.ToInt32(kinectButton.Element("Position").Value)
                                                                                                                                       })

                                                                                         })

                                                     })


             };
            return Stories.ToList();
        }


        internal KinectButton GetButtonData(int StoryID,int SectionID,int ButtonID)
        {
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
        }
    }
}
