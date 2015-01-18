using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;


namespace Kinect1
{
    public partial class MainWindow : Window
    {
        private int SelectedStory;
        private kinect1Window k1win;
        public MainWindow(int SelectedStoryToStart)
        {
            SelectedStory = SelectedStoryToStart;
            InitializeComponent();
            k1win = new kinect1Window(this);
            this.sensorChooserUi.KinectSensorChooser = k1win.sensorChooser;
        }
        private void activeButton()
        {
            btn0.Opacity = 1;
            btn0.IsEnabled = true;
            btn1.Opacity = 1;
            btn1.IsEnabled = true;
            btn2.Opacity = 1;
            btn2.IsEnabled = true;
        }
        private void disableButton()
        {
            btn0.Opacity = 0;
            btn0.IsEnabled = false;
            btn1.Opacity = 0;
            btn1.IsEnabled = false;
            btn2.Opacity = 0;
            btn2.IsEnabled = false;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            disableButton();
            myMediaElement.StartStoryKinect(SelectedStory, setButtonsBackground);
            //myMediaElement.StartStoryArduino(1, setButtonsBackground);//StroryID==1
            //myMediaElement.foo();
        }

        private void setButtonsBackground(String btn0URL)
        {
            activeButton();
            btn0.setbackground(btn0URL);
        }
        private void setButtonsBackground(String btn0URL, String btn1URL, String btn2URL)
        {
            activeButton();
            btn0.setbackground(btn0URL);
            btn1.setbackground(btn1URL);
            btn2.setbackground(btn2URL);
        }

        private void story1()
        {
            myMediaElement.StartStoryArduino(1, setButtonsBackground);
        }

        private void chosen0(object sender, EventArgs e)
        {
            myMediaElement.Chosen(0, story1);
            if (myMediaElement.rightChoice)
            {
                disableButton();
            }
        }

        private void chosen1(object sender, EventArgs e)
        {
            myMediaElement.Chosen(1, story1);
            if (myMediaElement.rightChoice)
            {
                disableButton();
            }
        }

        private void chosen2(object sender, EventArgs e)
        {
            myMediaElement.Chosen(2, story1);
            if (myMediaElement.rightChoice)
            {
                disableButton();
            }
        }
    }
}
