
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
            UnityEngine.Debug.LogError("ParentA�܂���ParentB���ݒ肳��Ă��܂���I");
            return;
        }

        // ParentA���炷�ׂẴ}�X�^�[bottle���擾
        GameObject[] masterBottles = GetChildrenWithTag(parentA, "bottle");
        // ParentB���炷�ׂẴT�ubottle���擾
        GameObject[] subBottles = GetChildrenWithTag(parentB, "SubBottle");


        // �}�X�^�[bottle�ƃT�ubottle�̑Ή��t�����s��
        foreach (GameObject masterBottle in masterBottles)
        {
            foreach (GameObject subBottle in subBottles)
            {
                // ���O����v����ꍇ�ɑΉ��t�����s��
                if (masterBottle.name == subBottle.name.Replace("Sub", "Master"))
                {
                    masterToSubMapping[masterBottle] = subBottle;

                    // �T�ubottle�̌��̐F��ۑ�
                    Renderer subRenderer = subBottle.GetComponent<Renderer>();
                    if (subRenderer != null)
                    {
                        originalColors[subBottle] = subRenderer.material.color;
                    }
                    break; // ��v�����玟�̃}�X�^�[bottle��
                }
            }
        }
 

  /*      // �e�}�X�^�[bottle�̐F��ۑ�
        foreach (GameObject masterBottle in masterToSubMapping.Keys)
        {
            Renderer masterRenderer = masterBottle.GetComponent<Renderer>();
            if (masterRenderer != null)
            {
                masterColors[masterBottle] = masterRenderer.material.color;
            }
        }*/

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

        /*        // �}�X�^�[�̐F���T�u�ɔ��f
                if (masterRenderer != null && subRenderer != null)
                {
                    Color masterColor = masterRenderer.material.color;

                    // �T�u�̐F���}�X�^�[�Ɠ���
                    subRenderer.material.color = masterColor;

                    // �T�ubottle���A�E�g���C�������ꍇ�A�A�E�g���C���̐F������
                    Outline masterOutline = masterBottle.GetComponent<Outline>();
                    Outline subOutline = subBottle.GetComponent<Outline>();

                    if (masterOutline != null && subOutline != null)
                    {
                        subOutline.OutlineColor = masterOutline.OutlineColor;
                        subOutline.OutlineWidth = masterOutline.OutlineWidth;
                    }
                }*/

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
    private GameObject[] GetChildrenWithTag(Transform parent, string tag)
    {
        List<GameObject> result = new List<GameObject>();
        foreach (Transform child in parent)
        {
            if (child.CompareTag(tag))
            {
                result.Add(child.gameObject);
            }
        }
        return result.ToArray();
    }
}

