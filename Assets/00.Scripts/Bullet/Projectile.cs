using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : Bullet
{
    RaycastHit _hit;
    
    public Vector3 forawrdDir,startPos;
    public float speedMulti = 1;
    [SerializeField]
    float _maxTime = 40;
    [SerializeField]
    LayerMask targetLayer;
    [SerializeField]
    ProjectileSO _soData;

    float _time = 0;
    private void Start()
    {
        forawrdDir = transform.forward;
        startPos = transform.position;
    }

    public void Init(Vector3 forwardDirection,Vector3 startPoss,float speedMultif)
    {
        transform.forward = forwardDirection;
        transform.position = startPoss;
        _time = 0;
        speedMulti = speedMultif;
    }

    public virtual void Update()
    {
        if (Physics.SphereCast(transform.position,_soData.radius,transform.forward,out _hit,_soData.speed*Time.deltaTime*speedMulti,targetLayer))
        {
            if (_hit.transform.CompareTag("Hitable"))
            {
                if (NetGameMana.Instance != null)
                {
                    AttackTop(_hit.transform).GetDamage(new AttackStrc(transform.forward, _soData.damage, _soData.power, _soData.dieEffect), _hit.point);
                }
                else
                {
                   //AttackTop(_hit.transform,true).damage(new AttackStrc(transform.forward, _soData.damage, _soData.power, _soData.dieEffect), _hit.point);
                }
            }
            Die();
        }

        transform.position = transform.position+transform.forward*_soData.speed*Time.deltaTime*speedMulti;
        _time += Time.deltaTime;

        if(_time > _maxTime)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        Instantiate(_soData.dieEffect, transform.position, transform.rotation);
        if(NetGameMana.Instance != null)
        {
        NetGameMana.Instance.pool.Get(_soData, gameObject);
        }
        else
        {
            OLDGameMana.instance.pool.Get(_soData, gameObject);
        }
    }
}
