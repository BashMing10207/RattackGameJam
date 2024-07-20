using System.Collections;
using System.Collections.Generic;
using TMPro;
//using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class TextCopy : MonoBehaviour
{
    public TextMeshProUGUI txt;
    public GameObject successMessage;

    private void Start()
    {
    }
    public void CopyTxt()
    {
        //EditorGUIUtility.systemCopyBuffer = txt.text;
        GUIUtility.systemCopyBuffer = txt.text;
        successMessage.SetActive(true);
    }
}
