using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugParticleTrajectory : MonoBehaviour
{
    public GameObject particlePrefab;  // è¨Ç≥Ç»åıãÖÇ»Ç«
    public float spacing = 0.2f;       // ó±ÇÃä‘äu
    public float smoothness = 0.05f;   // ï‚ä‘ÇÃç◊Ç©Ç≥

    private List<GameObject> particleInstances = new List<GameObject>();

    void Start()
    {
        List<Vector3> controlPoints = new List<Vector3>()
        {
             new Vector3(0, 0.1f, 0.12f),
        new Vector3(0.03f, 0.07f, 0.1f),
        new Vector3(0.06f, 0.05f, 0.15f),
        new Vector3(0.09f, 0.02f, 0.2f),
        new Vector3(0.07f, 0, 0.25f)
        };

        List<Vector3> interpolatedPoints = CatmullRomSpline(controlPoints, smoothness);
        DrawParticles(interpolatedPoints, spacing);
    }

    List<Vector3> CatmullRomSpline(List<Vector3> points, float step)
    {
        List<Vector3> result = new List<Vector3>();
        for (int i = 0; i < points.Count - 1; i++)
        {
            Vector3 p0 = points[Mathf.Max(i - 1, 0)];
            Vector3 p1 = points[i];
            Vector3 p2 = points[i + 1];
            Vector3 p3 = points[Mathf.Min(i + 2, points.Count - 1)];

            for (float t = 0; t < 1f; t += step)
            {
                Vector3 pt = CatmullRom(p0, p1, p2, p3, t);
                result.Add(pt);
            }
        }
        return result;
    }

    Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        return 0.5f * (
            2f * p1 +
            (-p0 + p2) * t +
            (2f * p0 - 5f * p1 + 4f * p2 - p3) * t * t +
            (-p0 + 3f * p1 - 3f * p2 + p3) * t * t * t
        );
    }

    void DrawParticles(List<Vector3> pathPoints, float interval)
    {
        float distAccum = 0f;
        Vector3 lastPos = pathPoints[0];
        particleInstances.Add(Instantiate(particlePrefab, lastPos, Quaternion.identity, transform));

        for (int i = 1; i < pathPoints.Count; i++)
        {
            float dist = Vector3.Distance(lastPos, pathPoints[i]);
            distAccum += dist;
            if (distAccum >= interval)
            {
                particleInstances.Add(Instantiate(particlePrefab, pathPoints[i], Quaternion.identity, transform));
                distAccum = 0f;
            }
            lastPos = pathPoints[i];
        }
    }

    public void ClearParticles()
    {
        foreach (var p in particleInstances)
            Destroy(p);
        particleInstances.Clear();
    }
}
