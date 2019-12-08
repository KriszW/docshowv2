using SendedModels;
using System.Windows.Forms;

namespace PositioningLib
{
    public class MonitorInfo
    {
        public MonitorInfo(int monitorIndex)
        {
            MonitorIndex = monitorIndex;
            MonitorScreen = Screen.AllScreens[monitorIndex];
        }

        public int MonitorIndex { get; private set; }

        public Screen MonitorScreen { get; private set; }

        public Positioning Positioner { get; set; }

        public void Position(PositionModel model)
        {
            Positioner?.CloseAllAdobe();

            Positioner = new Positioning(model,MonitorScreen);

            Positioner.PositionModel();
        }
    }
}