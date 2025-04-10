using System.Collections;
using System.Collections.Generic;
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
            Debug.Log("OperationUIを反転");
        }
    }
}
