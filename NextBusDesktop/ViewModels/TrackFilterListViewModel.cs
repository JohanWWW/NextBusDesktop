﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NextBusDesktop.Models;

namespace NextBusDesktop.ViewModels
{
    public class TrackFilterListViewModel : NotificationBase
    {
        private ObservableCollection<TrackViewModel> _tracks;
        public ObservableCollection<TrackViewModel> Tracks
        {
            get => _tracks;
            set => SetProperty(ref _tracks, value);
        }

        private int _selectedIndex;
        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                SetProperty(ref _selectedIndex, value);

                if (value is -1) SelectedTrack = null;
                else SelectedTrack = _tracks.ElementAt(value);

                System.Diagnostics.Debug.WriteLine($"Selected index: {_selectedIndex}");
            }
        }

        private TrackViewModel _selectedTrack;
        public TrackViewModel SelectedTrack
        {
            get => _selectedTrack;
            set
            {
                SetProperty(ref _selectedTrack, value);

                System.Diagnostics.Debug.WriteLine($"Selected track: {_selectedTrack?.TrackNumber}");
            }
        }

        public TrackFilterListViewModel()
        {
            _selectedIndex = -1;
            _tracks = new ObservableCollection<TrackViewModel>();
        }

        public void Add(TrackViewModel track)
        {
            if (!Tracks.Contains(track))
                Tracks.Add(track);
        }

        public void Remove(TrackViewModel track)
        {
            if (Tracks.Contains(track))
                Tracks.Remove(track);
        }

        public void Clear() => Tracks.Clear();
    }
}
