using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using UnityEngine;

public class BlackHoleSkill : Skill
{
    public LayerMask whatIsStone;
    public override void UIUse(NetPlayerStone netPlayerStone)
    {
        base.UIUse(netPlayerStone);
        ActivateSkill(netPlayerStone);
        
        Collider[] cols = Physics.OverlapSphere(netPlayerStone.transform.position , 30 ,whatIsStone );
        foreach (var item in cols)
        {
            item.GetComponentInParent<Rigidbody>()
                .AddForce((item.transform.position - transform.position).normalized * 2000);
        }
        
    }
}
