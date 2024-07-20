

public class WeightUpSKill : Skill
{
    public override void UIUse(NetPlayerStone netPlayerStone)
    {
        base.UIUse(netPlayerStone);

        netPlayerStone.weight += 20;
    }
}
