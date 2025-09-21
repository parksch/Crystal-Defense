using UnityEngine;

public class MainPanel : BasePanel
{
    [SerializeField] UIStage stage;
    [SerializeField] string commonPanelTitle;
    [SerializeField] string commonPanelDetail;

    public override void FirstLoad()
    {
        base.FirstLoad();
        stage.Init();
    }

    public override void Open()
    {
        base.Open();
    }

    public override void Close()
    {
        base.Close();
    }

    public override void CallEscafe()
    {
        CommonPanel commonPanel = UIManager.Instance.GetPanel<CommonPanel>();
        commonPanel.SetTitle(commonPanelTitle);
        commonPanel.SetDetail(commonPanelDetail);
        commonPanel.SetYesNo(()=>
        { 
            Application.Quit(); 
        });
        
        UIManager.Instance.AddPanel(commonPanel);
    }
}
