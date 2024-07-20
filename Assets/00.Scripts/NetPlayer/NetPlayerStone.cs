using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetPlayerStone : NetStone
{
    public Transform pivot;
    public int id;
    public bool isHost;
    private void Start()
    {
        if (NetGameMana.H_ISMULTI())
        {
            NetGameMana.INSTANCE.player.stones[isHost?0:1].Add(this);
        }
        else
        {
            NetGameMana.INSTANCE.playerOff.stones.Add(this);
        }
    }

    public override void ForceMove(Vector3 dir, float power, float damage)
    {
        base.ForceMove(dir, power*this.power, damage);
    }

    private void OnDisable()
    {
        if (NetGameMana.H_ISMULTI())
        {
            NetGameMana.INSTANCE.player.stones[isHost ? 0 : 1].Remove(this);
        }
        else
        {
            NetGameMana.INSTANCE.playerOff.stones.Remove(this);
        }
    }

    private void Update()
    {
        //print("ming");

        if (transform.position.y < -2)
        {
            Destroy(gameObject);
        }
    }
}
