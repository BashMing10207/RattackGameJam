using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Boom : Bullet
{
    [SerializeField]
    LayerMask _layerMask;
    [SerializeField]
    float size = 5f,damage=1,power = 1000;
    Collider[] colliders = new Collider[35];
    List<Rigidbody> _Rigidbodies = new List<Rigidbody>();
    private void OnEnable()
    {
        this.GetComponent<NetworkObject>().Spawn();
        if(Physics.OverlapSphereNonAlloc(transform.position, size,colliders,_layerMask) > 0)
        {
            print("mmdsf");
        for(int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i] != null)
                if (colliders[i].CompareTag("Hitable"))
                {
                    AttackTop(colliders[i].transform).GetDamage(new AttackStrc((colliders[i].transform.position-transform.position).normalized*3+Vector3.up,damage,power,null));
                }
            }

        }
    Destroy(gameObject,4f);
    }
}
