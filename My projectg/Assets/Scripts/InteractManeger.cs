

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractManeger : MonoBehaviour
{
    public GameObject currentDrawer; // ���ݑI������Ă���I�u�W�F�N�g
    public OutlineOnView _rayManager; // ���C�L���X�g�}�l�[�W���[
    private GameObject raycastHit;

    // ���C���[��؂�ւ��郁�\�b�h
    void switchLayer(GameObject obj, string layerName)
    {
        obj.layer = LayerMask.NameToLayer(layerName);
    }

    // ���݂̃I�u�W�F�N�g�����Z�b�g
    void resetCurrentDrawer()
    {
        if (currentDrawer != null)
        {
            // ���C���[�����ɖ߂�
            switchLayer(currentDrawer, "Default");

            // �A�E�g���C���𖳌���
            var outline = currentDrawer.GetComponent<UnityFx.Outline.OutlineBehaviour>();
            if (outline != null)
            {
                outline.enabled = false;
            }

            currentDrawer = null;
        }
    }

    // Start �͏���������
    void Start()
    {
    }

    // Update �͖��t���[���Ăяo�����
    void Update()
    {
        raycastHit = _rayManager.hitObject;

        // �{�g���܂��͊ʂɃq�b�g�����ꍇ
        if (raycastHit != null && (raycastHit.tag == "bottle" || raycastHit.tag == "can"))
        {
            resetCurrentDrawer();

            currentDrawer = raycastHit;
            switchLayer(currentDrawer, "bottole");

            // �A�E�g���C����L����
            var outline = currentDrawer.GetComponent<UnityFx.Outline.OutlineBehaviour>();
            if (outline != null)
            {
                outline.enabled = true;
            }

            // �T�u�{�g���̃A�E�g���C���𓯊�
            OutlineOnView outlineManager = FindObjectOfType<OutlineOnView>();
            if (outlineManager != null && outlineManager.masterToSubMapping.ContainsKey(currentDrawer))
            {
                var subBottle = outlineManager.masterToSubMapping[currentDrawer];
                var subOutline = subBottle.GetComponent<UnityFx.Outline.OutlineBehaviour>();
                if (subOutline != null)
                {
                    subOutline.enabled = true;
                }
            }
        }
        else
        {
            // �q�b�g���Ă��Ȃ��ꍇ
            resetCurrentDrawer();
        }
    }
}


