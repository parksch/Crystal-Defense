using System.Collections.Generic;
using UnityEngine;

namespace JsonClass
{
    public partial class SpawnListDataScriptable : ScriptableObject
    {
        public List<SpawnListData> spawnListData = new List<SpawnListData>();
    }

    [System.Serializable]
    public partial class SpawnListData
    {
        public string index;
        public List<int> spawns;
    }

}
