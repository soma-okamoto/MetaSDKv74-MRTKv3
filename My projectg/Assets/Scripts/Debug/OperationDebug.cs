using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class OperationDebug : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject targetObject; // 対象のオブジェクト

    public void ToggleActive()
    {
        if (targetObject != null)
        {
            // 現在の状態を反転
            targetObject.SetActive(!targetObject.activeSelf);
            
        }
    }
    public void Active()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(true);
            UnityEngine.Debug.Log("ON");
        }
    }

    public void False()
    {
        if (targetObject != null)
        {
            targetObject.SetActive(false); //
            UnityEngine.Debug.Log("OFF");
        }
    }
}
