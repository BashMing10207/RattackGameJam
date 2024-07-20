using UnityEngine;

public class ThunderSkill : Skill
{
    public LayerMask whatIsStone;
    public override void UIUse(NetPlayerStone netPlayerStone)
    {
        base.UIUse(netPlayerStone);
        ActivateSkill(netPlayerStone);
        Collider[] cols = Physics.OverlapSphere(netPlayerStone.transform.position , 20 ,whatIsStone);
        foreach (var item in cols )
        {
            item.GetComponentInParent<NetPlayerStone>().health -= 20;
        }

    }
}
