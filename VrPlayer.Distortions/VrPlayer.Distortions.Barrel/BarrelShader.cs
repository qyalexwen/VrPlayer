using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

using VrPlayer.Contracts.Distortions;

namespace VrPlayer.Distortions.Barrel
{
    public class BarrelShader : DistortionShader
    {
        public static readonly DependencyProperty InputProperty =
            RegisterPixelShaderSamplerProperty("Input", typeof(BarrelShader), 0);
        public Brush Input
        {
            get { return ((Brush)(GetValue(InputProperty))); }
            set { SetValue(InputProperty, value); }
        }

        public static readonly DependencyProperty EyeModeLeftProperty =
            DependencyProperty.Register("EyeModeLeft", typeof(double), typeof(BarrelShader),
            new UIPropertyMetadata((double)DistortionBase.EyeModes.LEFT, PixelShaderConstantCallback(0)));
        public double EyeModeLeft
        {
            get { return ((double)(GetValue(EyeModeLeftProperty))); }
            set { SetValue(EyeModeLeftProperty, value); }
        }

        public static readonly DependencyProperty EyeModeRightProperty =
            DependencyProperty.Register("EyeModeRight", typeof(double), typeof(BarrelShader),
            new UIPropertyMetadata((double)DistortionBase.EyeModes.RIGHT, PixelShaderConstantCallback(0)));
        public double EyeModeRight
        {
            get { return ((double)(GetValue(EyeModeRightProperty))); }
            set { SetValue(EyeModeRightProperty, value); }
        }

        public static readonly DependencyProperty EyeModeMonoProperty =
            DependencyProperty.Register("EyeModeMono", typeof(double), typeof(BarrelShader),
            new UIPropertyMetadata((double)DistortionBase.EyeModes.NONE, PixelShaderConstantCallback(0)));
        public double EyeModeMono
        {
            get { return ((double)(GetValue(EyeModeMonoProperty))); }
            set { SetValue(EyeModeMonoProperty, value); }
        }

        public BarrelShader(DistortionBase.EyeModes e)
        {
            var pixelShader = new PixelShader();
            pixelShader.UriSource = new Uri(string.Format(
                "pack://application:,,,/{0};component/{1}",
                "VrPlayer.Distortions.Barrel",
                "BarrelEffect.ps"));
            PixelShader = pixelShader;

            UpdateShaderValue(InputProperty);

            switch(e) {
                case(DistortionBase.EyeModes.LEFT):
                    UpdateShaderValue(EyeModeLeftProperty);
                    break;
                case (DistortionBase.EyeModes.RIGHT):
                    UpdateShaderValue(EyeModeRightProperty);
                    break;
                default:
                    UpdateShaderValue(EyeModeMonoProperty);
                    break;
            }
        }
    }
}
