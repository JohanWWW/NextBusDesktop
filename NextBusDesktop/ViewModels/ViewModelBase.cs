using System;
using NextBusDesktop.Utilities;

namespace NextBusDesktop.ViewModels
{
    /// <summary>
    /// Provides base functionality for a view model
    /// </summary>
    public class ViewModelBase : NotificationBase
    {
        private ILog _logger;
        public ILog Logger
        {
            set => _logger = value;
        }

        private bool _isLoading;
        /// <summary>
        /// Notifies UI whether or not the view model is busy.
        /// </summary>
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        private bool _hasErrorOccurred;
        /// <summary>
        /// Notifies UI whether or not an error has occurred in the view model.
        /// </summary>
        public bool HasErrorOccurred
        {
            get => _hasErrorOccurred;
            set => SetProperty(ref _hasErrorOccurred, value);
        }

        public bool EnableLogging = false;

        protected void Log(string message)
        {
            if (EnableLogging)
                _logger?.Log(message);
        }

        protected void Log(string message, string category)
        {
            if (EnableLogging)
                _logger?.Log(message, category);
        }
    }

    /// <summary>
    /// Provides base functionality for a view model
    /// </summary>
    /// <typeparam name="T">Model which the ViewModel represents</typeparam>
    public class ViewModelBase<T> : NotificationBase<T> where T : class, new()
    {
        private ILog _logger;
        public ILog Logger
        {
            set => _logger = value;
        }

        private bool _isLoading;
        /// <summary>
        /// Notifies UI whether or not the view model is busy.
        /// </summary>
        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

        private bool _hasErrorOccurred;
        /// <summary>
        /// Notifies UI whether or not an error has occurred in the view model.
        /// </summary>
        public bool HasErrorOccurred
        {
            get => _hasErrorOccurred;
            set => SetProperty(ref _hasErrorOccurred, value);
        }

        public bool EnableLogging = false;

        public ViewModelBase(T value = default) : base(value)
        {
        }

        protected void Log(string message)
        {
            if (EnableLogging)
                _logger?.Log(message);
        }

        protected void Log(string message, string category)
        {
            if (EnableLogging)
                _logger?.Log(message, category);
        }
    }
}
