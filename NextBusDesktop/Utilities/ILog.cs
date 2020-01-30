using System;

namespace NextBusDesktop.Utilities
{
    public interface ILog
    {
        void Log(object value);
        void Log(object value, string category);
    }
}
