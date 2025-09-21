using System.Collections.Generic;
using UnityEngine;

namespace JsonClass
{
    public partial class DefineDataScriptable // This Class is a functional Class.
    {
        Dictionary<string, DefineData> dataDict;

        public DefineData GetData(string key)
        {
            if(dataDict == null)
            {
                dataDict = new Dictionary<string, DefineData>(); 

                foreach (var item in defineData)
                {
                    dataDict[item.index] = item;
                }

            }

            return dataDict[key];
        }
    }

    public partial class DefineData // This Class is a functional Class.
    {
    }

}
