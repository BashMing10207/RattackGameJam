using System;
using System.Collections.Generic;
using UnityEngine;

public enum Skills : int
{
    IceExplosion,
    FireGaySkill,
    Gravity,
    PowerUp,
    Thunder,
    WeightUp,
    Bolt,
    ALL
}

public class SkillManager : MonoBehaviour
{
    private readonly Dictionary<Skills, Skill> skillDic = new();
    
    private void Awake()
    {
        if (NetGameMana.Instance.skillManager == null)
        {
            NetGameMana.Instance.skillManager = this;
        }
        
        var list = GetComponentsInChildren<Skill>();
        for (Skills i = 0; i < Skills.ALL; i++)
        {
            skillDic[i] = list[(int)i];
        }
        foreach (var a in skillDic)
        {
            print("key : " + a.Key + " Value : " + a.Value);
        }
        
    }
    public Skill GetSkill(Skills skillToGet)
    {
        return skillDic[skillToGet];
    }

}
