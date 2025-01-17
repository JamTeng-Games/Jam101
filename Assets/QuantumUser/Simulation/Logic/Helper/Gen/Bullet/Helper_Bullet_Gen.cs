using System.Collections.Generic;
using Quantum.Graph.Skill;

namespace Quantum.Helper
{

    public static unsafe partial class Helper_Bullet
    {
        static Helper_Bullet()
        {
            _bulletCmds = new Dictionary<int, BulletCmd>() 
            {
                { (int)BulletType.Arrow, new BulletCmd_Arrow() },
                { (int)BulletType.PistolBullet, new BulletCmd_PistolBullet() },

            };
        }
    }

}