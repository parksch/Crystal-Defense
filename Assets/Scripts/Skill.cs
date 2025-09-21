using ClientEnum;
using JsonClass;
using System.Data;
using UnityEngine;

public class Skill : Attack
{
    [SerializeField] State state;
    [SerializeField] float stateAddvalue;

    public override void Set(Hero caster, Monster target, string key, float attack)
    {
        SkillData data = ScriptableManager.Instance.Get<SkillDataScriptable>(ScriptableType.SkillData).GetData(key);
        state = (State)data.targetState;
        stateAddvalue = data.value;
        attack = caster.GetState(state);
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

    protected override void TriggerAttack(Collider other)
    {
        other.GetComponent<Monster>().Hit((int)(attackValue * stateAddvalue));
    }
}
