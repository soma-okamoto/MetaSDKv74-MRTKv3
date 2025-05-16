
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
            UnityEngine.Debug.Log("ParentAまたはParentBが設定されていません！");
            return;
        }

        List<GameObject> masterList = GetOrderedChildrenWithTag(parentA, "bottle");
        List<GameObject> subList = GetOrderedChildrenWithTag(parentB, "SubBottle");


        

    }

    public void SetParentB(Transform newParentB)
    {
        parentB = newParentB;

        // 並び順に従って取得
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
        // マスターbottleの色やアウトラインをサブbottleに反映
        foreach (var entry in masterToSubMapping)
        {
            GameObject masterBottle = entry.Key;
            GameObject subBottle = entry.Value;

            if (masterBottle != null && subBottle != null)
            {
                Renderer masterRenderer = masterBottle.GetComponent<Renderer>();
                Renderer subRenderer = subBottle.GetComponent<Renderer>();

        

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

        // Hierarchy 上の順番は Transform 順（for ループ通り）なのでソート不要
        return result;
    }


}

