using System.Collections.Generic;
using UnityEngine;

namespace JsonClass
{
    public partial class AttackDataScriptable // This Class is a functional Class.
    {
        Dictionary<string, AttackData> dataDict;

        public AttackData GetData(string key)
        {
            if(dataDict == null)
            {
                dataDict = new Dictionary<string, AttackData>(); 

                foreach (var item in attackData)
                {
                    dataDict[item.index] = item;
                }

            }

            return dataDict[key];
        }
    }

    public partial class AttackData // This Class is a functional Class.
    {
    }

}
