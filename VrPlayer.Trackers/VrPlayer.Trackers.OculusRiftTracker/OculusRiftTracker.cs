using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Windows.Threading;
using System.Windows.Media.Media3D;
using VrPlayer.Contracts.Trackers;
using VrPlayer.Helpers;
using OpenTK;
using System.Windows.Media;

namespace VrPlayer.Trackers.OculusRiftTracker
{
    [DataContract]
    unsafe public class OculusRiftTracker : TrackerBase, ITracker
    {
        private readonly OculusRift rift = new OculusRift();
        private TimeSpan lastRenderTime = new TimeSpan(0);

        public OculusRiftTracker()
        {
            CompositionTarget.Rendering += UpdateRotation;
        }

        protected void UpdateRotation(object sender, EventArgs e)
        {
            // Event may fire multiple times per render. Don't do unnecessary updates.
            TimeSpan nextRenderTime = ((RenderingEventArgs)e).RenderingTime;
            if (nextRenderTime != lastRenderTime)
            {
                lastRenderTime = nextRenderTime;

                OpenTK.Quaternion q = rift.PredictedOrientation;
                RawRotation = new System.Windows.Media.Media3D.Quaternion(q.X, -q.Y, q.Z, -q.W);
                UpdatePositionAndRotation();
            }
        }

        public override void Load()
        {
            if (!IsEnabled)
            {
                IsEnabled = true;
            }
        }

        public override void Unload()
        {
        }
    }
}