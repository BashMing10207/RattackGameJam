using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    public const int maxInventoryAmount = 3;
    private readonly List<Skill> skills = new();
    public List<Skill> GrtSkills => skills;
    public static Action OnInventoryChange;
    /// <summary>
    /// adds skill to inventory if possible
    /// </summary>
    public void TryAddSkill(Skill skillToAdd)
    {
        void AddSkill()
        {
            skills.Add(skillToAdd);
            OnInventoryChange?.Invoke();
        }
        if (skills.Count < maxInventoryAmount)
        {
            AddSkill();
        }
        else
        {
            Debug.LogError($"skill is already max number {maxInventoryAmount}");
        }
    }
    /// <summary>
    /// removes skill from inventory if possible
    /// </summary>
    public void TryRemoveSkill(Skill skillToRemove)
    {
        void RemoveSkill()
        {
            skills.Remove(skillToRemove);
            OnInventoryChange?.Invoke();
        }
        if (skills.Contains(skillToRemove))
        {
            RemoveSkill();
        }
        else
        {
            Debug.LogError($"skillList doesn't have a skill to remove");
        }
    }
    public void UseSkill(int index, NetPlayerStone netStone)
    {
        skills[index].TryActivateSkill(netStone);
    }
}
