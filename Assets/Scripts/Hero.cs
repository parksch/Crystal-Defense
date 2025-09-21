using JsonClass;
using Unity.VisualScripting;
using UnityEngine;

public class Hero : MonoBehaviour
{
    [SerializeField] protected string index;
    [SerializeField] protected string attackKey;
    [SerializeField] protected string skillKey;
    [SerializeField] protected int attack;
    [SerializeField] protected int skillpoint;
    [SerializeField] protected float attackSpeed;
    [SerializeField] protected SphereCollider sphereCollider;
    [SerializeField] GameObject render;
    [SerializeField] Animator animator;
    [SerializeField] Monster target;

    public string Index => index;

    Vector3 normal;
    int currentSp;

    public float GetState(ClientEnum.State state)
    {
        switch (state)
        {
            case ClientEnum.State.Attack:
                return attack;
            case ClientEnum.State.AttackSpeed:
                return attackSpeed;
            case ClientEnum.State.AttackRange:
                return sphereCollider.radius;
            default:
                break;
        }

        return 0;
    }

    public void Set(HeroData set)
    {
        target = null;
        render = PoolManager.Instance.Dequeue(set.prefab);
        render.transform.parent = transform;
        render.SetActive(true);
        index = set.index;
        attack = set.attack;
        attackKey = set.attackPrefab;
        attackSpeed = set.attackSpeed;
        sphereCollider.radius = set.attackRange;
        skillKey = set.skill;
        skillpoint = set.skillPoint;

        currentSp = 0;
        animator = render.GetComponentInChildren<Animator>();
        animator.SetFloat("AttackSpeed", attackSpeed);

        AnimationEvent animationEvent = render.GetComponentInChildren<AnimationEvent>();
        animationEvent.Set();
        animationEvent.AddAction(OnAttack);
        animationEvent.AddAction(OnSkill);
    }

    void OnAttack()
    {
        if (target == null)
        {
            return;
        }

        AddSp();
        Attack attackObject = PoolManager.Instance.Dequeue(attackKey).GetComponent<Attack>();
        attackObject.Set(this, target, attackKey, attack);
    }

    void OnSkill()
    {
        if (target == null)
        {
            return;
        }

        Skill skillObject = PoolManager.Instance.Dequeue(skillKey).GetComponent<Skill>();
        skillObject.Set(this, target, skillKey, attack);
    }

    void FixedUpdate()
    {
        if (target != null)
        {
            if (target.IsDeath)
            {
                target = null;
                animator.SetBool("Attack", false);
                return;
            }

            normal = (target.transform.position - transform.position).normalized;

            if (normal.x < 0)
            {
                render.transform.localScale = new Vector3(-1,1,1);
            }
            else
            {
                render.transform.localScale = Vector3.one;
            }
        }    
    }

    void OnTriggerStay(Collider other)
    {
        if (target == null)
        {
            target = other.GetComponent<Monster>();
            animator.SetBool("Attack", true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (target != null && other.gameObject == target.gameObject)
        {
            target = null;
            animator.SetBool("Attack", false);
        }
    }

    public void ResetHero()
    {
        target = null;

        animator.SetBool("Attack", false);
        animator.Play("Idle", -1, 0f);

        PoolManager.Instance.Enqueue(render.name, render.gameObject);
        PoolManager.Instance.Enqueue(name, gameObject);
        
        render = null;
    }

    public void AddSp()
    {
        currentSp += (int)ScriptableManager.Instance.Get<DefineDataScriptable>(ScriptableType.DefineData).GetData("AddSkillPoint").value;

        if (currentSp >= skillpoint)
        {
            currentSp -= skillpoint;
            OnSkill();
        }
    }
}
