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
using System.Windows.Shapes;

namespace Kinect1
{
    /// <summary>
    /// Interaction logic for StorySelection.xaml
    /// </summary>
    public partial class StorySelection : Window
    {
        public StorySelection()
        {
            InitializeComponent();
        }

        private void Button_Click_DoorStory(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            MainWindow win;
            switch (btn.Name)
            {
                case "btn0":
                     win = new MainWindow(0);
                     win.Show();
                     this.Close();
                    break;
                case "btn1":
                    win = new MainWindow(1);
                     win.Show();
                     this.Close();
                    break;
                case "btn2":
                    win = new MainWindow(2);
                     win.Show();
                     this.Close();
                    break;
                case "btn3":
                    win = new MainWindow(3);
                     win.Show();
                     this.Close();
                    break;
                case "btn4":
                    win = new MainWindow(4);
                     win.Show();
                     this.Close();
                    break;
                case "btn5":
                    win = new MainWindow(5);
                     win.Show();
                     this.Close();
                    break;
            }
            
           
            
            
        }
    }
}
