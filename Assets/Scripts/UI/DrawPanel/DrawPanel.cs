using ClientEnum;
using UnityEngine;

public class DrawPanel : BasePanel
{
    [SerializeField] GameObject summonText;
    [SerializeField] DrawPopup drawPopup;
    [SerializeField] ChoicePopup choicePopup;

    public override bool IsEscafePossible { get { return false; }}

    DrawHierarchy drawResult;

    public override void FirstLoad()
    {
        base.FirstLoad();
        drawPopup.Init();
    }

    public override void Open()
    {
        base.Open();
        drawPopup.Set();
    }

    public override void Close()
    {
        summonText.SetActive(false);
        base.Close();
    }

    public void OnClickDraw()
    {
        drawResult = drawPopup.OpenCard();
    }

    public void OnClickSelect()
    {
        drawPopup.gameObject.SetActive(false);
        choicePopup.Set(drawResult);
    }
}
