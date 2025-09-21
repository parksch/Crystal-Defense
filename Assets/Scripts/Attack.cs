using ClientEnum;
using JsonClass;
using System.Threading;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class Attack : MonoBehaviour
{
    [SerializeField] protected float attackValue;
    [SerializeField] protected float optionalValue;
    [SerializeField] protected float optionalValue2;
    [SerializeField] protected DestoryType destoryType;
    [SerializeField] protected bool targetFallow;
    [SerializeField] protected GameObject reverseObject;

    protected float currentValue;
    protected Hero hero;
    protected Monster monster;
    protected Vector3 des;

    public virtual void Set(Hero caster, Monster target, string key,float attack)
    {
        AttackData data = ScriptableManager.Instance.Get<AttackDataScriptable>(ScriptableType.AttackData).GetData(key);
        
        DetaSet(
            caster,
            target,
            attack,
            data.targetFallow,
            data.optionalValue,
            data.optionalValue2,
            (DestoryType)data.destoryType,
            (StartType)data.startType);
    }

    protected virtual void DetaSet(Hero caster, Monster target, float attack,bool targetFallow,float optionalValue,float optionalValue2, DestoryType destoryType,StartType startType)
    {
        Vector3 normal = target.transform.position - caster.transform.position;
        this.targetFallow = targetFallow;
        des = target.transform.position;
        hero = caster;
        monster = target;
        normal.y = 0;

        attackValue = attack;
        this.optionalValue = optionalValue;
        this.optionalValue2 = optionalValue2;
        this.destoryType = destoryType;

        switch (startType)
        {
            case StartType.Caster:
                transform.position = caster.transform.position;
                break;
            case StartType.Target:
                transform.position = target.transform.position;
                break;
            default:
                break;
        }

        currentValue = 0;
        transform.rotation = Quaternion.LookRotation(normal);

        if (reverseObject != null)
        {
            if (normal.x < 0)
            {
                reverseObject.transform.localRotation = Quaternion.Euler(0, 90, 180);
            }
            else
            {
                reverseObject.transform.localRotation = Quaternion.Euler(0, 270, 0);
            }
        }

        gameObject.SetActive(true);
    }

    void FixedUpdate()
    {
        switch (destoryType)
        {
            case DestoryType.Arrive:
                if (targetFallow && !monster.IsDeath)
                {
                    des = monster.transform.position;
                }

                transform.position = Vector3.MoveTowards(transform.position, des, optionalValue * Time.deltaTime);

                if (Vector3.Distance(transform.position,des) <= optionalValue * Time.deltaTime)
                {
                    PoolManager.Instance.Enqueue(name, gameObject);
                }

                break;
            case DestoryType.TimeOut:
                currentValue += Time.deltaTime;

                if (currentValue >= optionalValue)
                {
                    PoolManager.Instance.Enqueue(name,gameObject);
                }
                break;
            default:
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        TriggerAttack(other);
    }

    protected virtual void TriggerAttack(Collider other)
    {
        other.GetComponent<Monster>().Hit((int)attackValue);
    }
}
