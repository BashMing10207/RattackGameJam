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
    public List<GameObject> prefs = new List<GameObject>();//이거 생성
    public SkillType skillType;//이거로 유형//
    public float Properies;
    public OtherSkills otherSkills;


}
