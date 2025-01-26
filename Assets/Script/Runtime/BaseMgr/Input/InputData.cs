using Jam.Runtime.Data_;
using UnityEngine;

namespace Jam.Runtime.Input_
{

    public class InputData : IData
    {
        // Input data
        public Vector2 moveDir;
        public Vector2 aimVector;
        public bool attackPrepare;
        public bool doAttack;
        public bool skillPrepare;
        public bool doSkill;
        public bool superSkillPrepare;
        public bool doSuperSkill;
        public bool doCancel;

        public void Reset()
        {
            // moveDir = Vector2.zero;
            // aimVector = Vector2.zero;
            // attackPrepare = false;
            // skillPrepare = false;
            // superSkillPrepare = false;

            doAttack = false;
            doSkill = false;
            doSuperSkill = false;
            doCancel = false;
        }

        protected override void OnDispose()
        {
        }
    }

}