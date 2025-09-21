using ClientEnum;
using JsonClass;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class GameStage : MonoBehaviour
{
    [SerializeField] List<Monster> monsterObjects;
    [SerializeField] Map currentMap;
    [SerializeField] int wave;
    [SerializeField] int life;
    [SerializeField] float spawnTime;
    [SerializeField] float addValue;
    [SerializeField] float bossAdd;
    [SerializeField] string bossMonster;
    [SerializeField] List<string> monsters;
    [SerializeField] string needDrawKey;

    DefineDataScriptable defineScriptable;

    public void AddMonster(Monster monster) => monsterObjects.Add(monster);
    public void RemoveMonster(Monster monster)
    {
        if (monster.IsBoss)
        {
            SetValue(UIType.Life, currentLife);
        }

        monsterObjects.Remove(monster);
    }
    public bool CheckDrawCoin => currentCoin >= defineScriptable.GetData(needDrawKey).value;
    public bool IsStageEnd => currentMap.IsSpawnEnd() && monsterObjects.Count == 0;

    public bool IsAlive => currentLife > 0;

    bool isOn = false;
    float currentTime = 0;
    int currentWave;
    int currentCoin;
    int currentLife;

    private void Start()
    {
        defineScriptable = ScriptableManager.Instance.Get<DefineDataScriptable>(ScriptableType.DefineData);
    }

    public void Set(StageData stage)
    {
        life = stage.life;
        wave = stage.wave;
        spawnTime = stage.spawnTime;
        addValue = stage.addValue;
        bossAdd = stage.bossAdd;
        bossMonster = stage.bossMonster;
        monsters = stage.monsters.ToList();

        MapData mapData = ScriptableManager.Instance.Get<MapDataScriptable>(ScriptableType.MapData).GetData(stage.map);
        currentMap = GameManager.Instance.ResetTrans(PoolManager.Instance.Dequeue(mapData.prefab), transform).GetComponent<Map>();
        currentMap.Set(mapData,stage.spawn);
        currentWave = 0;
        currentLife = life;
        currentCoin = stage.startDraw;
        TextAllUpdate();
    }

    public void WaveStart() 
    {
        isOn = true;
        currentTime = 0;
        SetValue(UIType.Wave, 1);
        
        if (currentWave <= wave)
        {
            currentMap.WaveStart();
        }
        else
        {
            currentMap.BossSpawn(transform, bossMonster,bossAdd);
        }
    }

    public void ResetData()
    {
        if (currentMap != null)
        {
            PoolManager.Instance.Enqueue(currentMap.name, currentMap.gameObject);
            currentMap = null;
        }

        AllMonsterEnqueue();

        isOn = false;
        currentTime = 0;
        currentWave = 0;
        currentCoin = 0;

        life = 0;
        wave = 0;
        spawnTime = 0;
        addValue = 0;
        bossAdd = 0;
        bossMonster = "";
        monsters.Clear();
    }

    void FixedUpdate()
    {
        if (isOn)
        {
            currentTime += Time.deltaTime;

            if (currentTime >= spawnTime)
            {
                currentTime = 0;
                currentMap.Spawn(transform,monsters,currentWave,addValue);
            }
        }
    }

    void TextAllUpdate()
    {
        UIManager.Instance.SetText(UIType.Wave, $"{currentWave}/{wave}");
        UIManager.Instance.SetText(UIType.Coin, $"{currentCoin:N0}");
        UIManager.Instance.SetText(UIType.Life, $"{currentLife}/{life}");
    }

    public void UseDrawCoin()
    {
        SetValue(UIType.Coin,-(int)defineScriptable.GetData(needDrawKey).value);
    }

    public void SetValue(UIType uIType,int value)
    {
        switch (uIType)
        {
            case UIType.Wave:
                currentWave += value;
                UIManager.Instance.SetText(UIType.Wave, $"{currentWave}/{wave}");
                break;
            case UIType.Life:
                currentLife += value;

                if (currentLife < 0)
                {
                    currentLife = 0;
                }

                UIManager.Instance.SetText(UIType.Life, $"{currentLife}/{life}");
                break;
            case UIType.Coin:
                currentCoin += value;
                UIManager.Instance.SetText(UIType.Coin, $"{currentCoin:N0}");
                break;
            default:
                break;
        }
    }

    public void OpenSpots(HeroData target)
    {
        currentMap.BlackBack(true,target);
    }

    public void AllMonsterEnqueue()
    {
        foreach (var item in monsterObjects)
        {
            item.Enqueue();
        }
    }
}
