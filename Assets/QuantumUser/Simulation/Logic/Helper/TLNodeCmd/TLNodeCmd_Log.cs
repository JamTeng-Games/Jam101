namespace Quantum.Helper
{

    public static unsafe partial class Helper_TLNode
    {
        private class TLNodeCmd_Log : ITLNodeCmd
        {
            public void Execute(Frame f, TimelineObj tlObj, TLNode node)
            {
                Log.Debug($"Execute {node.Log->content}");
            }
        }
    }

}