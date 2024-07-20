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
                Debug.LogWarning("������Ʈ ���� �׎��� �±״� �� �޾Ƴ���??");
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
    //            Debug.LogWarning("������Ʈ ���� �׎��� �±״� �� �޾Ƴ���??");
    //            result = null;
    //            return null;
    //        }
    //    }
    //    result = ming;
    //    return null;
    //}
}
