using System.Collections.Generic;
using UnityEngine;

namespace JsonClass
{
    public partial class SkillDataScriptable : ScriptableObject
    {
        public List<SkillData> skillData = new List<SkillData>();
    }

    [System.Serializable]
    public partial class SkillData
    {
        public string index;
        public int targetState;
        public float value;
        public int startType;
        public bool targetFallow;
        public int destoryType;
        public float optionalValue;
        public float optionalValue2;
    }

}
