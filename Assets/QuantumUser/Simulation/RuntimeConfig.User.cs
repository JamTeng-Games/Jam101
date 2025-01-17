using System.Collections.Generic;
using NewGraph;

namespace Quantum
{

    public partial class RuntimeConfig
    {
        public List<AssetRef<AssetObjectGraphModel>> SkillGraphs;
        public AssetRef<EntityPrototype> BulletPrototype;
        public AssetRef<EntityPrototype> AoePrototype;
    }

}