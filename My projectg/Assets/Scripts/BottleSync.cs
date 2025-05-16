
using UnityEngine;
using System.Collections.Generic;

public class BottleSync : MonoBehaviour
{
    public Transform parentA; // マスターbottleの親オブジェクト (ParentA)
    public Transform parentB; // サブbottleの親オブジェクト (ParentB)

    private Dictionary<GameObject, GameObject> masterToSubMapping = new Dictionary<GameObject, GameObject>();
    private Dictionary<GameObject, Color> masterColors = new Dictionary<GameObject, Color>();
    private Dictionary<GameObject, Color> originalColors = new Dictionary<GameObject, Color>();

    void Start()
    {
        if (parentA == null || parentB == null)
        {
            UnityEngine.Debug.LogError("ParentAまたはParentBが設定されていません！");
            return;
        }

        // ParentAからすべてのマスターbottleを取得
        GameObject[] masterBottles = GetChildrenWithTag(parentA, "bottle");
        // ParentBからすべてのサブbottleを取得
        GameObject[] subBottles = GetChildrenWithTag(parentB, "SubBottle");


        // マスターbottleとサブbottleの対応付けを行う
        foreach (GameObject masterBottle in masterBottles)
        {
            foreach (GameObject subBottle in subBottles)
            {
                // 名前が一致する場合に対応付けを行う
                if (masterBottle.name == subBottle.name.Replace("Sub", "Master"))
                {
                    masterToSubMapping[masterBottle] = subBottle;

                    // サブbottleの元の色を保存
                    Renderer subRenderer = subBottle.GetComponent<Renderer>();
                    if (subRenderer != null)
                    {
                        originalColors[subBottle] = subRenderer.material.color;
                    }
                    break; // 一致したら次のマスターbottleへ
                }
            }
        }
 

  /*      // 各マスターbottleの色を保存
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
        // マスターbottleの色やアウトラインをサブbottleに反映
        foreach (var entry in masterToSubMapping)
        {
            GameObject masterBottle = entry.Key;
            GameObject subBottle = entry.Value;

            if (masterBottle != null && subBottle != null)
            {
                Renderer masterRenderer = masterBottle.GetComponent<Renderer>();
                Renderer subRenderer = subBottle.GetComponent<Renderer>();

        /*        // マスターの色をサブに反映
                if (masterRenderer != null && subRenderer != null)
                {
                    Color masterColor = masterRenderer.material.color;

                    // サブの色をマスターと同期
                    subRenderer.material.color = masterColor;

                    // サブbottleがアウトラインを持つ場合、アウトラインの色も同期
                    Outline masterOutline = masterBottle.GetComponent<Outline>();
                    Outline subOutline = subBottle.GetComponent<Outline>();

                    if (masterOutline != null && subOutline != null)
                    {
                        subOutline.OutlineColor = masterOutline.OutlineColor;
                        subOutline.OutlineWidth = masterOutline.OutlineWidth;
                    }
                }*/

                // マスターの位置と回転をサブに反映
                Transform masterParent = masterBottle.transform.parent;
                Vector3 masterLocalPosition = masterParent.InverseTransformPoint(masterBottle.transform.position);
                Quaternion masterLocalRotation = Quaternion.Inverse(masterParent.rotation) * masterBottle.transform.rotation;

                // サブの位置と回転を同期
                subBottle.transform.localPosition = masterLocalPosition;
                subBottle.transform.localRotation = masterLocalRotation;
            }
        }
    }


    // 指定された親オブジェクトの子オブジェクトの中から特定のタグを持つものを取得
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

