using UnityEngine;
using System.Collections.Generic;

public partial class UIManager : Singleton<UIManager>
{
    [SerializeField] BasePanel currentPanel;
    [SerializeField] List<BasePanel> panels;
    
    Stack<BasePanel> openPanels = new Stack<BasePanel>();

    protected override void Awake()
    {

    }

    public void Init()
    {
        PlayerController.Instance.AddEscafeAction(EscafeEvent);
        currentPanel = null;

        foreach (BasePanel panel in panels)
        {
            panel.FirstLoad();
        }

        AddPanel(GetPanel<MainPanel>());
    }

    public T GetPanel<T>() where T : BasePanel { return panels.Find(x => x is T) as T; }

    public void AddPanel(BasePanel target)
    {
        if (currentPanel != null)
        {
            openPanels.Push(currentPanel);
            if (!target.IsTranslucent)
            {
                ClosePanel(currentPanel);
            }
        }

        OpenPanel(target);
    }

    public void PopPanel()
    {
        if (currentPanel != null)
        {
            ClosePanel(currentPanel);

            if (openPanels.Count > 0)
            {
                BasePanel panel = openPanels.Pop();
                OpenPanel(panel);
            }
        }
    }

    void OpenPanel(BasePanel target)
    {
        target.Open();
        target.gameObject.SetActive(true);
        currentPanel = target;
    }

    void ClosePanel(BasePanel target)
    {
        target.Close();
        target.gameObject.SetActive(false);
        currentPanel = null;
    }

    void EscafeEvent()
    {
        if (!currentPanel.IsEscafePossible)
        {
            return;
        }

        currentPanel.CallEscafe();
    }
}
