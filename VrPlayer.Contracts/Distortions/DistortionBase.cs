using System.Windows.Media.Effects;

namespace VrPlayer.Contracts.Distortions
{
    public abstract class DistortionBase: ILoadable
    {
        public enum EyeModes { NONE=0, LEFT=-1, RIGHT=1 };

        public void Load() { }
        public void Unload() { }

        public abstract DistortionShader Left { get; }
        public abstract DistortionShader Right { get; }
    }
}
