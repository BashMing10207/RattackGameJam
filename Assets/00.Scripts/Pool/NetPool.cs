using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


public class NetPool : NetworkBehaviour
{
    public Dictionary<ProjectileSO,Stack<GameObject>>poolMing = new Dictionary<ProjectileSO,Stack<GameObject>>();

    // Start is called before the first frame update
    void OnEnable()
    {
        NetGameMana.INSTANCE.pool = this;
    }

    public void Get(ProjectileSO prjtype, GameObject target)
    {
        Create(prjtype);
        poolMing[prjtype].Push(target);
        target.SetActive(false);
    }

    public GameObject Give(ProjectileSO prjtype, Transform targetTr)
    {
        Create(prjtype);

        GameObject gameObject;

        if (!poolMing[prjtype].TryPeek(out gameObject))
        {

            //gameObject = Instantiate(prjtype.gameObj, transform.position, transform.rotation, transform);
            gameObject = Instantiate(prjtype.gameObj, transform.position, transform.rotation, transform);
        }
        else
        {
            poolMing[prjtype].Pop();
        }
        gameObject.transform.SetPositionAndRotation(targetTr.position, targetTr.rotation);
        gameObject.SetActive(true);
        if (NetGameMana.H_ISMULTI())
            gameObject.GetComponent<NetworkObject>().Spawn(true);
        return gameObject;
    }

    public GameObject Give(ProjectileSO prjtype, Transform targetTr, float randomDeg)//������ �߰�. �� �����κ� �ݺ��� �ֳ�...
    {
        Create(prjtype);

        GameObject gameObject;

        if (!poolMing[prjtype].TryPeek(out gameObject))
        {
            gameObject = Instantiate(prjtype.gameObj, transform.position, transform.rotation, transform);
        }
        else
        {
            poolMing[prjtype].Pop();
        }
        gameObject.transform.SetPositionAndRotation(targetTr.position, targetTr.rotation);
        gameObject.transform.Rotate(Random.Range(-randomDeg, randomDeg),
            Random.Range(-randomDeg, randomDeg), Random.Range(-randomDeg, randomDeg));
        gameObject.SetActive(true);
        return gameObject;
    }

    public void Create(ProjectileSO prjtype)
    {
        if (!poolMing.ContainsKey(prjtype))
        {
            poolMing[prjtype] = new Stack<GameObject>();
        }
    }

}
