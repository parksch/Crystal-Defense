using System.Collections.Generic;
using UnityEngine;

namespace JsonClass
{
    public partial class MonsterDataScriptable // This Class is a functional Class.
    {
        Dictionary<string, MonsterData> dataDict;

        public MonsterData GetData(string key)
        {
            if(dataDict == null)
            {
                dataDict = new Dictionary<string, MonsterData>(); 

                foreach (var item in monsterData)
                {
                    dataDict[item.index] = item;
                }

            }

            return dataDict[key];
        }
    }

    public partial class MonsterData // This Class is a functional Class.
    {
    }

}
