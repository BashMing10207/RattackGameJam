using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetStone : NetAgent
{
    public override void Die()
    {
        
    }

    public virtual void ForceMove(Vector3 dir,float power,float damage)
    {
        rb.AddForce(dir*power);
    }
}
