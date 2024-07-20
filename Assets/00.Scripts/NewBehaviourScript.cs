using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public NetPlayerStone netPlayerStone;
    public PlayerInventory inv;
    public SkillManager sm;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            inv.TryAddSkill(sm.GetSkill(Skills.FireGaySkill));
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            inv.UseSkill(0, netPlayerStone);
        }
    }
}
