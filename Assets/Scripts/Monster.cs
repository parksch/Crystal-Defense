using JsonClass;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] Rigidbody rigid;
    [SerializeField] Animator animator;
    [SerializeField] List<Spot> path = new List<Spot>();
    [SerializeField] float speed;
    [SerializeField] int hp;
    [SerializeField] int coin;
    [SerializeField] int subtractionLife;
    [SerializeField] Vector3 noraml;
    [SerializeField] AnimationEvent animationEvent;

    public bool IsBoss => isBoss;
    public bool IsDeath => curHp <= 0;

    bool isOn = false;
    bool isBoss = false;
    float closeDist = 0.1f;
    float curSpeed;
    int curHp;

    public void Set(List<Spot> spots, MonsterData monsterData,float addValue,bool isBoss =false)
    {
        this.isBoss = isBoss;
        isOn = true;
        path = spots.ToList();
        speed = monsterData.speed;
        hp =  monsterData.hp + ((int)(monsterData.hp * addValue));
        coin = monsterData.addCoin;
        subtractionLife = monsterData.subtractionLife;
        curHp = hp;
        curSpeed = speed;

        if (isBoss)
        {
            animator.gameObject.transform.localScale = Vector3.one * 2;
        }
        else
        {
            animator.gameObject.transform.localScale = Vector3.one;
        }

        animator.SetBool("isStop", false);
        animator.SetBool("isDeath", false);
        animator.SetFloat("X", 0);
        animator.SetFloat("Y", 0);
        animator.SetFloat("Speed", curSpeed / speed);
        animationEvent.Set();
        animationEvent.AddAction(Death);
    }

    void FixedUpdate()
    {
        if (isOn && curHp > 0)
        {
            if (Vector3.Distance(transform.position, path[0].transform.position) < closeDist)
            {
                transform.position = path[0].transform.position;
                noraml = path[0].GetDiraction;

                if (noraml == Vector3.zero)
                {
                    animator.SetBool("isStop", true);
                }
                else
                {
                    animator.SetFloat("X", noraml.x);
                    animator.SetFloat("Y", noraml.z);
                }

                if (path[0].IsEnd)
                {
                    isOn = false;

                    GameManager.Instance.AddEnemyDeathEvent(() =>
                    {
                        GameManager.Instance.SubtractionLife(-subtractionLife);
                        GameManager.Instance.RemoveMonster(this);
                        GameManager.Instance.WaveCheck();
                    });

                    GameManager.Instance.OnEnemyDeath();

                    PoolManager.Instance.Enqueue(name, gameObject);
                }

                path.RemoveAt(0);
            }
            else
            {
                rigid.MovePosition(transform.position + noraml * Time.deltaTime * speed);
            }
        }    
    }

    public void Hit(int attack)
    {
        if (curHp <= 0)
        {
            return;
        }

        curHp -= attack;
        curHp = curHp < 0 ? 0 : curHp;

        if (curHp == 0)
        {
            isOn = false;

            GameManager.Instance.AddEnemyDeathEvent(() =>
            {
                GameManager.Instance.AddCoin(coin);
                GameManager.Instance.RemoveMonster(this);
                GameManager.Instance.WaveCheck();
            });

            GameManager.Instance.OnEnemyDeath();

            animator.SetBool("isDeath", true);
        }
    }

    void Death()
    {
        PoolManager.Instance.Enqueue(name, gameObject);
    }

    public void Enqueue()
    {
        isOn = false;
        PoolManager.Instance.Enqueue(name, gameObject);
    }
}
