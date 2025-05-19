using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// �O���f�[�^���������鋤�ʃC���^�t�F�[�X�iROS�łł����̌`�ŋ����j
/// </summary>
public class TrajectoryInputProvider : MonoBehaviour
{
    // ���f�o�b�O�p�̐���_
    [SerializeField]
    private List<Vector3> debugTrajectory = new List<Vector3>()
    {
        new Vector3(0, 0.1f, 0.12f),
        new Vector3(0.03f, 0.07f, 0.1f),
        new Vector3(0.06f, 0.05f, 0.15f),
        new Vector3(0.09f, 0.02f, 0.2f),
        new Vector3(0.07f, 0, 0.25f)
    };

    // ���J�F���̃X�N���v�g���擾
    public List<Vector3> GetTrajectoryPoints()
    {
        return debugTrajectory;
    }

    // ROS�Ƃ̐ڑ����ɂ͂��̊֐������������邾��
    public void UpdateTrajectoryFromROS(List<Vector3> newPoints)
    {
        debugTrajectory = newPoints;
    }
}
