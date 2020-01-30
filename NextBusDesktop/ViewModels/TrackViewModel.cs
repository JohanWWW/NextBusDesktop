using System;
using NextBusDesktop.Models;

namespace NextBusDesktop.ViewModels
{
    public class TrackViewModel : ViewModelBase<Track>
    {
        public string TrackNumber
        {
            get => Model.Id;
            set => SetProperty(Model.Id, value, () => Model.Id = value);
        }

        private string _type;
        public string Type
        {
            get => _type;
            set => SetProperty(ref _type, value);
        }

        /// <param name="id">Id of the track</param>
        public TrackViewModel(string id, string type = null) : this(new Track { Id = id }, type)
        {
        }

        public TrackViewModel(Track track = null, string type = null) : base(track)
        {
            _type = type;
        }
    }
}
