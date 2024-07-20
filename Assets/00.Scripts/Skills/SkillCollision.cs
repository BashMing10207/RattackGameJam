using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillCollision : MonoBehaviour
{
    private readonly List<Collision> activatedCollisions = new();
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("gay"))
        {
            if (!activatedCollisions.Contains(collision))
            {
                activatedCollisions.Add(collision);
                if (collision.transform.TryGetComponent(out NetPlayerStone otherNetPlayerStone))
                {
                    OnEnter(otherNetPlayerStone);        
                }
            }
        }
    }
    protected virtual void OnEnter(NetPlayerStone netPlayerStone)
    {
    }
    public virtual void OnExit(NetPlayerStone netPlayerStone)
    {

    }
}
