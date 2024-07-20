using System;
using UnityEngine;

public class NetPlayerStone : NetStone
{
    public Transform pivot;
    public GameObject outLine;
    
    public int id;
    public bool isHost;
    
    public float health;
    public float weight;
    public float force;
    public event Action<NetPlayerStone> Actions;
    private void Awake()
    {
        NetCPlayer.OnTurnEnd += HandleOnTurn;
    }
    private void Start()
    {
        if (NetGameMana.H_ISMULTI())
        {
            NetCPlayer.stones[isHost?0:1].Add(this);
        }
        else
        {
            NetGameMana.Instance.playerOff.stones.Add(this);
        }
        
        outLine.SetActive(false);
    }
    private void HandleOnTurn()
    {
        //print("HandleON")
        Actions?.Invoke(this);
    }
    public override void ForceMove(Vector3 dir, float power, float damage)
    {
        base.ForceMove(dir, power*this.power, damage);
    }

    private void OnDisable()
    {
        NetCPlayer.OnTurnEnd -= HandleOnTurn;
        if (NetGameMana.H_ISMULTI())
        {
            NetCPlayer.stones[isHost ? 0 : 1].Remove(this);
        }
        else
        {
            NetGameMana.Instance.playerOff.stones.Remove(this);
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
