namespace Quantum.Helper
{

    public static unsafe partial class Helper_TLNode
    {
        private class TLNodeCmd_PlayAnim : ITLNodeCmd
        {
            public void Execute(Frame f, TimelineObj tlObj, TLNode node)
            {
                f.Events.PlayAnim(tlObj.caster, node.PlayAnim->animKey, node.PlayAnim->force);
            }
        }
    }

}