using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ObjectDuplicator : MonoBehaviour
{
    [Tooltip("�������������̃I�u�W�F�N�g")]
    public GameObject objectToDuplicate;

    [Tooltip("�������Ɉʒu���ǂꂾ�����炷��")]
    public Vector3 offset = new Vector3(1, 0, 0);

    [Tooltip("�������ɂǂꂾ����]�����邩 (Euler �p�x)")]
    public Vector3 rotationEuler = Vector3.zero;

    [Tooltip("�������̃X�P�[��")]
    public Vector3 scale = Vector3.one;

    public void DuplicateObject()
    {
        if (objectToDuplicate != null)
        {
            // Rotation�͌��̉�] + �w���]
            Quaternion newRotation = objectToDuplicate.transform.rotation * Quaternion.Euler(rotationEuler);

            // �������ĐV�����ʒu�E��]�Ő���
            GameObject duplicatedObject = Instantiate(
                objectToDuplicate,
                objectToDuplicate.transform.position + offset,
                newRotation
            );

            duplicatedObject.name = objectToDuplicate.name + "_Copy";

            // Scale�͎w�肵�����̂�K�p�i���̃X�P�[�����g�������Ȃ� objectToDuplicate.transform.localScale �ɕύX�j
            duplicatedObject.transform.localScale = scale;

            // PointCloudRenderer �� subscriber �̐ݒ���R�s�[
            var originalRenderer = objectToDuplicate.GetComponentInChildren<PointCloudRenderer>();
            var originalSubscriber = originalRenderer?.subscriber;

            var duplicatedRenderer = duplicatedObject.GetComponentInChildren<PointCloudRenderer>();
            if (duplicatedRenderer != null && originalSubscriber != null)
            {
                duplicatedRenderer.subscriber = originalSubscriber;
            }
            else
            {
                UnityEngine.Debug.LogWarning("Renderer��subscriber��������܂���ł���");
            }
        }
        else
        {
            UnityEngine.Debug.LogWarning("objectToDuplicate���ݒ肳��Ă��܂���I");
        }
    }
}
