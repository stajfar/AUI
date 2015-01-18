using System;
namespace KinectControls
{
    public interface IMainWindow
    {
        void setButtonsBackground(string btn0URL);
        void setButtonsBackground(string btn0URL, string btn1URL, string btn2URL);
    }
}
