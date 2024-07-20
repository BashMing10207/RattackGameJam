using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Skill : MonoBehaviour
{
    [SerializeField] private SO_CardAsset so_Card;
    public SO_CardAsset GetCardSO => so_Card;

    [SerializeField] protected float skillCoolDown;
    protected float _skillTimer;
    private List<ParticleSystem> particleSystems;
    protected virtual void Awake()
    {
        if(name == "_")
            name = ToString();
    }
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
