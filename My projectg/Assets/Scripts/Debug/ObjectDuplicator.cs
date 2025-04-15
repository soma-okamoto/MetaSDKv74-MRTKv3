using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ObjectDuplicator : MonoBehaviour
{
    [Tooltip("複製したい元のオブジェクト")]
    public GameObject objectToDuplicate;

    [Tooltip("複製時に位置をどれだけずらすか")]
    public Vector3 offset = new Vector3(1, 0, 0);

    [Tooltip("複製時にどれだけ回転させるか (Euler 角度)")]
    public Vector3 rotationEuler = Vector3.zero;

    [Tooltip("複製時のスケール")]
    public Vector3 scale = Vector3.one;

    public void DuplicateObject()
    {
        if (objectToDuplicate != null)
        {
            // Rotationは元の回転 + 指定回転
            Quaternion newRotation = objectToDuplicate.transform.rotation * Quaternion.Euler(rotationEuler);

            // 複製して新しい位置・回転で生成
            GameObject duplicatedObject = Instantiate(
                objectToDuplicate,
                objectToDuplicate.transform.position + offset,
                newRotation
            );

            duplicatedObject.name = objectToDuplicate.name + "_Copy";

            // Scaleは指定したものを適用（元のスケールを使いたいなら objectToDuplicate.transform.localScale に変更可）
            duplicatedObject.transform.localScale = scale;

            // PointCloudRenderer の subscriber の設定をコピー
            var originalRenderer = objectToDuplicate.GetComponentInChildren<PointCloudRenderer>();
            var originalSubscriber = originalRenderer?.subscriber;

            var duplicatedRenderer = duplicatedObject.GetComponentInChildren<PointCloudRenderer>();
            if (duplicatedRenderer != null && originalSubscriber != null)
            {
                duplicatedRenderer.subscriber = originalSubscriber;
            }
            else
            {
                UnityEngine.Debug.LogWarning("Rendererかsubscriberが見つかりませんでした");
            }
        }
        else
        {
            UnityEngine.Debug.LogWarning("objectToDuplicateが設定されていません！");
        }
    }
}
