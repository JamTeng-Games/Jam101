using System.Collections.Generic;

namespace Quantum.Helper
{

    public interface ITLNodeCmd
    {
        void Execute(Frame f, TimelineObj tlObj, TLNode node);
    }

    public static unsafe partial class Helper_TLNode
    {
        private static Dictionary<ETLNodeType, ITLNodeCmd> _nodeCmd;

        static Helper_TLNode()
        {
            _nodeCmd = new Dictionary<ETLNodeType, ITLNodeCmd>()
            {
                { ETLNodeType.Log, new TLNodeCmd_Log() },
                { ETLNodeType.AddBuffToCaster, new TLNodeCmd_AddBuffToCaster() },
                { ETLNodeType.PlayAnim, new TLNodeCmd_PlayAnim() },
            };
        }

        public static void Execute(Frame f, TimelineObj tlObj, TimelineNode tlNode)
        {
            if (_nodeCmd.TryGetValue(tlNode.nodeType, out var cmd))
            {
                cmd.Execute(f, tlObj, tlNode.node);
            }
            else
            {
                Log.Error($"Unknown TLNode type: {tlNode.nodeType}");
            }
        }
    }

}