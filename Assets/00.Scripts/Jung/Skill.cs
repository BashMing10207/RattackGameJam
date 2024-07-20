using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Skill : MonoBehaviour
{
    public Transform stone;
    private List<ParticleSystem> particleSystems;
        
    public virtual void TryActivateSkill()
    {
        ActivateSkill();
    }

    protected virtual void ActivateSkill()
    {
        foreach (var item in particleSystems)
        {
            item.Play();
        }
        
    }
}
