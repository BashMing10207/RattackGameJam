using System.Collections.Generic;
using UnityEngine;

public class IceExplosionSkill : Skill
{
    public float explosionRadius;
    public LayerMask whatIsStone;
    public PhysicMaterial iceMaterial;
    public PhysicMaterial _originMaterial;
    private readonly List<PhysicMaterial> objsToReset = new();
    protected override void ActivateSkill(NetPlayerStone netPlayerStone)
    {
        base.ActivateSkill(netPlayerStone);
        Collider[] colliders = Physics.OverlapSphere(stone.transform.position ,explosionRadius , whatIsStone);
        
        foreach (var item in colliders)
        {
            var a = item.GetComponent<MeshCollider>().material = iceMaterial;
            objsToReset.Add(a);
        }
        //다음턴이 지났을때 다시 오리진으로 돌려줘야 됨.
    }
    protected override void OnDeregisterEvent(NetPlayerStone netPlayerStone)
    {
        base.OnDeregisterEvent(netPlayerStone);
        //여기서 오리진으로 되돌리셈
        for(int i = 0; i < objsToReset.Count; i++)
        {
            objsToReset[i] = _originMaterial;
        }
    }

}
