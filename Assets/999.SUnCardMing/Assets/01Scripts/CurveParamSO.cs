using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/CurveParam")]
public class CurveParamSO : ScriptableObject
{
    public AnimationCurve positioning;
    public float positionInfluence = 0.02f;
    public AnimationCurve rotation;
    public float rotationInfluence = 1.2f;
}
