using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace NextBusDesktop.ViewModels
{
    public class NotificationBase : INotifyPropertyChanged
    {
        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string property = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;
            field = value;
            RaisePropertyChanged(property);
            return true;
        }

        protected bool SetProperty<T>(T currentValue, T newValue, Action doSet, [CallerMemberName] string property = null)
        {
            if (EqualityComparer<T>.Default.Equals(currentValue, newValue))
                return false;
            doSet();
            RaisePropertyChanged(property);
            return true;
        }

        protected void RaisePropertyChanged(string property) => 
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));

        public event PropertyChangedEventHandler PropertyChanged;
    }

    public class NotificationBase<T> : NotificationBase where T : class, new()
    {
        protected T Model;

        public NotificationBase(T value = null) => Model = value is null ? new T() : value;

        public static implicit operator T(NotificationBase<T> value) => value.Model;
    }
}
