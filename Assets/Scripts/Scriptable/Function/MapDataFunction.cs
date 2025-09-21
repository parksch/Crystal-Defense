using System.Collections.Generic;
using UnityEngine;

namespace JsonClass
{
    public partial class MapDataScriptable // This Class is a functional Class.
    {
        Dictionary<string, MapData> dataDict;

        public MapData GetData(string key)
        {
            if(dataDict == null)
            {
                dataDict = new Dictionary<string, MapData>(); 

                foreach (var item in mapData)
                {
                    dataDict[item.index] = item;
                }

            }

            return dataDict[key];
        }
    }

    public partial class MapData // This Class is a functional Class.
    {
    }

}
