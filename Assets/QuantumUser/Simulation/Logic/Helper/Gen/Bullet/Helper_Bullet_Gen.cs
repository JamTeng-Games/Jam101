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
                { (int)BulletType.test_bullet_1, new BulletCmd_test_bullet_1() },

            };
        }
    }

}