using UnityEngine;

public class bottle : MonoBehaviour
{
    public GameObject bottlePrefab;  // �v���n�u�Q��
    public float spawnDistance = 0.5f;  // �炩��̋���
    public GameObject parentObject;  // �������ꂽ�I�u�W�F�N�g�̐e�I�u�W�F�N�g�i�q�G�����L�[���j

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
        GameObject spawnedBottle = Instantiate(bottlePrefab, spawnPosition, horizontalRotation);

        // �q�G�����L�[���ł̐e�I�u�W�F�N�g��ݒ�
        if (parentObject != null)
        {
            spawnedBottle.transform.SetParent(parentObject.transform);
            //Debug.Log("�������ꂽ�I�u�W�F�N�g���q�G�����L�[���Őe�ɐݒ肵�܂����B");
        }
        else
        {
            Debug.LogWarning("�e�I�u�W�F�N�g���ݒ肳��Ă��܂���I");
        }

        //Debug.Log("spown");
    }
}
