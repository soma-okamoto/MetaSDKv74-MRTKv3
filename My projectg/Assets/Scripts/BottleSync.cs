
using UnityEngine;
using System.Collections.Generic;

public class BottleSync : MonoBehaviour
{
    public Transform parentA; // �}�X�^�[bottle�̐e�I�u�W�F�N�g (ParentA)
    public Transform parentB; // �T�ubottle�̐e�I�u�W�F�N�g (ParentB)

    private Dictionary<GameObject, GameObject> masterToSubMapping = new Dictionary<GameObject, GameObject>();
    private Dictionary<GameObject, Color> masterColors = new Dictionary<GameObject, Color>();
    private Dictionary<GameObject, Color> originalColors = new Dictionary<GameObject, Color>();

    void Start()
    {
        if (parentA == null || parentB == null)
        {
            UnityEngine.Debug.Log("ParentA�܂���ParentB���ݒ肳��Ă��܂���I");
            return;
        }

        List<GameObject> masterList = GetOrderedChildrenWithTag(parentA, "bottle");
        List<GameObject> subList = GetOrderedChildrenWithTag(parentB, "SubBottle");


        

    }

    public void SetParentB(Transform newParentB)
    {
        parentB = newParentB;

        // ���я��ɏ]���Ď擾
        List<GameObject> masterList = GetOrderedChildrenWithTag(parentA, "bottle");
        List<GameObject> subList = GetOrderedChildrenWithTag(parentB, "SubBottle");

        masterToSubMapping.Clear();
        originalColors.Clear();

        int pairCount = Mathf.Min(masterList.Count, subList.Count);
        for (int i = 0; i < pairCount; i++)
        {
            GameObject master = masterList[i];
            GameObject sub = subList[i];

            masterToSubMapping[master] = sub;

            Renderer subRenderer = sub.GetComponent<Renderer>();
            if (subRenderer != null)
            {
                originalColors[sub] = subRenderer.material.color;
            }
        }
    }




    void Update()
    {
        // �}�X�^�[bottle�̐F��A�E�g���C�����T�ubottle�ɔ��f
        foreach (var entry in masterToSubMapping)
        {
            GameObject masterBottle = entry.Key;
            GameObject subBottle = entry.Value;

            if (masterBottle != null && subBottle != null)
            {
                Renderer masterRenderer = masterBottle.GetComponent<Renderer>();
                Renderer subRenderer = subBottle.GetComponent<Renderer>();

        

                // �}�X�^�[�̈ʒu�Ɖ�]���T�u�ɔ��f
                Transform masterParent = masterBottle.transform.parent;
                Vector3 masterLocalPosition = masterParent.InverseTransformPoint(masterBottle.transform.position);
                Quaternion masterLocalRotation = Quaternion.Inverse(masterParent.rotation) * masterBottle.transform.rotation;

                // �T�u�̈ʒu�Ɖ�]�𓯊�
                subBottle.transform.localPosition = masterLocalPosition;
                subBottle.transform.localRotation = masterLocalRotation;
            }
        }
    }


    // �w�肳�ꂽ�e�I�u�W�F�N�g�̎q�I�u�W�F�N�g�̒��������̃^�O�������̂��擾
    private List<GameObject> GetOrderedChildrenWithTag(Transform parent, string tag)
    {
        List<GameObject> result = new List<GameObject>();

        foreach (Transform child in parent)
        {
            if (child.CompareTag(tag))
            {
                result.Add(child.gameObject);
            }
        }

        // Hierarchy ��̏��Ԃ� Transform ���ifor ���[�v�ʂ�j�Ȃ̂Ń\�[�g�s�v
        return result;
    }


}

