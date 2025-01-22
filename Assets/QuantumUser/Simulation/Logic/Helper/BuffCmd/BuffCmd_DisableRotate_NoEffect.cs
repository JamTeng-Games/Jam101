using Quantum.Graph.Skill;

namespace Quantum.Helper
{

    public unsafe class BuffCmd_DisableRotate_NoEffect : BuffCmd
    {
        public override void OnAdd(Frame f, EntityRef entity, ref BuffObj buffObj, int modifyStack)
        {
            Helper_Stats.AddRC_DisableRotate(f, buffObj.owner);
        }

        public override void OnRemove(Frame f, EntityRef entity, ref BuffObj buffObj)
        {
            Helper_Stats.ReduceRC_DisableRotate(f, buffObj.owner);
        }
    }

}