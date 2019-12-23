using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextBusDesktop.ViewModels
{
    /// <summary>
    /// Provides base functionality for a view model
    /// </summary>
    public class ViewModelBase : NotificationBase
    {
        public bool EnableLogging = true;

        /// <summary>
        /// Cleans resources or anything that might be running when this instance is no longer in use.
        /// </summary>
        protected virtual void Deconstruct()
        {
        }

        /// <summary>
        /// Should be called when the user has left the view represented by this view model.
        /// </summary>
        public void OnViewLeave()
        {
            Deconstruct();
            System.Diagnostics.Debug.WriteLineIf(EnableLogging, $"Deconstruct -> {this}", "Info");
        }
    }

    /// <summary>
    /// Provides base functionality for a view model
    /// </summary>
    /// <typeparam name="T">Model which the ViewModel represents</typeparam>
    public class ViewModelBase<T> : NotificationBase<T> where T : class, new()
    {
        public bool EnableLogging = true;

        public ViewModelBase(T value = default) : base(value)
        {
        }

        /// <summary>
        /// Cleans resources or anything that might be running when this instance is no longer in use.
        /// </summary>
        protected virtual void Deconstruct()
        {
        }

        /// <summary>
        /// Should be called when the user has left the view represented by this view model.
        /// </summary>
        public void OnViewLeave()
        {
            Deconstruct();
            System.Diagnostics.Debug.WriteLineIf(EnableLogging, $"Deconstruct -> {this}", "Info");
        }
    }
}
