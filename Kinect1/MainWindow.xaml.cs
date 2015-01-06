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
using Microsoft.Kinect.Toolkit;
using Microsoft.Kinect.Toolkit.BackgroundRemoval;
using System.Windows.Threading;


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


        /// <summary>
        /// Our core library which does background 
        /// </summary>
        private BackgroundRemovedColorStream backgroundRemovedColorStream;
        /// <summary>
        /// Track whether Dispose has been called
        /// </summary>
        private bool disposed;

        /// <summary>
        /// Bitmap that will hold color information
        /// </summary>
        private WriteableBitmap foregroundBitmap;

        /// <summary>
        /// Active Kinect sensor
        /// </summary>
        private KinectSensorChooser sensorChooser;
        /// <summary>
        /// the skeleton that is currently tracked by the app
        /// </summary>
        private int currentlyTrackedSkeletonId;


        public MainWindow()
        {
            InitializeComponent();
            // initialize the sensor chooser and UI
            this.sensorChooser = new KinectSensorChooser();
            this.sensorChooserUi.KinectSensorChooser = this.sensorChooser;
            this.sensorChooser.KinectChanged += this.SensorChooserOnKinectChanged;
            this.sensorChooser.Start();

            
        }
        /// <summary>
        /// Finalizes an instance of the MainWindow class.
        /// This destructor will run only if the Dispose method does not get called.
        /// </summary>
        ~MainWindow()
        {
            this.Dispose(false);
        }
        /// <summary>
        /// Dispose the allocated frame buffers and reconstruction.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);

            // This object will be cleaned up by the Dispose method.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Frees all memory associated with the FusionImageFrame.
        /// </summary>
        /// <param name="disposing">Whether the function was called from Dispose.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (null != this.backgroundRemovedColorStream)
                {
                    this.backgroundRemovedColorStream.Dispose();
                    this.backgroundRemovedColorStream = null;
                }

                this.disposed = true;
            }
        }
        private void activeButton()
        {
            btn1.Opacity = 1;
            btn1.IsEnabled = true;
            btn2.Opacity = 1;
            btn2.IsEnabled = true;
            btn3.Opacity = 1;
            btn3.IsEnabled = true;
        }
        private void disableButton()
        {
            btn1.Opacity = 0;
            btn1.IsEnabled = false;
            btn2.Opacity = 0;
            btn2.IsEnabled = false;
            btn3.Opacity = 0;
            btn3.IsEnabled = false;
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            this.Cursor = Cursors.Hand;
            disableButton();
            btn1.setbackground("../../../Images/cacke.jpg");
            btn2.setbackground("../../../Images/cacke.jpg");
            btn3.setbackground("../../../Images/cacke.jpg");
            myMediaElement.startStory(1,activeButton);//StroryID==1

            string test=btn1.Image;
            
        }

        /// <summary>
        /// Handle the background removed color frame ready event. The frame obtained from the background removed
        /// color stream is in RGBA format.
        /// </summary>
        /// <param name="sender">object that sends the event</param>
        /// <param name="e">argument of the event</param>
        private void BackgroundRemovedFrameReadyHandler(object sender, BackgroundRemovedColorFrameReadyEventArgs e)
        {
            using (var backgroundRemovedFrame = e.OpenBackgroundRemovedColorFrame())
            {
                if (backgroundRemovedFrame != null)
                {
                    if (null == this.foregroundBitmap || this.foregroundBitmap.PixelWidth != backgroundRemovedFrame.Width
                        || this.foregroundBitmap.PixelHeight != backgroundRemovedFrame.Height)
                    {
                        this.foregroundBitmap = new WriteableBitmap(backgroundRemovedFrame.Width, backgroundRemovedFrame.Height, 96.0, 96.0, PixelFormats.Bgra32, null);

                        // Set the image we display to point to the bitmap where we'll put the image data
                        this.MaskedColor.Source = this.foregroundBitmap;
                    }

                    // Write the pixel data into our bitmap
                    this.foregroundBitmap.WritePixels(
                        new Int32Rect(0, 0, this.foregroundBitmap.PixelWidth, this.foregroundBitmap.PixelHeight),
                        backgroundRemovedFrame.GetRawPixelData(),
                        this.foregroundBitmap.PixelWidth * sizeof(int),
                        0);
                }
            }
        }

        /// <summary>
        /// Called when the KinectSensorChooser gets a new sensor
        /// </summary>
        /// <param name="sender">sender of the event</param>
        /// <param name="args">event arguments</param>
        private void SensorChooserOnKinectChanged(object sender, KinectChangedEventArgs args)
        {
            if (args.OldSensor != null)
            {
                try
                {
                    args.OldSensor.AllFramesReady -= this.sensor_AllFramesReady;
                    args.OldSensor.DepthStream.Disable();
                    args.OldSensor.ColorStream.Disable();
                    args.OldSensor.SkeletonStream.Disable();

                    // Create the background removal stream to process the data and remove background, and initialize it.
                    if (null != this.backgroundRemovedColorStream)
                    {
                        this.backgroundRemovedColorStream.BackgroundRemovedFrameReady -= this.BackgroundRemovedFrameReadyHandler;
                        this.backgroundRemovedColorStream.Dispose();
                        this.backgroundRemovedColorStream = null;
                        sensor = null;
                    }
                }
                catch (InvalidOperationException)
                {
                    // KinectSensor might enter an invalid state while enabling/disabling streams or stream features.
                    // E.g.: sensor might be abruptly unplugged.
                }
            }

            if (args.NewSensor != null)
            {
                try
                {
                    sensor = args.NewSensor;
                    args.NewSensor.DepthStream.Enable(DepthImageFormat.Resolution320x240Fps30);
                    args.NewSensor.ColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30);
                    args.NewSensor.SkeletonStream.Enable();

                    this.backgroundRemovedColorStream = new BackgroundRemovedColorStream(args.NewSensor);
                    this.backgroundRemovedColorStream.Enable(ColorImageFormat.RgbResolution640x480Fps30, DepthImageFormat.Resolution320x240Fps30);

                    

                    // Add an event handler to be called when the background removed color frame is ready, so that we can
                    // composite the image and output to the app
                    this.backgroundRemovedColorStream.BackgroundRemovedFrameReady += this.BackgroundRemovedFrameReadyHandler;

                    // Add an event handler to be called whenever there is new depth frame data
                    args.NewSensor.AllFramesReady += this.sensor_AllFramesReady;
                                        
                }
                catch (InvalidOperationException)
                {
                    // KinectSensor might enter an invalid state while enabling/disabling streams or stream features.
                    // E.g.: sensor might be abruptly unplugged.
                }
            }
        }
       

        /// <summary>
        /// Execute shutdown tasks
        /// </summary>
        /// <param name="sender">object sending the event</param>
        /// <param name="e">event arguments</param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {           
            this.sensorChooser.Stop();
            this.sensorChooser = null;
        }


        void sensor_AllFramesReady(object sender, AllFramesReadyEventArgs e)
        {
            Skeleton me = null;
            using (ColorImageFrame colorframe = e.OpenColorImageFrame())
            {
                if (colorframe == null)
                    return;
                // to remove the background 
                this.backgroundRemovedColorStream.ProcessColor(colorframe.GetRawPixelData(), colorframe.Timestamp);
               
            }
           
            Getskeleton(e, ref me);
            if (me == null)
                return;
            GetCameraPoint(e, ref me);
            this.ChooseSkeleton();

        }


        private void Getskeleton(AllFramesReadyEventArgs e, ref Skeleton me)
        {
            using (SkeletonFrame skeletonframeDATA = e.OpenSkeletonFrame())
            {
                if (skeletonframeDATA == null)
                    return;
                skeletonframeDATA.CopySkeletonDataTo(allskeletons);
                //to remove background
                this.backgroundRemovedColorStream.ProcessSkeleton(this.allskeletons, skeletonframeDATA.Timestamp);
                //Select the Tracked skeleton
                me = (from s in allskeletons where s.TrackingState == SkeletonTrackingState.Tracked select s).FirstOrDefault();

            }
        }

        private void GetCameraPoint(AllFramesReadyEventArgs e, ref Skeleton me)
        {
            using (DepthImageFrame depthimageframeDATA = e.OpenDepthImageFrame())
            {
                if (depthimageframeDATA == null || sensor == null)
                    return;
                //to remove the background
                this.backgroundRemovedColorStream.ProcessDepth(depthimageframeDATA.GetRawPixelData(), depthimageframeDATA.Timestamp);

                CoordinateMapper coordinatemapper = new CoordinateMapper(sensor);
                DepthImagePoint headDepthPoint = coordinatemapper.MapSkeletonPointToDepthPoint(me.Joints[JointType.Head].Position, DepthImageFormat.Resolution640x480Fps30);
                DepthImagePoint LefthandDepthPoint = coordinatemapper.MapSkeletonPointToDepthPoint(me.Joints[JointType.HandLeft].Position, DepthImageFormat.Resolution640x480Fps30);
                DepthImagePoint RighthandDepthPoint = coordinatemapper.MapSkeletonPointToDepthPoint(me.Joints[JointType.HandRight].Position, DepthImageFormat.Resolution640x480Fps30);
                DepthImagePoint HipCenterDepthPoint = coordinatemapper.MapSkeletonPointToDepthPoint(me.Joints[JointType.HipCenter].Position, DepthImageFormat.Resolution640x480Fps30);



                ColorImagePoint headcolorPoint = coordinatemapper.MapDepthPointToColorPoint(DepthImageFormat.Resolution640x480Fps30, headDepthPoint, ColorImageFormat.InfraredResolution640x480Fps30);
                ColorImagePoint lefthandcolorPoint = coordinatemapper.MapDepthPointToColorPoint(DepthImageFormat.Resolution640x480Fps30, LefthandDepthPoint, ColorImageFormat.InfraredResolution640x480Fps30);
                ColorImagePoint righthandcolorPoint = coordinatemapper.MapDepthPointToColorPoint(DepthImageFormat.Resolution640x480Fps30, RighthandDepthPoint, ColorImageFormat.InfraredResolution640x480Fps30);
                ColorImagePoint HipCentercolorPoint = coordinatemapper.MapDepthPointToColorPoint(DepthImageFormat.Resolution640x480Fps30, HipCenterDepthPoint, ColorImageFormat.InfraredResolution640x480Fps30);


                



                Joint handJoint = me.Joints[JointType.HandRight];

                // hand.SetPosition(handJoint);
                Canvas.SetLeft(elips1, HipCentercolorPoint.X - elips1.Width / 2);
                Canvas.SetTop(elips1, 240);
                btn1.Check(elips1, me.Joints[JointType.HipCenter].Position.Z);
                btn2.Check(elips1, me.Joints[JointType.HipCenter].Position.Z);
                btn3.Check(elips1, me.Joints[JointType.HipCenter].Position.Z);

            }

        }

        /// <summary>
        /// Use the sticky skeleton logic to choose a player that we want to set as foreground. This means if the app
        /// is tracking a player already, we keep tracking the player until it leaves the sight of the camera, 
        /// and then pick the closest player to be tracked as foreground.
        /// </summary>
        private void ChooseSkeleton()
        {
            var isTrackedSkeltonVisible = false;
            var nearestDistance = float.MaxValue;
            var nearestSkeleton = 0;

            foreach (var skel in this.allskeletons)
            {
                if (null == skel)
                {
                    continue;
                }

                if (skel.TrackingState != SkeletonTrackingState.Tracked)
                {
                    continue;
                }

                if (skel.TrackingId == this.currentlyTrackedSkeletonId)
                {
                    isTrackedSkeltonVisible = true;
                    break;
                }

                if (skel.Position.Z < nearestDistance)
                {
                    nearestDistance = skel.Position.Z;
                    nearestSkeleton = skel.TrackingId;
                }
            }

            if (!isTrackedSkeltonVisible && nearestSkeleton != 0)
            {
                this.backgroundRemovedColorStream.SetTrackedPlayer(nearestSkeleton);
                this.currentlyTrackedSkeletonId = nearestSkeleton;
            }
        }

        private void after(object sender, EventArgs e)
        {
            disableButton();
            myMediaElement.startStory(2,activeButton);
            myMediaElement.chosen(1, disableButton, disableButton);
        }

        private void chose1(object sender, EventArgs e)
        {
            myMediaElement.chosen(1, disableButton, disableButton);
        }

        private void chose2(object sender, EventArgs e)
        {
            myMediaElement.chosen(2, disableButton, disableButton);
        }

        private void chose3(object sender, EventArgs e)
        {
            myMediaElement.chosen(3, disableButton, disableButton);
        }

       

       
    }
}
