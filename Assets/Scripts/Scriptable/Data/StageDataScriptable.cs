using System.Collections.Generic;
using UnityEngine;

namespace JsonClass
{
    public partial class StageDataScriptable : ScriptableObject
    {
        public List<StageData> stageData = new List<StageData>();
    }

    [System.Serializable]
    public partial class StageData
    {
        public string index;
        public string title;
        public string map;
        public float spawnTime;
        public List<string> monsters;
        public string bossMonster;
        public float addValue;
        public float bossAdd;
        public int life;
        public int wave;
        public int startDraw;
        public List<string> spawn;
    }

}
