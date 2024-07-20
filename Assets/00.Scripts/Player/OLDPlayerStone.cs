using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OLDPlayerStone : OLDStone 
{ 
    public Transform pivot;
    public int id;
    public bool isHost;
    private void Start()
    {
        //GameMana.instance.player.stones.Add(this);
    }

    public override void ForceMove(Vector3 dir, float power, float damage)
    {
        base.ForceMove(dir, power * this.power, damage);
    }

    private void OnDisable()
    {
        //GameMana.instance.player.stones.Remove(this);
    }

    private void Update()
    {
        if (transform.position.y < -2)
        {
            Destroy(gameObject);
        }
    }
}
