using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireExplosionSkill : Skill
{
    protected override void ActivateSkill(NetPlayerStone netPlayerStone)
    {
        base.ActivateSkill(netPlayerStone);
        
        
        
    }

    protected override void OnEndTurn(NetPlayerStone netPlayerStone)
    {
        base.OnEndTurn(netPlayerStone);
    }

    protected override void OnDeregisterEvent(NetPlayerStone netPlayerStone)
    {
        base.OnDeregisterEvent(netPlayerStone);
    }
}
