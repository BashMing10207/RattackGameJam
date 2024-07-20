using System;
using System.Collections.Generic;
using UnityEngine;

public enum SKills
{
    IceExplosion,
    
}

public class SkillManager : MonoBehaviour
{
    public IceExplosionSkill IceExplosionSkill { get; private set; }

    private Dictionary<SKills, IceExplosionSkill> skillDic = new Dictionary<SKills, IceExplosionSkill>();
    
    private void Awake()
    {
        if (NetGameMana.INSTANCE.skillManager == null)
        {
            NetGameMana.INSTANCE.skillManager = this;
        }
    }

    private void Start()
    {
        IceExplosionSkill = GetComponent<IceExplosionSkill>();
        
        
        
    }

    public Action GetSkill(SKills sKills)
    {
        return skillDic[sKills].TryActivateSkill;
    }
}
