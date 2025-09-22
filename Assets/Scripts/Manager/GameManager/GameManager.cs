using JsonClass;
using System;
using UnityEngine;

public partial class GameManager : Singleton<GameManager>
{
    [SerializeField] GameStage currentStage;
    [SerializeField] string titleKey;
    [SerializeField] string descKey;

    public delegate void EnemyDeathEvent();

    EnemyDeathEvent enemyDeath;

    public void AddEnemyDeathEvent(EnemyDeathEvent action) => enemyDeath += action; 

    public bool CheckDrawCoin => currentStage.CheckDrawCoin;

    protected override void Awake()
    {

    }

    private void Start()
    {
        Init();
    }

    public void Init()
    {
        UIManager.Instance.Init();
    }

    public void SetGameStage(StageData stage)
    {
        currentStage.ResetData();
        currentStage.Set(stage);

        UIManager.Instance.PopPanel();
        UIManager.Instance.AddPanel(UIManager.Instance.GetPanel<GamePanel>());
    }

    public void ResetStage()
    {
        currentStage.ResetData();
    }

    public void WaveStart()
    {
        currentStage.WaveStart();
    }

    public void WaveCheck()
    {
        if (!currentStage.IsAlive)
        {
            currentStage.AllMonsterEnqueue();
            CommonPanel commonPanel = UIManager.Instance.GetPanel<CommonPanel>();
            commonPanel.SetTitle(titleKey);
            commonPanel.SetDetail(descKey);
            commonPanel.SetOK(() => 
            {
                ResetStage();
                UIManager.Instance.PopPanel();
                UIManager.Instance.AddPanel(UIManager.Instance.GetPanel<MainPanel>());
            });
            UIManager.Instance.AddPanel(commonPanel);
        }
        else
        {
            if (currentStage.IsStageEnd)
            {
                GamePanel gamePanel = UIManager.Instance.GetPanel<GamePanel>();
                gamePanel.ActiveSelect();
            }
        }
    }

    public void WaveEnd()
    {

    }

    public GameObject ResetTrans(GameObject target,Transform parent)
    {
        target.transform.parent = parent;
        target.transform.localPosition = Vector3.zero;
        target.transform.localRotation = Quaternion.identity;
        target.transform.localScale = Vector3.one;
        target.SetActive(true);

        return target;
    }

    public void OnEnemyDeath()
    {
        enemyDeath?.Invoke();
        enemyDeath = null;
    }

    public void SubtractionLife(int value)
    {
        currentStage.SetValue(ClientEnum.UIType.Life, value);
    }

    public void AddCoin(int value)
    {
        currentStage.SetValue(ClientEnum.UIType.Coin, value);
    }

    public void AddMonster(Monster monster)
    {
        currentStage.AddMonster(monster);
    }

    public void RemoveMonster(Monster monster)
    {
        currentStage.RemoveMonster(monster);
    }
}
