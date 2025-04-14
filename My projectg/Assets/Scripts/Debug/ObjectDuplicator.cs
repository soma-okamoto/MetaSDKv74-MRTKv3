using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDuplicator : MonoBehaviour
{
    [Tooltip("複製したい元のオブジェクト")]
    public GameObject objectToDuplicate;

    [Tooltip("複製時に位置をどれだけずらすか")]
    public Vector3 offset = new Vector3(1, 0, 0);

    public void DuplicateObject()
    {
        if (objectToDuplicate != null)
        {
            // 複製して新しい位置に置く
            GameObject duplicatedObject = Instantiate(
                objectToDuplicate,
                objectToDuplicate.transform.position + offset,
                objectToDuplicate.transform.rotation
            );

            duplicatedObject.name = objectToDuplicate.name + "_Copy";

            // 元オブジェクトの PointCloudRenderer を取得
            var originalRenderer = objectToDuplicate.GetComponentInChildren<PointCloudRenderer>();  // 子オブジェクトの PointCloudRenderer 取得
            var originalSubscriber = originalRenderer?.subscriber;

            // 複製先のオブジェクト内で PointCloudRenderer を探して同じ subscriber を渡す
            var duplicatedRenderer = duplicatedObject.GetComponentInChildren<PointCloudRenderer>();  // 子オブジェクトの PointCloudRenderer 取得
            if (duplicatedRenderer != null && originalSubscriber != null)
            {
                duplicatedRenderer.subscriber = originalSubscriber;
            }
            else
            {
                Debug.LogWarning("Rendererかsubscriberが見つかりませんでした");
            }
        }
        else
        {
            Debug.LogWarning("objectToDuplicateが設定されていません！");
        }
    }
}
