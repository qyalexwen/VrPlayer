using System;
using System.ComponentModel.Composition;
using VrPlayer.Contracts;
using VrPlayer.Contracts.Projections;
using VrPlayer.Helpers;

namespace VrPlayer.Projections.Hemisphere
{
    [Export(typeof(IPlugin<IProjection>))]
    public class HemispherePlugin : PluginBase<IProjection>
    {
        public HemispherePlugin()
        {
            try
            {
                Name = "Hemisphere";
                var projection = new HemisphereProjection();
                Content = projection;
                Panel = new HemispherePanel(projection);
                InjectConfig(PluginConfig.FromSettings(ConfigHelper.LoadConfig().AppSettings.Settings));
            }
            catch (Exception exc)
            {
                Logger.Instance.Error(string.Format("Error while loading '{0}'", GetType().FullName), exc);
            }
        }
    }
}