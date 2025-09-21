using System.Collections.Generic;
using UnityEngine;

namespace JsonClass
{
    public partial class MonsterDataScriptable : ScriptableObject
    {
        public List<MonsterData> monsterData = new List<MonsterData>();
    }

    [System.Serializable]
    public partial class MonsterData
    {
        public string index;
        public int hp;
        public float speed;
        public List<int> effect;
        public int addCoin;
        public int subtractionLife;
    }

}
