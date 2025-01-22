using Quantum.Graph.Skill;

namespace Quantum.Helper
{

    public unsafe class BuffCmd_EnableRotate : BuffCmd
    {
        public override void OnAdd(Frame f, EntityRef entity, ref BuffObj buffObj, int modifyStack)
        {
            Helper_Stats.ReduceRC_DisableRotate(f, buffObj.owner);
        }
    }

}