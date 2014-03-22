using System;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Windows.Threading;
using System.Windows.Media.Media3D;
using VrPlayer.Contracts.Trackers;
using VrPlayer.Helpers;
using OpenTK;

namespace VrPlayer.Trackers.OculusRiftTracker
{
    [DataContract]
    unsafe public class OculusRiftTracker : TrackerBase, ITracker
    {
        readonly OculusRift rift = new OculusRift();

        private readonly DispatcherTimer _timer;

        public OculusRiftTracker()
        {
            _timer = new DispatcherTimer(DispatcherPriority.Send);
            _timer.Interval = new TimeSpan(0, 0, 0, 0, 15);
            _timer.Tick += timer_Tick;
        }

        public override void Load()
        {
            try
            {
                if (!IsEnabled)
                {
                    IsEnabled = true;
                }
            }
            catch (Exception exc)
            {
                Logger.Instance.Error(exc.Message, exc);
                IsEnabled = false;
            }
            _timer.Start();
        }

        public override void Unload()
        {
            _timer.Stop();
        }

        void timer_Tick(object sender, EventArgs e)
        {
            try
            {
                OpenTK.Quaternion q = rift.PredictedOrientation;

                RawRotation = new System.Windows.Media.Media3D.Quaternion(q.X, -q.Y, q.Z, -q.W);

                UpdatePositionAndRotation();
            }
            catch (Exception exc)
            {
                Logger.Instance.Error(exc.Message, exc);
            }
        }

        private static void ThrowErrorOnResult(int result, string message)
        {
            if (result == -1)
            {
                throw new Exception(message);
            }
        }
    }
}