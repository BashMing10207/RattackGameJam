using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSkill : Skill
{
    public override void UIUse(NetPlayerStone netPlayerStone)
    {
        base.UIUse(netPlayerStone);
        print("upgrade myself");
    }



}
