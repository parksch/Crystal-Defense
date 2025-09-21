using JsonClass;
using UnityEngine;

public class Slot : MonoBehaviour
{
    [SerializeField] string heroKey;
    [SerializeField] Transform heroParent;
    [SerializeField] Hero current;

    public bool SummonCheck(HeroData heroData)
    {
        return (current == null);
    }

    public void Init()
    {
        if (current != null)
        {
            current.ResetHero();
            current = null;
        }
    }

    public void OnClick(HeroData heroData)
    {
        current = PoolManager.Instance.Dequeue(heroKey).GetComponent<Hero>();
        current.Set(heroData);
        current.transform.parent = heroParent;
        current.transform.position = transform.position;
        current.gameObject.SetActive(true);
    }
}
