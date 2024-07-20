using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public virtual NetAgent AttackTop(Transform trm)
    {
        NetAgent ming;
        while (!trm.TryGetComponent<NetAgent>(out ming))
        {
            if(trm.parent != null)
            trm = trm.parent;
            else
            {
                Debug.LogWarning("에이전트 없음 그럱데 태그는 왜 달아놓음??");
                return null;
            }
        }

        return ming;
    }
    //public virtual NetAgent AttackTop(Transform trm,out Agent result)
    //{
    //    Agent ming;
    //    while (!trm.TryGetComponent<Agent>(out ming))
    //    {
    //        if (trm.parent != null)
    //            trm = trm.parent;
    //        else
    //        {
    //            Debug.LogWarning("에이전트 없음 그럱데 태그는 왜 달아놓음??");
    //            result = null;
    //            return null;
    //        }
    //    }
    //    result = ming;
    //    return null;
    //}
}
