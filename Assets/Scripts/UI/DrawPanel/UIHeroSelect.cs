using ClientEnum;
using JsonClass;
using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class UIHeroSelect : MonoBehaviour
{
    [SerializeField] GameObject summonText;
    [SerializeField] GameStage stage;
    [SerializeField] List<GameObject> disableObject;
    [SerializeField] TMP_Text title;
    [SerializeField] UIValueText valueText;
    [SerializeField] RectTransform heroPoint;
    [SerializeField] RectTransform content;
    [SerializeField] GameObject targetHero;
    [SerializeField] string traitTextKey;
    [SerializeField] string SkillTextkey;

    HeroData target;

    public void Set(HeroData hero)
    {
        target = hero;
        title.text = DataManager.Instance.GetText(target.title);

        if (targetHero != null)
        {
            PoolManager.Instance.Enqueue(targetHero.name, targetHero);
        }

        GameObject heroObject = PoolManager.Instance.Dequeue(target.uiPrefab);
        heroObject.transform.SetParent(heroPoint);
        heroObject.transform.localRotation = Quaternion.identity;
        heroObject.transform.localPosition = Vector3.zero;
        heroObject.transform.localScale = Vector3.one;
        heroObject.SetActive(true);

        string traits = "";

        for (int i = 0; i < target.trait.Count; i++)
        {
            traits += DataManager.Instance.GetText(target.trait[i]);

            if (i != target.trait.Count - 1 )
            {
                traits += ",";
            }
        }

        valueText.ResetText();
        valueText.SetText(DataManager.Instance.GetText(traitTextKey), traits);
        valueText.SetText(DataManager.Instance.GetText(SkillTextkey), DataManager.Instance.GetText(target.skill));
        valueText.SetText(DataManager.Instance.GetText(State.Attack.ToString()), target.attack.ToString());
        valueText.SetText(DataManager.Instance.GetText(State.AttackSpeed.ToString()), target.attackSpeed.ToString());
        valueText.SetText(DataManager.Instance.GetText(State.AttackRange.ToString()), target.attackRange.ToString());

        content.anchoredPosition = Vector3.zero;
    }

    public void OnClickSelect()
    {
        stage.OpenSpots(target);
        summonText.gameObject.SetActive(true);

        for (int i = 0; i < disableObject.Count; i++)
        {
            disableObject[i].SetActive(false);
        }
    }
}
