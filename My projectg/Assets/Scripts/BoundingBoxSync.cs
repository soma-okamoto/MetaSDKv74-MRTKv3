using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MixedReality.Toolkit.UX;
using MixedReality.Toolkit.SpatialManipulation;

public class BoundingBoxSync : MonoBehaviour
{
    [Header("MRTK3 Components")]
    public BoundsControl boundsControl;        // BoundingBoxオブジェクトに付いている
    public Transform overrideTargetTransform;  // 空のオブジェクト

    [Header("Collider")]
    public BoxCollider sourceCollider;         // 動的にサイズが変わるコライダー（参照元）

    void Start()
    {
        if (boundsControl == null) boundsControl = GetComponent<BoundsControl>();

        // オーバーライド先を空のTransformに設定
        boundsControl.BoundsOverride = overrideTargetTransform;
    }

    void Update()
    {
        UpdateOverrideTransformFromCollider();

        // 強制的に再評価させる
        boundsControl.BoundsOverride = null;
        boundsControl.BoundsOverride = overrideTargetTransform;
    }


    void UpdateOverrideTransformFromCollider()
    {
        // コライダー中心をワールド座標で取得し、Overrideオブジェクトの位置に設定
        Vector3 worldCenter = sourceCollider.transform.TransformPoint(sourceCollider.center);
        overrideTargetTransform.position = worldCenter;

        // スケールをワールド空間に変換（ローカルスケール * コライダースケール）
        Vector3 worldSize = Vector3.Scale(sourceCollider.size, sourceCollider.transform.lossyScale);
        overrideTargetTransform.localScale = worldSize;
    }
}
