namespace Quantum.Helper
{

    public static unsafe partial class Helper_TLNode
    {
        private class TLNodeCmd_AddBuffToCaster : ITLNodeCmd
        {
            public void Execute(Frame f, TimelineObj tlObj, TLNode node)
            {
                var addBuffInfo = node.AddBuffToCaster->addBuffInfo;
                addBuffInfo.caster = tlObj.caster;
                addBuffInfo.target = tlObj.caster;
                Helper_Buff.AddBuff(f, tlObj.caster, addBuffInfo);
            }
        }
    }

}