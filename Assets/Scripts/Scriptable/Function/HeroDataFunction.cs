using System.Collections.Generic;
using UnityEngine;

namespace JsonClass
{
    public partial class HeroDataScriptable // This Class is a functional Class.
    {
        Dictionary<string, HeroData> dataDict;

        public HeroData GetData(string key)
        {
            if(dataDict == null)
            {
                dataDict = new Dictionary<string, HeroData>(); 

                foreach (var item in heroData)
                {
                    dataDict[item.index] = item;
                }

            }

            return dataDict[key];
        }
    }

    public partial class HeroData // This Class is a functional Class.
    {
    }

}
