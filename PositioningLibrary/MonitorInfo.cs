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

        public PositionModel Model { get; private set; }

        public void AddNewModel(PositionModel model)
        {
            if (model != default)
            {
                Model = model;
            }
        }

        public void Position()
        {
            Positioning.PositionModel(Model, MonitorScreen);
        }
    }
}