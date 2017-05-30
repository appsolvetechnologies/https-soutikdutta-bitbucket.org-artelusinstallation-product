using Helpers;
using System;
using System.ComponentModel;
using System.Windows.Media.Imaging;

namespace Artelus.Model
{
    public class CameraEntity : BaseViewModel, IDataErrorInfo
    {
        private string eye;
        public string Eye
        {
            get { return eye; }
            set
            {
                if (eye != value)
                {
                    eye = value;
                    RaisePropertyChange("Eye");
                }
            }
        }
        private BitmapImage captureStream;
        private BitmapImage liveStream;
        public BitmapImage LiveStream
        {
            get { return liveStream; }
            set
            {
                if (liveStream != value)
                {
                    liveStream = value;
                    RaisePropertyChange("LiveStream");
                }
            }
        }
        private BitmapImage leftPosterior;
        public BitmapImage LeftPosterior
        {
            get { return leftPosterior; }
            set
            {
                if (leftPosterior != value)
                {
                    leftPosterior = value;
                    RaisePropertyChange("LeftPosterior");
                }
            }
        }
        private BitmapImage rightPosterior;
        public BitmapImage RightPosterior
        {
            get { return rightPosterior; }
            set
            {
                if (rightPosterior != value)
                {
                    rightPosterior = value;
                    RaisePropertyChange("RightPosterior");
                }
            }
        }
        private BitmapImage leftAnterior;
        public BitmapImage LeftAnterior
        {
            get { return leftAnterior; }
            set
            {
                if (leftAnterior != value)
                {
                    leftAnterior = value;
                    RaisePropertyChange("LeftAnterior");
                }
            }
        }
        private BitmapImage rightAnterior;
        public BitmapImage RightAnterior
        {
            get { return rightAnterior; }
            set
            {
                if (rightAnterior != value)
                {
                    rightAnterior = value;
                    RaisePropertyChange("RightAnterior");
                }
            }
        }
        public BitmapImage CaptureStream
        {
            get { return captureStream; }
            set
            {
                if (captureStream != value)
                {
                    captureStream = value;
                    RaisePropertyChange("CaptureStream");
                }
            }
        }

        public string this[string columnName]
        {
            get
            {
                return Validate(columnName);
            }
        }
        public string Error
        {
            get
            {
                throw new NotImplementedException();
            }
        }
        private string Validate(string properyName)
        {
            string validationMessgae = string.Empty;
            return validationMessgae;
        }
    }

    public class PredictionEntity
    {
        public int current { get; set; }
        public string result { get; set; }
        public string state { get; set; }
        public string status { get; set; }
        public int total { get; set; }
    }
}
