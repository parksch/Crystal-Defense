using System.Collections.Generic;
using UnityEngine;

namespace JsonClass
{
    public partial class AttackDataScriptable : ScriptableObject
    {
        public List<AttackData> attackData = new List<AttackData>();
    }

    [System.Serializable]
    public partial class AttackData
    {
        public string index;
        public string prefab;
        public int startType;
        public bool targetFallow;
        public int destoryType;
        public float optionalValue;
        public float optionalValue2;
    }

}
