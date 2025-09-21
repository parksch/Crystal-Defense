using System.Collections.Generic;
using UnityEngine;

namespace JsonClass
{
    public partial class LocalizationDataScriptable : ScriptableObject
    {
        public List<LocalizationData> localizationData = new List<LocalizationData>();
    }

    [System.Serializable]
    public partial class LocalizationData
    {
        public string index;
        public string kr;
        public string en;
    }

}
