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
    [SerializeField] private int endTurnAmount;
    [SerializeField] private int endTurnAmountMax;
    protected virtual void Awake()
    {
        if(name == "_")
            name = ToString();
    }
    public void TryActivateSkill(NetPlayerStone netPlayerStone)
    {
        ActivateSkill(netPlayerStone);
    }

    protected virtual void ActivateSkill(NetPlayerStone netPlayerStone)
    {
        endTurnAmount = endTurnAmountMax;
        foreach (var item in particleSystems)
        {
            item.Play();
        }
        netPlayerStone.Actions += OnEndTurn;
    }
    protected virtual void OnEndTurn(NetPlayerStone netPlayerStone)
    {
        endTurnAmount--;
        if(endTurnAmount < 0)
        {
            netPlayerStone.Actions -= OnEndTurn;
            print("ended skill effect");
        }
    }
}
