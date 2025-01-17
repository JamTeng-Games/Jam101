using Quantum.Graph.Skill;

namespace Quantum.Helper
{

    public static unsafe class Helper_Anim
    {
        public static void PlayAnim(Frame f, EntityRef entity, AnimationKey animKey, bool force)
        {
            f.Events.PlayAnim(entity, (int)animKey, force);
        }
    }

}