using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ‹O“¹‚ğ‰Â‹‰»‚·‚éi«—ˆ‚Ì“ü—Í•ÏX‚É‚à_“î‚É‘Î‰j
/// </summary>
public class TrajectoryVisualizer : MonoBehaviour
{
    public TrajectoryInputProvider inputProvider;
    public GameObject particlePrefab;
    public float spacing = 0.2f;
    public float smoothness = 0.05f;

    private List<GameObject> particleInstances = new List<GameObject>();

    void Start()
    {
        UpdateTrajectory();
    }

    public void UpdateTrajectory()
    {
        ClearParticles();

        List<Vector3> rawPoints = inputProvider.GetTrajectoryPoints();
        List<Vector3> interpolated = InterpolateCatmullRom(rawPoints, smoothness);
        DrawParticles(interpolated, spacing);
    }

    private List<Vector3> InterpolateCatmullRom(List<Vector3> points, float step)
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
                result.Add(CatmullRom(p0, p1, p2, p3, t));
            }
        }
        return result;
    }

    private Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        return 0.5f * (
            2f * p1 +
            (-p0 + p2) * t +
            (2f * p0 - 5f * p1 + 4f * p2 - p3) * t * t +
            (-p0 + 3f * p1 - 3f * p2 + p3) * t * t * t
        );
    }

    private void DrawParticles(List<Vector3> points, float interval)
    {
        float distAccum = 0f;
        Vector3 last = points[0];
        particleInstances.Add(Instantiate(particlePrefab, last, Quaternion.identity, transform));

        for (int i = 1; i < points.Count; i++)
        {
            distAccum += Vector3.Distance(last, points[i]);
            if (distAccum >= interval)
            {
                particleInstances.Add(Instantiate(particlePrefab, points[i], Quaternion.identity, transform));
                distAccum = 0f;
            }
            last = points[i];
        }
    }

    public void ClearParticles()
    {
        foreach (var p in particleInstances)
            Destroy(p);
        particleInstances.Clear();
    }
}
