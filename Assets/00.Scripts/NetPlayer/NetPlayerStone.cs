using UnityEngine;

public class NetPlayerStone : NetStone
{
    public Transform pivot;
    public int id;
    public bool isHost;

    public float health;
    public float weight;
    public float force;
    
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

    public void AddHealth(float amount)
    {
        health += amount;
    }
    
    public void AddForce(float amount)
    {
        force += amount;
    }
    
    public void AddWeight(float amount)
    {
        weight += amount;
    }
    
}
