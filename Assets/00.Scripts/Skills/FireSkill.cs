using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSkill : Skill
{
    protected override void ActivateSkill(NetPlayerStone netPlayerStone)
    {
        base.ActivateSkill(netPlayerStone);
        //gameObject.AddComponent<>
    }
    protected override void OnDeregisterEvent(NetPlayerStone netPlayerStone)
    {
        base.OnDeregisterEvent(netPlayerStone);
        //Destroy(GetComponent<>);
    }
}
