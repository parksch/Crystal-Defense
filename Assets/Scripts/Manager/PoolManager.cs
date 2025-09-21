using UnityEngine;
using System.Collections.Generic;

public class PoolManager : Singleton<PoolManager>
{
    [SerializeField] RectTransform rect;
    Dictionary<string,Queue<GameObject>> pools = new Dictionary<string,Queue<GameObject>>();
    
    public void Enqueue(string key,GameObject obj)
    {
        if (!pools.ContainsKey(key))
        {
            pools[key] = new Queue<GameObject>();
        }

        if (obj.TryGetComponent<RectTransform>(out _))
        {
            obj.transform.SetParent(rect);
        }
        else
        {
            obj.transform.SetParent(transform);
        }

        ResetObject(obj);
        pools[key].Enqueue(obj);
    }
    
    public GameObject Dequeue(string key)
    {
        if (!pools.ContainsKey(key))
        {
            pools[key] = new Queue<GameObject>();
        }

        GameObject obj = null;

        if (pools[key].Count == 0)
        {
            GameObject prefab = ResourcesManager.Instance.GetPrefab(key);
            GameObject copy = null;

            if (prefab.TryGetComponent<RectTransform>(out _))
            {
                copy = Instantiate(prefab, rect);
            }
            else
            {
                copy = Instantiate(prefab, transform);
            }

            copy.name = prefab.name;
            ResetObject(copy);
            obj = copy;
        }
        else
        {
            obj = pools[key].Dequeue();
        }

        return obj;
    }

    GameObject ResetObject(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localRotation = Quaternion.identity;
        obj.transform.localScale = Vector3.one;

        return obj;
    }
}
