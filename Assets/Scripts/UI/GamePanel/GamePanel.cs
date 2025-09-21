using TMPro;
using UnityEngine;

public class GamePanel : BasePanel
{
    [SerializeField] UIButton drawButton;
    [SerializeField] TMP_Text waveText;
    [SerializeField] TMP_Text lifeText;
    [SerializeField] TMP_Text coinText;
    [SerializeField] GameObject selectObject;
    [SerializeField] string commonPanelTitle ;
    [SerializeField] string commonPanelDetail ;

    public void ActiveSelect() 
    {
        drawButton.SetInteractive(GameManager.Instance.CheckDrawCoin);
        selectObject.SetActive(true); 
    }

    public override void Open()
    {
        selectObject.SetActive(true);
        drawButton.SetInteractive(GameManager.Instance.CheckDrawCoin);
        base.Open();
    }

    public override void Close()
    {
        base.Close();
    }

    public override void CallEscafe()
    {
        CreateCommonPanel();
    }

    public void OnClickStart()
    {
        selectObject.SetActive(false);
        GameManager.Instance.WaveStart();
    }

    void CreateCommonPanel()
    {
        CommonPanel commonPanel = UIManager.Instance.GetPanel<CommonPanel>();
        commonPanel.SetTitle(commonPanelTitle);
        commonPanel.SetDetail(commonPanelDetail);
        commonPanel.SetYesNo(() =>
        {
            GameManager.Instance.ResetStage();
            UIManager.Instance.PopPanel();
            UIManager.Instance.AddPanel(UIManager.Instance.GetPanel<MainPanel>());
        });
        UIManager.Instance.AddPanel(commonPanel);
    }

}
