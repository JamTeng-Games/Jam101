using System.Collections.Generic;
using Quantum.Graph.Skill;

namespace Quantum.Helper
{

    public static unsafe partial class Helper_Aoe
    {
        static Helper_Aoe()
        {
            _aoeCmds = new Dictionary<int, AoeCmd>() 
            {
                { (int)AoeType.test_aoe_1, new AoeCmd_test_aoe_1() },
                { (int)AoeType.test_aoe_2, new AoeCmd_test_aoe_2() },

            };
        }
    }

}