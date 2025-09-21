using System.Collections.Generic;
using UnityEngine;

namespace JsonClass
{
    public enum Language
    {
        KR,
        EN
    }

    public partial class LocalizationDataScriptable // This Class is a functional Class.
    {
        Dictionary<string, LocalizationData> dataDict;

        public LocalizationData GetData(string key)
        {
            if(dataDict == null)
            {
                dataDict = new Dictionary<string, LocalizationData>(); 

                foreach (var item in localizationData)
                {
                    dataDict[item.index] = item;
                }

            }

            return dataDict[key];
        }

        public string GetText(Language language, string key)
        {
            LocalizationData data = GetData(key);
            return data.GetText(language);
        }
    }

    public partial class LocalizationData // This Class is a functional Class.
    {
        public string GetText(Language language)
        {
            switch (language)
            {
                case Language.KR:
                    return kr;
                case Language.EN:
                    return en;
                default:
                    break;
            }

            return "NoText";
        }
    }

}
