using System;
using System.Collections.Generic;
using UnityEngine;

public enum Skills : int
{
    IceExplosion,
    FireGaySkill,
    ALL
}

public class SkillManager : MonoBehaviour
{
    private readonly Dictionary<Skills, Skill> skillDic = new();
    
    private void Awake()
    {
        if (NetGameMana.INSTANCE.skillManager == null)
        {
            NetGameMana.INSTANCE.skillManager = this;
        }
        
    }
    private void Start()
    {
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
    public Action GetSkill(Skill sKills)
    {
        return default;
        //return skillDic[sKills].TryActivateSkill;
    }
}
