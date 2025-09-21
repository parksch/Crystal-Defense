using JsonClass;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Map : MonoBehaviour
{
    [SerializeField] List<SpawnSpot> spawnSpots;
    [SerializeField] ClickSummonHero clickSummon;
    [SerializeField] Transform HeroParent;
    [SerializeField] GameObject blackBack;

    public bool IsSpawnEnd()
    {
        for (int i = 0; i < spawnSpots.Count; i++)
        {
            if(!spawnSpots[i].IsSpawnDone)
            {
                return false;
            }
        }

        return true;
    }

    public void BlackBack(bool onOff, HeroData data)
    {
        clickSummon.SetHeroData(data);
        blackBack.SetActive(onOff);
    }

    public void Set(MapData mapData,List<string> spawn)
    {
        clickSummon.ResetSummonSpots();

        for (int i = 0; i < spawnSpots.Count; i++)
        {
            spawnSpots[i].Set(spawn);
        }
    }

    public void WaveStart()
    {
        for (int i = 0; i < spawnSpots.Count; i++)
        {
            spawnSpots[i].WaveStart();
        }
    }

    public void Spawn(Transform parent,List<string> monsters,int wave,float addValue)
    {
        for (int i = 0; i < spawnSpots.Count; i++)
        {
            spawnSpots[i].Spawn(parent,wave, monsters, addValue);
        }
    }

    public void BossSpawn(Transform parent, string bossMonster,float addValue)
    {
        for (int i = 0; i < spawnSpots.Count; i++)
        {
            spawnSpots[i].SpawnMonster(parent, bossMonster, 2, addValue, true);
        }
    }

}
