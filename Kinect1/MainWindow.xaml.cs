using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Kinect;

namespace Kinect1
{

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        KinectSensor sensor;
        const int SKELETON_COUNT = 6;
        Skeleton[] allskeletons = new Skeleton[SKELETON_COUNT];

        public MainWindow()
        {
            InitializeComponent();
        }

        private void HoverButton_Click(object sender, EventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            sensor = KinectSensor.KinectSensors[0];// kinect = new Runtime();
            sensor.ColorStream.Enable();
            sensor.DepthStream.Enable();
            sensor.SkeletonStream.Enable();
            //sensor.SkeletonFrameReady += new EventHandler<SkeletonFrameReadyEventArgs>(kinect_SkeletonFrameReady);
            sensor.AllFramesReady += new EventHandler<AllFramesReadyEventArgs>(sensor_AllFramesReady);
            this.Cursor = Cursors.None;
            sensor.Start();
        }
        /*
                void kinect_SkeletonFrameReady(object sender, SkeletonFrameReadyEventArgs e)
                {
                    Skeleton me = null;
                    using (SkeletonFrame skeletonframeDATA = e.OpenSkeletonFrame())
                    {
                        if (skeletonframeDATA == null)
                            return;
                        skeletonframeDATA.CopySkeletonDataTo(allskeletons);
                        me = (from s in allskeletons where s.TrackingState == SkeletonTrackingState.Tracked select s).FirstOrDefault();
                    }
                    if (me == null)
                        return;
                    Joint handJoint = me.Joints[JointType.HandRight];
                    hand.SetPosition(handJoint);
                   // btn1.Check(hand);
                }  */

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (sensor != null)
                sensor.Stop();

            sensor = null;
        }


        void sensor_AllFramesReady(object sender, AllFramesReadyEventArgs e)
        {
            using (ColorImageFrame colorframe = e.OpenColorImageFrame())
            {
                if (colorframe == null)
                    return;

                byte[] pixels = new byte[colorframe.PixelDataLength];
                colorframe.CopyPixelDataTo(pixels);

                int stride = colorframe.Width * 4;
                vid.Source = BitmapSource.Create(colorframe.Width, colorframe.Height, 96, 96, PixelFormats.Bgr32, null, pixels, stride);
            }
            Skeleton me = null;
            Getskeleton(e, ref me);
            if (me == null)
                return;
            GetCameraPoint(e, ref me);

        }


        private void Getskeleton(AllFramesReadyEventArgs e, ref Skeleton me)
        {
            using (SkeletonFrame skeletonframeDATA = e.OpenSkeletonFrame())
            {
                if (skeletonframeDATA == null)
                    return;
                skeletonframeDATA.CopySkeletonDataTo(allskeletons);

                me = (from s in allskeletons where s.TrackingState == SkeletonTrackingState.Tracked select s).FirstOrDefault();

            }
        }

        private void GetCameraPoint(AllFramesReadyEventArgs e, ref Skeleton me)
        {
            using (DepthImageFrame depthimageframeDATA = e.OpenDepthImageFrame())
            {
                if (depthimageframeDATA == null || sensor == null)
                    return;
                CoordinateMapper coordinatemapper = new CoordinateMapper(sensor);
                DepthImagePoint headDepthPoint = coordinatemapper.MapSkeletonPointToDepthPoint(me.Joints[JointType.Head].Position, DepthImageFormat.Resolution640x480Fps30);
                DepthImagePoint LefthandDepthPoint = coordinatemapper.MapSkeletonPointToDepthPoint(me.Joints[JointType.HandLeft].Position, DepthImageFormat.Resolution640x480Fps30);
                DepthImagePoint RighthandDepthPoint = coordinatemapper.MapSkeletonPointToDepthPoint(me.Joints[JointType.HandRight].Position, DepthImageFormat.Resolution640x480Fps30);
                DepthImagePoint HipCenterDepthPoint = coordinatemapper.MapSkeletonPointToDepthPoint(me.Joints[JointType.HipCenter].Position, DepthImageFormat.Resolution640x480Fps30);



                ColorImagePoint headcolorPoint = coordinatemapper.MapDepthPointToColorPoint(DepthImageFormat.Resolution640x480Fps30, headDepthPoint, ColorImageFormat.InfraredResolution640x480Fps30);
                ColorImagePoint lefthandcolorPoint = coordinatemapper.MapDepthPointToColorPoint(DepthImageFormat.Resolution640x480Fps30, LefthandDepthPoint, ColorImageFormat.InfraredResolution640x480Fps30);
                ColorImagePoint righthandcolorPoint = coordinatemapper.MapDepthPointToColorPoint(DepthImageFormat.Resolution640x480Fps30, RighthandDepthPoint, ColorImageFormat.InfraredResolution640x480Fps30);
                ColorImagePoint HipCentercolorPoint = coordinatemapper.MapDepthPointToColorPoint(DepthImageFormat.Resolution640x480Fps30, HipCenterDepthPoint, ColorImageFormat.InfraredResolution640x480Fps30);


                //Deneey found
                // Select the hand closer to the sensor.
                var activeHand = me.Joints[JointType.HandRight].Position.Z <= me.Joints[JointType.HandLeft].Position.Z ? me.Joints[JointType.HandRight] : me.Joints[JointType.HandLeft];

                // Get the hand's position relatively to the color image.
                var position = sensor.CoordinateMapper.MapSkeletonPointToColorPoint(
                                                        activeHand.Position,
                                                        ColorImageFormat.RgbResolution640x480Fps30);



                Joint handJoint = me.Joints[JointType.HandRight];

                // hand.SetPosition(handJoint);
                Canvas.SetLeft(elips1, HipCentercolorPoint.X - elips1.Width / 2);
                Canvas.SetTop(elips1, 240);
                btn1.Check(elips1, me.Joints[JointType.HipCenter].Position.Z);
                btn2.Check(elips1, me.Joints[JointType.HipCenter].Position.Z);
                btn3.Check(elips1, me.Joints[JointType.HipCenter].Position.Z);

            }

        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            myMediaElement.Play();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            myMediaElement.Pause();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            myMediaElement.Position = TimeSpan.FromSeconds(11.1);
            myMediaElement.Play();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            myMediaElement.Position = TimeSpan.FromSeconds(31.1);
            myMediaElement.Play();
        }
    }
}
