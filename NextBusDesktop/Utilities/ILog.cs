using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextBusDesktop.Utilities
{
    public interface ILog
    {
        void Log(object value);
        void Log(object value, string category);
    }
}
