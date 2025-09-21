using System.Collections.Generic;
using UnityEngine;

namespace JsonClass
{
    public partial class SkillDataScriptable // This Class is a functional Class.
    {
        Dictionary<string, SkillData> dataDict;

        public SkillData GetData(string key)
        {
            if(dataDict == null)
            {
                dataDict = new Dictionary<string, SkillData>(); 

                foreach (var item in skillData)
                {
                    dataDict[item.index] = item;
                }

            }

            return dataDict[key];
        }
    }

    public partial class SkillData // This Class is a functional Class.
    {
    }

}
