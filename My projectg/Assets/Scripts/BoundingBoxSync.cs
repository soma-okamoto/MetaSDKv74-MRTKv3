using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MixedReality.Toolkit.UX;
using MixedReality.Toolkit.SpatialManipulation;

public class BoundingBoxSync : MonoBehaviour
{
    [Header("MRTK3 Components")]
    public BoundsControl boundsControl;        // BoundingBox�I�u�W�F�N�g�ɕt���Ă���
    public Transform overrideTargetTransform;  // ��̃I�u�W�F�N�g

    [Header("Collider")]
    public BoxCollider sourceCollider;         // ���I�ɃT�C�Y���ς��R���C�_�[�i�Q�ƌ��j

    void Start()
    {
        if (boundsControl == null) boundsControl = GetComponent<BoundsControl>();

        // �I�[�o�[���C�h������Transform�ɐݒ�
        boundsControl.BoundsOverride = overrideTargetTransform;
    }

    void Update()
    {
        UpdateOverrideTransformFromCollider();

        // �����I�ɍĕ]��������
        boundsControl.BoundsOverride = null;
        boundsControl.BoundsOverride = overrideTargetTransform;
    }


    void UpdateOverrideTransformFromCollider()
    {
        // �R���C�_�[���S�����[���h���W�Ŏ擾���AOverride�I�u�W�F�N�g�̈ʒu�ɐݒ�
        Vector3 worldCenter = sourceCollider.transform.TransformPoint(sourceCollider.center);
        overrideTargetTransform.position = worldCenter;

        // �X�P�[�������[���h��Ԃɕϊ��i���[�J���X�P�[�� * �R���C�_�[�X�P�[���j
        Vector3 worldSize = Vector3.Scale(sourceCollider.size, sourceCollider.transform.lossyScale);
        overrideTargetTransform.localScale = worldSize;
    }
}
