using System.Collections.Generic;
using UnityEngine;

namespace JsonClass
{
    public partial class MapDataScriptable : ScriptableObject
    {
        public List<MapData> mapData = new List<MapData>();
    }

    [System.Serializable]
    public partial class MapData
    {
        public string index;
        public string prefab;
    }

}
