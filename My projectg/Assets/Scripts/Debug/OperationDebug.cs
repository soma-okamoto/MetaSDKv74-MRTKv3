using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperationDebug : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject targetObject; // �Ώۂ̃I�u�W�F�N�g

    public void ToggleActive()
    {
        if (targetObject != null)
        {
            // ���݂̏�Ԃ𔽓]
            targetObject.SetActive(!targetObject.activeSelf);
            Debug.Log("OperationUI�𔽓]");
        }
    }
}
