using System.Collections.Generic;
using UnityEngine;

namespace JsonClass
{
    public partial class StageDataScriptable // This Class is a functional Class.
    {
        Dictionary<string, StageData> dataDict;

        public StageData GetData(string key)
        {
            if(dataDict == null)
            {
                dataDict = new Dictionary<string, StageData>(); 

                foreach (var item in stageData)
                {
                    dataDict[item.index] = item;
                }

            }

            return dataDict[key];
        }
    }

    public partial class StageData // This Class is a functional Class.
    {
    }

}
