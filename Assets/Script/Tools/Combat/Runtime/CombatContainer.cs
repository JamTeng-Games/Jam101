using System;
using System.Collections.Generic;
using UnityEngine;

namespace Jam.Runtime.Combat_
{

    [Serializable]
    public class CombatContainer : ScriptableObject
    {
        public List<CombatNodeLinkData> NodeLinks = new List<CombatNodeLinkData>();
        public List<CombatNodeData> NodeData = new List<CombatNodeData>();
        public List<ExposedProperty> ExposedProperties = new List<ExposedProperty>();
    }

}