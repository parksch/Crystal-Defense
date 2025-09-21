using JsonClass;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIStage : MonoBehaviour
{
    [SerializeField] StageData current;
    [SerializeField] TMP_Text title;
    [SerializeField] GameObject leftArrow;
    [SerializeField] GameObject rightArrow;

    int index;
    List<StageData> stages;

    public void Init()
    {
        index = 0;
        stages = ScriptableManager.Instance.Get<StageDataScriptable>(ScriptableType.StageData).stageData;
        current = stages[index];
        Set();
    }

    public void Set()
    {
        title.text = DataManager.Instance.GetText(current.title);
        ArrowCheck();
    }

    public void OnClickNext()
    {
        index = (index + 1 < stages.Count ? index + 1 : index);
        Set();
    }

    public void OnClickPrev()
    {
        index = (index - 1 > 0 ? index - 1 : 0);
        Set();
    }

    public void OnClickStart()
    {
        GameManager.Instance.SetGameStage(current);
    }

    void ArrowCheck()
    {
        leftArrow.SetActive(index > 0);
        rightArrow.SetActive(index + 1 < stages.Count);
    }
}
