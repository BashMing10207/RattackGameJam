using UnityEngine;

public class DropProjectile : Projectile
{
    [SerializeField]
    Vector3 _rotateDir = new Vector3(1,0,0);
    [SerializeField]
    float _downSpeed = 1;
    public override void Update()
    {
        transform.Rotate(_rotateDir.normalized * Time.deltaTime*_downSpeed);
        base.Update();
    }
}
