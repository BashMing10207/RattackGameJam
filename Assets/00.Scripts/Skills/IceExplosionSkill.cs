using UnityEngine;

public class IceExplosionSkill : Skill
{
    public float explosionRadius;
    public LayerMask whatIsStone;
    public PhysicMaterial iceMaterial;
    public PhysicMaterial _originMaterial;
    
    protected override void ActivateSkill(NetPlayerStone netPlayerStone)
    {
        base.ActivateSkill(netPlayerStone);
        Collider[] colliders = Physics.OverlapSphere(stone.transform.position ,explosionRadius , whatIsStone);

        foreach (var item in colliders)
        {
            item.GetComponent<MeshCollider>().material = iceMaterial;
        }
        //다음턴이 지났을때 다시 오리진으로 돌려줘야 됨.
        
        
    }
    
    
}
