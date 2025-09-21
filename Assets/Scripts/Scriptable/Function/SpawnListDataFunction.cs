using System.Collections.Generic;
using UnityEngine;

namespace JsonClass
{
    public partial class SpawnListDataScriptable // This Class is a functional Class.
    {
        Dictionary<string, SpawnListData> dataDict;

        public SpawnListData GetData(string key)
        {
            if(dataDict == null)
            {
                dataDict = new Dictionary<string, SpawnListData>(); 

                foreach (var item in spawnListData)
                {
                    dataDict[item.index] = item;
                }

            }

            return dataDict[key];
        }
    }

    public partial class SpawnListData // This Class is a functional Class.
    {
    }

}
