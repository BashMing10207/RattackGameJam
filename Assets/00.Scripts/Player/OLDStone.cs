using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OLDStone : OLDAgent
{
    public override void die()
    {

    }

    public virtual void ForceMove(Vector3 dir, float power, float damage)
    {
        rb.AddForce(dir * power);
    }
}
