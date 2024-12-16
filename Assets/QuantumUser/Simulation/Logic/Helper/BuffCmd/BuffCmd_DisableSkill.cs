using Quantum.Graph.Skill;

namespace Quantum.Helper
{

    public unsafe class BuffCmd_DisableSkill : BuffCmd
    {
        public override void OnAdd(Frame f, EntityRef entity, ref BuffObj buffObj, int modifyStack)
        {
            Helper_Stats.AddRC_DisableSkill(f, buffObj.owner);
        }

        public override void OnRemove(Frame f, EntityRef entity, ref BuffObj buffObj)
        {
            Helper_Stats.ReduceRC_DisableSkill(f, buffObj.owner);
        }
    }

}