using UnityEngine;
using TMPro;

public class PointCloudDistanceChecker : MonoBehaviour
{
    public PointCloudRenderer pointCloudRenderer;
    public OutlineOnView outlineOnView;
    
    public TextMeshPro distanceText;

    public float checkInterval = 0.2f;
    private float timer = 0f;

    private int previousClosestIndex = -1; // ← 前回赤くしたインデックスを記録

    public GameObject linePrefab;  // ← Prefab をアサイン

    private LineRenderer lineRenderer;

    void Start()
    {
        if (lineRenderer == null && linePrefab != null)
        {
            GameObject lineObj = Instantiate(linePrefab);
            lineRenderer = lineObj.GetComponent<LineRenderer>();
        }
    }


    void Update()
    {
       

        timer += Time.deltaTime;
        if (timer < checkInterval) return;
        timer = 0f;

        var targetUI = outlineOnView?.GetTargetUI();
        if (targetUI == null || !targetUI.activeSelf)
        {
            if (lineRenderer != null) lineRenderer.enabled = false;
            if (distanceText != null) distanceText.text = "";
            ResetPreviousPointColor(); // ← UI非表示時に赤を戻す
            return;
        }

        GameObject target = outlineOnView.hitObject;
        if (target == null || !target.CompareTag("bottle"))
        {
            if (lineRenderer != null) lineRenderer.enabled = false;
            if (distanceText != null) distanceText.text = "";
            ResetPreviousPointColor();
            return;
        }

        Vector3[] points = pointCloudRenderer?.GetPointCloud();
        if (points == null || points.Length == 0) return;

        Vector3 targetPos = target.transform.position;

        float minDistSqr = float.MaxValue;
        Vector3 closestPoint = Vector3.zero;

        foreach (var p in points)
        {
            float d = (p - targetPos).sqrMagnitude;
            if (d < minDistSqr)
            {
                minDistSqr = d;
                closestPoint = p;
            }
        }

        float distance = Mathf.Sqrt(minDistSqr);
        float distanceCm = distance * 100f;

        if (distanceText != null)
        {
            if (distanceCm <= 20f)
            {
                distanceText.text = $"Dist: {distanceCm:F1} cm";

                // 白→赤のグラデーション（25cmで白, 0cmで赤）
                float t = Mathf.Clamp01(1.0f - (distance / 0.20f));
                Color gradColor = Color.Lerp(Color.white, Color.red, t);
                distanceText.color = gradColor;
            }
            else
            {
                distanceText.text = "";
            }
        }



        Vector3 closestWorldPoint = pointCloudRenderer.transform.TransformPoint(closestPoint);

        if (lineRenderer != null)
        {
            if (distanceCm <= 20f)                  
            {
                lineRenderer.enabled = true;
                lineRenderer.positionCount = 2;
                lineRenderer.SetPosition(0, targetPos);
                lineRenderer.SetPosition(1, closestWorldPoint);
            }
            else
            {
                lineRenderer.enabled = false;
            }
        }


        // 最近傍点のインデックス取得
        int closestIndex = pointCloudRenderer.GetClosestPointIndex(targetPos);
        if (closestIndex >= 0)
        {
            var colors = pointCloudRenderer.GetColors();

            // 前回の色を白に戻す
            if (previousClosestIndex >= 0 && previousClosestIndex < colors.Length)
            {
                colors[previousClosestIndex] = Color.white;
            }

            // グラデーション色の計算（距離20cm以内を赤く）
            float t = Mathf.Clamp01(1.0f - (distance / 0.20f));
            Color gradColor = Color.Lerp(Color.white, Color.red, t);

            // 点に色を適用
            colors[closestIndex] = gradColor;
            pointCloudRenderer.UpdateColors(colors);
            previousClosestIndex = closestIndex;

            // 線にも同じ色を適用
            if (lineRenderer != null)
            {
                lineRenderer.startColor = gradColor;
                lineRenderer.endColor = gradColor;
            }
        }

        //Debug.Log($"[Line] BottlePos: {targetPos:F3} → ClosestPoint: {closestWorldPoint:F3} (Distance: {distanceCm:F1} cm)");
        //Debug.DrawLine(targetPos, closestWorldPoint, Color.green, 1f); // Sceneビューで確認

    }

    // UIが非表示になったときに赤い点を元に戻す
    void ResetPreviousPointColor()
    {
        if (previousClosestIndex < 0) return;

        var colors = pointCloudRenderer.GetColors();
        if (colors != null && previousClosestIndex < colors.Length)
        {
            colors[previousClosestIndex] = Color.white;
            pointCloudRenderer.UpdateColors(colors);
        }

        previousClosestIndex = -1;
    }
}
