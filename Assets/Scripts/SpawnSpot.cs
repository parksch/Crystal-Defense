using JsonClass;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSpot : Spot
{
    [SerializeField] List<Spot> spots = new List<Spot>();
    [SerializeField] string monsterKey;

    public bool IsSpawnDone => count == listData.spawns.Count;

    List<SpawnListData> spawnListDatas = new List<SpawnListData>();
    SpawnListData listData;
    int count = 0;


    public void Set(List<string> list)
    {
        spawnListDatas.Clear();

        for (int i = 0; i < list.Count; i++)
        {
            SpawnListData listData = ScriptableManager.Instance.Get<SpawnListDataScriptable>(ScriptableType.SpawnListData).GetData(list[i]);
            spawnListDatas.Add(listData);
        }
    }

    public void WaveStart()
    {
        count = 0;
    }

    public void Spawn(Transform parent,int wave,List<string> monsters,float addValue)
    {
        listData = spawnListDatas[(wave - 1) % spawnListDatas.Count];

        if (count < listData.spawns.Count)
        {
            SpawnMonster(parent, monsters[listData.spawns[count] - 1], wave, addValue,false);
            count++;
        }
    }

    public void Move(Monster target,MonsterData data,float addValue,bool isBoss)
    {
        target.transform.position = transform.position;
        target.gameObject.SetActive(true);
        target.Set(spots, data, addValue, isBoss);
    }

    public void SpawnMonster(Transform parent,string monster, int wave, float addValue,bool isBoss)
    {
        string monsterName = monster;
        Monster monsterObject = PoolManager.Instance.Dequeue(monsterName).GetComponent<Monster>();
        MonsterData monsterData = ScriptableManager.Instance.Get<MonsterDataScriptable>(ScriptableType.MonsterData).GetData(monsterName);
        monsterObject.transform.parent = parent;
        Move(monsterObject, monsterData, (wave - 1) * addValue, isBoss);
        GameManager.Instance.AddMonster(monsterObject);
    }
}
