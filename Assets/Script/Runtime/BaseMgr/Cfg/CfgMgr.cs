using Jam.Core;
using Jam.Runtime;
using Jam.Runtime.Event;

namespace Jam.Cfg
{

    public partial class Tables : IMgr
    {
        public Tables()
        {
        }

        public void Shutdown(bool isAppQuit)
        {
        }

        public void LoadCfg()
        {
            LoadCfgImpl(OnLoadCfgDone);
        }

        private void OnLoadCfgDone(bool success)
        {
            if (success)
            {
                G.Event.Send(GlobalEventId.LoadCfgSuccess);
            }
            else
            {
                G.Event.Send(GlobalEventId.LoadCfgFailure);
            }
        }
    }

}