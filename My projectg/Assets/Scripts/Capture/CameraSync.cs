using UnityEngine;

public class CameraSync : MonoBehaviour
{
    public GameObject mainCamera;  // Main Camera ���C���X�y�N�^�ŃA�T�C��

    void LateUpdate()
    {
        // Main Camera �̈ʒu�Ɖ�]�𓯊�
        transform.position = mainCamera.transform.position;
        transform.rotation = mainCamera.transform.rotation;
    }
}
