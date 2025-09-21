using JsonClass;
using System;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UIElements;

public class CommonPanel : BasePanel
{
    [SerializeField] TMP_Text titleText;
    [SerializeField] TMP_Text detailText;
    [SerializeField] GameObject yesnoObject;
    [SerializeField] GameObject okObject;

    Action yesAction;
    Action noAction;
    Action okAction;

    public void SetTitle(string titleKey)
    {
        titleText.text = DataManager.Instance.GetText(titleKey);
    }

    public void SetDetail(string detail, bool isStringKey = true)
    {
        if (isStringKey)
        {
            detailText.text = DataManager.Instance.GetText(detail);
        } 
        else
        {
            detailText.text = detail;
        }
    }

    public void SetOK(Action okClick = null)
    {
        okAction = okClick;
        yesnoObject.SetActive(false);
        okObject.SetActive(true);
    }

    public void SetYesNo(Action yesClick = null,Action noClick = null)
    {
        yesAction = yesClick;
        noAction = noClick;
        yesnoObject.SetActive(true);
        okObject.SetActive(false);
    }

    public override void Open()
    {
        base.Open();
    }

    public override void Close() 
    {
        base.Close(); 
    }

    public void OnClickOK()
    {
        UIManager.Instance.PopPanel();
        okAction?.Invoke();
        okAction = null;
    }

    public void OnClickYes()
    {
        UIManager.Instance.PopPanel();
        yesAction?.Invoke();
        yesAction = null;
    }

    public void OnClickNo()
    {
        UIManager.Instance.PopPanel();
        noAction?.Invoke();
        noAction = null;
    }

    public override void CallEscafe()
    {
        if (yesnoObject.activeSelf)
        {
            OnClickNo();
            return;
        }

        if(okObject.activeSelf)
        {
            OnClickOK();
            return;
        }
    }
}
