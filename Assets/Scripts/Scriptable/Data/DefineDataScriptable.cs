using System.Collections.Generic;
using UnityEngine;

namespace JsonClass
{
    public partial class DefineDataScriptable : ScriptableObject
    {
        public List<DefineData> defineData = new List<DefineData>();
    }

    [System.Serializable]
    public partial class DefineData
    {
        public string index;
        public float value;
    }

}
