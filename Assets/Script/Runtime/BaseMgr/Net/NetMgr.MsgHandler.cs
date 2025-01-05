using System.Collections.Generic;
using Jam.Core;

namespace Jam.Runtime.Net_
{

    public partial class NetMgr
    {
        private List<IMsgHandler> _msgHandlers;

        private void InitMsgHandlers()
        {
            _msgHandlers = new List<IMsgHandler>(64);
            foreach (var type in Utils.Assembly.GetTypes())
            {
                if (type.IsClass && type.GetInterface("IMsgHandler") != null)
                {
                    IMsgHandler handler = (IMsgHandler)System.Activator.CreateInstance(type);
                    _msgHandlers.Add(handler);
                }
            }
        }

        private void RegisterAllMsgHandlers()
        {
            foreach (var handler in _msgHandlers)
            {
                AddMessageHook(handler.Cmd, handler.HandleMsg);
            }
        }

        private void UnregisterAllMsgHandlers()
        {
            foreach (var handler in _msgHandlers)
            {
                RemoveMessageHook(handler.Cmd, handler.HandleMsg);
            }
        }
    }

}