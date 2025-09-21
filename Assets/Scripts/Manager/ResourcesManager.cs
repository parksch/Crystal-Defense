using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class ResourcesManager : Singleton<ResourcesManager>
{
    [SerializeField] List<string> resourcesFile = new List<string>();
    Dictionary<string,GameObject> prefabs = new Dictionary<string,GameObject>();
    Dictionary<string,SpriteAtlas> atlasDict = new Dictionary<string,SpriteAtlas>();

    protected override void Awake()
    {
        var spriteAtlas = Resources.LoadAll<SpriteAtlas>("SpriteAtlas");

        foreach (var atlas in spriteAtlas)
        {
            atlasDict[atlas.name] = atlas;
        }

        foreach (var file in resourcesFile)
        {
            var objects = Resources.LoadAll<GameObject>($"Prefab/{file}");
            foreach (var gameObject in objects)
            {
                prefabs[gameObject.name] = gameObject;
            }
        }
    }

    public Sprite GetSprite(string atlas,string name)
    {
        SpriteAtlas spriteAtlas = atlasDict[atlas];
        return spriteAtlas.GetSprite(name);
    }

    public GameObject GetPrefab(string key)
    {
        return prefabs[key];
    }
}
