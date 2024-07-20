

using UnityEngine;

public class PowerUp : Skill
{
    [SerializeField] private int amount;
    protected override void ActivateSkill(NetPlayerStone netPlayerStone)
    {
        base.ActivateSkill(netPlayerStone);
        netPlayerStone.hp += amount;
        netPlayerStone.Cadwdawad();
    }
}
