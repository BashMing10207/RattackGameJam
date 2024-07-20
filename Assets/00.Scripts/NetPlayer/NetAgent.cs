using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public struct AttackStrc
{
    public Vector3 accelDir;
    public float damage;
    public float power;
    public GameObject effect;

    public AttackStrc(Vector3 accelDir, float damage, float power,GameObject effect)
    {
        this.accelDir = accelDir;
        this.damage = damage;
        this.power = power;
        this.effect = effect;
    }
};

public abstract class NetAgent : NetworkBehaviour
{
    public float maxhp = 10, hp = 10,mass=1,power=10,bounce=0;
    public void Cadwdawad()
    {
        //fuckfcuckfuck
        //프로퍼티좀써라게이야..
        hp = hp > maxhp ? maxhp : hp;
    }
    public Rigidbody rb;
    void Start()
    {
        hp = maxhp;
    }
    public virtual void GetDamage(float damage)
    {
        hp -= damage;
        hp = Mathf.Clamp(hp, 0.05f, maxhp);

        rb.mass = mass * hp / maxhp;
    }
    public virtual void GetDamage(AttackStrc atsource)
    {
        GetDamage(atsource.damage);
        rb.AddForce(atsource.accelDir*atsource.power);
        print("mmm");
    }

    public virtual void GetDamage(AttackStrc atsource,Vector3 hitPos)
    {
        GetDamage(atsource.damage);
        rb.AddForceAtPosition(atsource.accelDir*atsource.power, hitPos);
    }

    public abstract void Die();
}
