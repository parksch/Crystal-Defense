using ClientEnum;
using TMPro;
using UnityEngine;
using System.Collections.Generic;
using JsonClass;

public class ChoicePopup : MonoBehaviour
{
    [SerializeField] TMP_Text titleText;
    [SerializeField] List<UIHeroSelect> selects;
    [SerializeField] string choiceTitleKey;

    public void Set(DrawHierarchy hierarchy)
    {
        titleText.text = DataManager.Instance.GetText(choiceTitleKey);
        HeroData test = ScriptableManager.Instance.Get<HeroDataScriptable>(ScriptableType.HeroData).GetData("Knight");

        for (int i = 0; i < selects.Count; i++)
        {
            selects[i].Set(test);
        }

        gameObject.SetActive(true);
    }
}
