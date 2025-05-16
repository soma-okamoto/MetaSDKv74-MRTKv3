using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleColorChanger : MonoBehaviour
{
    [SerializeField] private GameObject targetObject; // Stage�I�u�W�F�N�g
    private Renderer targetRenderer;
    private Material targetMaterial;

    void Start()
    {
        // Stage�I�u�W�F�N�g��Renderer���擾
        targetRenderer = targetObject.GetComponent<Renderer>();
        targetMaterial = targetRenderer.material;
    }

    void Update()
    {
        // Bottle�^�O�����I�u�W�F�N�g�itargetObject�j�Ƃ̋������v�Z
        GameObject[] bottles = GameObject.FindGameObjectsWithTag("bottle");

        foreach (GameObject bottle in bottles)
        {
            // Bottle�̈ʒu���^�[�Q�b�g�ʒu�Ƃ��ăV�F�[�_�[�ɓn��
            targetMaterial.SetVector("_TargetPosition", bottle.transform.position);
        }
    }
}
