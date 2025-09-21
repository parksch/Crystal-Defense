using System.Collections.Generic;
using UnityEngine;

namespace JsonClass
{
    public partial class HeroDataScriptable : ScriptableObject
    {
        public List<HeroData> heroData = new List<HeroData>();
    }

    [System.Serializable]
    public partial class HeroData
    {
        public string index;
        public string uiPrefab;
        public string prefab;
        public string title;
        public int attack;
        public float attackSpeed;
        public float attackRange;
        public List<string> trait;
        public string skill;
        public float addValue;
        public string attackPrefab;
        public int skillPoint;
    }

}
