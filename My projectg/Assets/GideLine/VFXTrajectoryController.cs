using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class VFXTrajectoryController : MonoBehaviour
{
    public VisualEffect vfx;
    public List<Vector3> debugTrajectory;

    private GraphicsBuffer buffer;

    void Start()
    {
        UpdateTrajectory(debugTrajectory);
    }

    public void UpdateTrajectory(List<Vector3> trajectory)
    {
        if (vfx == null || trajectory == null || trajectory.Count < 2) return;

        // 安全にバッファをクリア
        if (buffer != null)
            buffer.Release();

        int count = trajectory.Count;
        buffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, count, sizeof(float) * 3);
        buffer.SetData(trajectory);

        vfx.SetInt("pointCount", count);
        vfx.SetGraphicsBuffer("pathBuffer", buffer);
    }

    void OnDestroy()
    {
        if (buffer != null)
            buffer.Release();
    }
}
