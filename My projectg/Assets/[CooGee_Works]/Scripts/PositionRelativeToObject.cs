using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionRelativeToObject : MonoBehaviour
{
    public Transform targetObject; // �Q�ƃI�u�W�F�N�g (�z�u�̊�Ƃ���I�u�W�F�N�g)
    public GameObject objectToMove; // �ړ�������I�u�W�F�N�g�i��A�N�e�B�u���Ă��ʒu���擾�j
    private Animator objectToMoveAnimator;
    public string parameterName = "AppearObject"; // Animator�̃p�����[�^��

    private bool isObjectVisible = false; // ���݂̏�Ԃ�ێ�

    public Vector3 positionOffset; // �ʒu�̃I�t�Z�b�g�i�C���X�y�N�^�Œ����\�j
    public Vector3 rotationOffset; // ��]�̃I�t�Z�b�g�i�C���X�y�N�^�Œ����\�j

    private Vector3 targetPosition; // �I�t�Z�b�g�K�p��̃^�[�Q�b�g�ʒu��ۑ�
    private Quaternion targetRotation; // �I�t�Z�b�g�K�p��̃^�[�Q�b�g��]��ۑ�

    private void Start()
    {
        // ��I�u�W�F�N�g����̏����ʒu���擾
        if (targetObject != null)
        {
            UpdateObjectPosition();
        }

        if (objectToMove != null)
        {
            objectToMoveAnimator = objectToMove.GetComponent<Animator>();
        }
    }

    // �{�^�����N���b�N���ꂽ���ɌĂяo����郁�\�b�h
    public void PositionRelativeToTarget()
    {

        if (targetObject == null)
        {
            Debug.LogWarning("Target Object���ݒ肳��Ă��܂���I");
            return;
        }

        if (objectToMove == null)
        {
            Debug.LogWarning("Object to Move���ݒ肳��Ă��܂���I");
            return;
        }


        // �V�����ʒu�Ɖ�]���X�V����
        UpdateObjectPosition();

        objectToMove.transform.position = targetPosition;
        objectToMove.transform.rotation = targetRotation;

        // ���݂̏�Ԃ𔽓]
        isObjectVisible = !isObjectVisible;

        // Animator�̃p�����[�^���X�V
        objectToMoveAnimator.SetBool(parameterName, isObjectVisible);
    }

    private void UpdateObjectPosition()
    {
        // �Q�ƃI�u�W�F�N�g�̈ʒu�Ɖ�]���擾
        targetPosition = targetObject.position + targetObject.TransformDirection(positionOffset);
        targetRotation = targetObject.rotation * Quaternion.Euler(rotationOffset);

        // �������J�����ƍ��킹�邽�߂�Y���W���L�[�v����i�K�v�ɉ����āj
        targetPosition.y = targetObject.position.y;
    }
}
