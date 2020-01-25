using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NextBusDesktop.Utilities
{
    public class OutputLogger : ILog
    {
        private readonly string _ownerName;

        public OutputLogger(Type owner) => _ownerName = owner.Name;

        public OutputLogger(string ownerName) => _ownerName = ownerName;

        public void Log(object value) => System.Diagnostics.Debug.WriteLine($"{_ownerName} -> {value.ToString()}");

        public void Log(object value, string category) => System.Diagnostics.Debug.WriteLine($"{_ownerName} -> [{category}] -> {value.ToString()}");
    }
}
