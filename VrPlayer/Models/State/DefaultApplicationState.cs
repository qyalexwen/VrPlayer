﻿using System;
using System.IO;
using System.Windows.Controls;
using System.Windows.Media.Effects;

using WPFMediaKit.DirectShow.Controls;

using VrPlayer.Helpers;
using VrPlayer.Helpers.Mvvm;
using VrPlayer.Models.Config;
using VrPlayer.Models.Plugins;
using VrPlayer.Models.Trackers;
using VrPlayer.Models.Wrappers;
using VrPlayer.Models.Shaders;

namespace VrPlayer.Models.State
{
    public class DefaultApplicationState : ViewModelBase, IApplicationState
    {
        #region Fields

        private MediaUriElement _media;
        public MediaUriElement Media
        {
            get
            {
                return _media;
            }
        }

        private StereoMode _stereoMode;
        public StereoMode StereoMode
        {
            get
            {
                return _stereoMode;
            }
            set
            {
                _stereoMode = value;
                OnPropertyChanged("StereoMode");
            }
        }

        private EffectPlugin _effectPlugin;
        public EffectPlugin EffectPlugin
        {
            get
            {
                return _effectPlugin;
            }
            set
            {
                _effectPlugin = value;
                OnPropertyChanged("EffectPlugin");
            }
        }

        private WrapperPlugin _wrapperPlugin;
        public WrapperPlugin WrapperPlugin
        {
            get
            {
                return _wrapperPlugin;
            }
            set
            {
                _wrapperPlugin = value;
                OnPropertyChanged("WrapperPlugin");
            }
        }

        private TrackerPlugin _trackerPlugin;
        public TrackerPlugin TrackerPlugin
        {
            get
            {
                return _trackerPlugin;
            }
            set
            {
                _trackerPlugin = value;
                OnPropertyChanged("TrackerPlugin");
            }
        }

        private ShaderPlugin _shaderPlugin;
        public ShaderPlugin ShaderPlugin
        {
            get
            {
                return _shaderPlugin;
            }
            set
            {
                _shaderPlugin = value;
                OnPropertyChanged("ShaderPlugin");
            }
        }

        #endregion

        public DefaultApplicationState(IApplicationConfig config)
        {
            _media = new MediaUriElement();
            _media.BeginInit();
            _media.EndInit();
        }
    }
}
