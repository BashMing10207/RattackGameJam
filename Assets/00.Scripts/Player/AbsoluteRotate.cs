using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbsoluteRotate : MonoBehaviour
{
    void Update()
    {
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }
}
