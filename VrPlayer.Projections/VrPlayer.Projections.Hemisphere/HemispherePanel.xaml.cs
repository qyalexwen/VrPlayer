using System;
using System.Windows.Controls;

namespace VrPlayer.Projections.Hemisphere
{
    public partial class HemispherePanel : UserControl
    {
        public HemispherePanel(HemisphereProjection projection)
        {
            InitializeComponent();
            try
            {
                DataContext = projection;
            }
            catch (Exception exc)
            {
            }
        }
    }
}
