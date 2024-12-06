using System.Collections.Generic;

namespace Quantum.Helper
{

    public static unsafe partial class Helper_Buff
    {
        static Helper_Buff()
        {
            _buffCmds = new Dictionary<int, BuffCmd>() { };
        }
    }

}