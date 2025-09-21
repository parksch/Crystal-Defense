using UnityEngine;
using JsonClass;

public class DataManager : Singleton<DataManager>
{
    [SerializeField] Language language;

    protected override void Awake()
    {
        base.Awake();
        Application.targetFrameRate = 60;
    }

    public string GetText(string key)
    {
        LocalizationDataScriptable localization = ScriptableManager.Instance.Get<LocalizationDataScriptable>(ScriptableType.LocalizationData);
        return localization.GetText(language, key);
    }
}
