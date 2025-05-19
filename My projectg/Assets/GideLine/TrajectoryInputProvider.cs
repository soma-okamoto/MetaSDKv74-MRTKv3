using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 軌道データを供給する共通インタフェース（ROS版でもこの形で供給）
/// </summary>
public class TrajectoryInputProvider : MonoBehaviour
{
    // 仮デバッグ用の制御点
    [SerializeField]
    private List<Vector3> debugTrajectory = new List<Vector3>()
    {
        new Vector3(0, 0.1f, 0.12f),
        new Vector3(0.03f, 0.07f, 0.1f),
        new Vector3(0.06f, 0.05f, 0.15f),
        new Vector3(0.09f, 0.02f, 0.2f),
        new Vector3(0.07f, 0, 0.25f)
    };

    // 公開：他のスクリプトが取得
    public List<Vector3> GetTrajectoryPoints()
    {
        return debugTrajectory;
    }

    // ROSとの接続時にはこの関数を書き換えるだけ
    public void UpdateTrajectoryFromROS(List<Vector3> newPoints)
    {
        debugTrajectory = newPoints;
    }
}
