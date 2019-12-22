using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextBusDesktop.Models;

namespace NextBusDesktop.ViewModels
{
    public class TrackViewModel : NotificationBase<Track>
    {
        public string TrackNumber
        {
            get => This.Id;
            set => SetProperty(This.Id, value, () => This.Id = value);
        }

        private string _type;
        public string Type
        {
            get => _type;
            set => SetProperty(ref _type, value);
        }

        public TrackViewModel(string id, string type = null) : this(new Track { Id = id }, type)
        {
        }

        public TrackViewModel(Track track = null, string type = null) : base(track)
        {
            _type = type;
        }
    }
}
