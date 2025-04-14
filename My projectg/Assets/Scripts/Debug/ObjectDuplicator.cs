using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDuplicator : MonoBehaviour
{
    [Tooltip("�������������̃I�u�W�F�N�g")]
    public GameObject objectToDuplicate;

    [Tooltip("�������Ɉʒu���ǂꂾ�����炷��")]
    public Vector3 offset = new Vector3(1, 0, 0);

    public void DuplicateObject()
    {
        if (objectToDuplicate != null)
        {
            // �������ĐV�����ʒu�ɒu��
            GameObject duplicatedObject = Instantiate(
                objectToDuplicate,
                objectToDuplicate.transform.position + offset,
                objectToDuplicate.transform.rotation
            );

            duplicatedObject.name = objectToDuplicate.name + "_Copy";

            // ���I�u�W�F�N�g�� PointCloudRenderer ���擾
            var originalRenderer = objectToDuplicate.GetComponentInChildren<PointCloudRenderer>();  // �q�I�u�W�F�N�g�� PointCloudRenderer �擾
            var originalSubscriber = originalRenderer?.subscriber;

            // ������̃I�u�W�F�N�g���� PointCloudRenderer ��T���ē��� subscriber ��n��
            var duplicatedRenderer = duplicatedObject.GetComponentInChildren<PointCloudRenderer>();  // �q�I�u�W�F�N�g�� PointCloudRenderer �擾
            if (duplicatedRenderer != null && originalSubscriber != null)
            {
                duplicatedRenderer.subscriber = originalSubscriber;
            }
            else
            {
                Debug.LogWarning("Renderer��subscriber��������܂���ł���");
            }
        }
        else
        {
            Debug.LogWarning("objectToDuplicate���ݒ肳��Ă��܂���I");
        }
    }
}
