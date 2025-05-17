using UnityEngine;
using TMPro;

public class PointCloudDistanceChecker : MonoBehaviour
{
    public PointCloudRenderer pointCloudRenderer;
    public OutlineOnView outlineOnView;
    
    public TextMeshPro distanceText;

    public float checkInterval = 0.2f;
    private float timer = 0f;

    private int previousClosestIndex = -1; // �� �O��Ԃ������C���f�b�N�X���L�^

    public GameObject linePrefab;  // �� Prefab ���A�T�C��

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
            ResetPreviousPointColor(); // �� UI��\�����ɐԂ�߂�
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

                // �����Ԃ̃O���f�[�V�����i25cm�Ŕ�, 0cm�Őԁj
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


        // �ŋߖT�_�̃C���f�b�N�X�擾
        int closestIndex = pointCloudRenderer.GetClosestPointIndex(targetPos);
        if (closestIndex >= 0)
        {
            var colors = pointCloudRenderer.GetColors();

            // �O��̐F�𔒂ɖ߂�
            if (previousClosestIndex >= 0 && previousClosestIndex < colors.Length)
            {
                colors[previousClosestIndex] = Color.white;
            }

            // �O���f�[�V�����F�̌v�Z�i����20cm�ȓ���Ԃ��j
            float t = Mathf.Clamp01(1.0f - (distance / 0.20f));
            Color gradColor = Color.Lerp(Color.white, Color.red, t);

            // �_�ɐF��K�p
            colors[closestIndex] = gradColor;
            pointCloudRenderer.UpdateColors(colors);
            previousClosestIndex = closestIndex;

            // ���ɂ������F��K�p
            if (lineRenderer != null)
            {
                lineRenderer.startColor = gradColor;
                lineRenderer.endColor = gradColor;
            }
        }

        //Debug.Log($"[Line] BottlePos: {targetPos:F3} �� ClosestPoint: {closestWorldPoint:F3} (Distance: {distanceCm:F1} cm)");
        //Debug.DrawLine(targetPos, closestWorldPoint, Color.green, 1f); // Scene�r���[�Ŋm�F

    }

    // UI����\���ɂȂ����Ƃ��ɐԂ��_�����ɖ߂�
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
