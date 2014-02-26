using System;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

using VrPlayer.Contracts.Distortions;

namespace VrPlayer.Distortions.Barrel
{
    public class BarrelEffect : DistortionBase
    {
        public override DistortionShader Left
        {
            get { return new BarrelShader(EyeModes.LEFT); ; }
        }

        public override DistortionShader Right
        {
            get { return new BarrelShader(EyeModes.RIGHT); ; }
        }

        public BarrelEffect()
        {
        }
    }
}