using System.Collections.Generic;
using Quantum.Graph.Skill;

namespace Quantum.Helper
{

    public static unsafe partial class Helper_Buff
    {
        static Helper_Buff()
        {
            _buffCmds = new Dictionary<int, BuffCmd>() 
            {
                { (int)BuffType.AutoReload, new BuffCmd_AutoReload() },
                { (int)BuffType.Dash, new BuffCmd_Dash() },
                { (int)BuffType.DisableMove, new BuffCmd_DisableMove() },
                { (int)BuffType.DisableMove_NoEffect, new BuffCmd_DisableMove_NoEffect() },
                { (int)BuffType.DisableRotate_NoEffect, new BuffCmd_DisableRotate_NoEffect() },
                { (int)BuffType.DisableSkill, new BuffCmd_DisableSkill() },
                { (int)BuffType.EnableRotate, new BuffCmd_EnableRotate() },
                { (int)BuffType.Hot, new BuffCmd_Hot() },
                { (int)BuffType.PeterAttrib, new BuffCmd_PeterAttrib() },
                { (int)BuffType.Poison, new BuffCmd_Poison() },
                { (int)BuffType.PureAttrib, new BuffCmd_PureAttrib() },
                { (int)BuffType.test_item_1, new BuffCmd_test_item_1() },
                { (int)BuffType.test_item_2, new BuffCmd_test_item_2() },
                { (int)BuffType.test_item_3, new BuffCmd_test_item_3() },
                { (int)BuffType.Test1, new BuffCmd_Test1() },

            };
        }
    }

}