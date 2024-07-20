using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SkillType
{
    Projectile,
    Others,
    Passive
}

public enum OtherSkills
{
    Health,
    Power,
    Mass,
    Bounde,
    Collide
}

[CreateAssetMenu(fileName="mnxzhnjkzxhjkm")]
public class SKillSO : ScriptableObject
{
    public List<GameObject> prefs = new List<GameObject>();//�̰� ����
    public SkillType skillType;//�̰ŷ� ����//
    public float Properies;
    public OtherSkills otherSkills;


}
