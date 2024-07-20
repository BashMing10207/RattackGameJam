using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OLDAgent : MonoBehaviour
{
    public float maxhp = 10, hp = 10, mass = 1, power = 10;
    public Rigidbody rb;
    void Start()
    {
        hp = maxhp;
    }
    public virtual void damage(float damage)
    {
        hp -= damage;
        hp = Mathf.Clamp(hp, 0.05f, maxhp);

        rb.mass = mass * hp / maxhp;
    }
    public virtual void damage(AttackStrc atsource)
    {
        damage(atsource.damage);
        rb.AddForce(atsource.accelDir * atsource.power);
        print("mmm");
    }

    public virtual void damage(AttackStrc atsource, Vector3 hitPos)
    {
        damage(atsource.damage);
        rb.AddForceAtPosition(atsource.accelDir * atsource.power, hitPos);
    }

    public abstract void die();
}
