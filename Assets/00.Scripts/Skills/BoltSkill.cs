using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoltSkill : Skill
{
    public LayerMask whatIsStone;
    public override void UIUse(NetPlayerStone netPlayerStone)
    {
        base.UIUse(netPlayerStone);
        
        ActivateSkill(netPlayerStone);
        Collider[] cols = Physics.OverlapSphere(netPlayerStone.transform.position , 20 ,whatIsStone);
        foreach (var item in cols )
        {
            item.GetComponent<NetPlayerStone>().weight += 40;
            item.GetComponent<NetPlayerStone>().force -= 20;
        }
    }
}