using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class Skill : MonoBehaviour
{
    public Transform stone;
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
    public virtual void TryActivateSkill(NetPlayerStone netPlayerStone)
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
