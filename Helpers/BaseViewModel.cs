using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Helpers
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        //public event PropertyChangedEventHandler PropertyChanged;

        //public void RaisePropertyChange(string propertyName)
        //{
        //    if (PropertyChanged != null)
        //    {
        //        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //    }
        //}
        protected virtual void SetProperty<T>(ref T member, T val,
         [CallerMemberName] string propertyName = null)
        {
            if (object.Equals(member, val)) return;

            member = val;
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        protected virtual void RaisePropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
