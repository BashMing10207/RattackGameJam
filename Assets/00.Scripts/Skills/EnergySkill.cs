using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergySkill : Skill
{
    public LayerMask whatIsBadukdol;
    public override void UIUse(NetPlayerStone netPlayerStone)
    {
        base.UIUse(netPlayerStone);

        Collider[] cols = Physics.OverlapSphere(netPlayerStone.transform.position , 7,whatIsBadukdol);

        foreach (var item in cols)
        {
            Rigidbody rb = item.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(50f, item.transform.position + Vector3.forward, 50f);
            }
        }
    }
}
