using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravituSkill : Skill
{
    [SerializeField] private int amount;
    protected override void ActivateSkill(NetPlayerStone netPlayerStone)
    {
        base.ActivateSkill(netPlayerStone);
        netPlayerStone.weight += amount;
    }
    protected override void OnDeregisterEvent(NetPlayerStone netPlayerStone)
    {
        base.OnDeregisterEvent(netPlayerStone);
        netPlayerStone.weight -= amount;
    }

}
