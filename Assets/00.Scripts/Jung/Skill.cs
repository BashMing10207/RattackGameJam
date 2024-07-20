using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Skill : MonoBehaviour
{
    [SerializeField] protected float skillCoolDown;
    protected float _skillTimer;
    private List<ParticleSystem> particleSystems;
    
    public virtual void TryActivateSkill()
    {
        if (skillCoolDown <= _skillTimer)
        {
            _skillTimer = 0;
            ActivateSkill();
        }
    }

    private void Update()
    {
        _skillTimer += Time.deltaTime;
    }

    protected virtual void ActivateSkill()
    {
        foreach (var item in particleSystems)
        {
            item.Play();
        }
        
        
    }
}
