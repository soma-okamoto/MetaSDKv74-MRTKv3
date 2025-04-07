using UnityEngine;

public class bottle : MonoBehaviour
{
    public GameObject bottlePrefab;  // �v���n�u�Q��
    public float spawnDistance = 0.5f;  // �炩��̋���

    public void SpawnBottle()
    {
        if (bottlePrefab == null)
        {
            Debug.LogWarning("�{�g���v���n�u���ݒ肳��Ă��܂���I");
            return;
        }

        // HMD�̃J�����ʒu�ƌ���
        Transform cameraTransform = Camera.main.transform;

        // ��̑O���ɐ����ʒu���v�Z
        Vector3 spawnPosition = cameraTransform.position + cameraTransform.forward * spawnDistance;

        // �J������Y����]�̂ݓK�p�i�����j
        Quaternion horizontalRotation = Quaternion.Euler(0, cameraTransform.eulerAngles.y, 0);

        // �����ɐ���
        Instantiate(bottlePrefab, spawnPosition, horizontalRotation);
        Debug.Log("spown");
    }
}
