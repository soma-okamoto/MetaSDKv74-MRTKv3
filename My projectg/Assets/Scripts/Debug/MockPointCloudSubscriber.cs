/*using UnityEngine;

public class MockPointCloudSubscriber : MonoBehaviour
{
    [Header("Mock Mode (Use for testing without ROS)")]
    public bool useMockData = false;

    [SerializeField] private int mockSize = 10;           // ���A�����A���s�������ʉ�
    [SerializeField] private float mockSpacing = 0.1f;

    private Vector3[] points;
    private Color[] colors;

    private MeshRenderer meshRenderer;
    private Material pointCloudMaterial;

    // Inspector �ŕύX���ꂽ��A�����Ƀ|�C���g�N���E�h�f�[�^���X�V
    private void OnValidate()
    {
        UpdatePointCloudData();
    }

    // �|�C���g�N���E�h�f�[�^�̍X�V
    private void UpdatePointCloudData()
    {
        int size = mockSize;
        float spacing = mockSpacing;

        // �n�ʗp�̓_�Q�쐬�iy = 0�j
        points = new Vector3[size * size];
        int index = 0;
        for (int z = 0; z < size; z++)
        {
            for (int x = 0; x < size; x++)
            {
                // �n�ʂ̍��W�� y = 0 �Ƃ��Ax, z ���W�ōL����
                points[index++] = new Vector3(x * spacing, 0, z * spacing);
            }
        }

        // �F�����ōX�V
        colors = new Color[points.Length];
        for (int i = 0; i < points.Length; i++)
        {
            colors[i] = Color.white;  // �O���f�[�V�����Ȃ��Ŕ��ɐݒ�
        }

        // �Ǘp�̓_�Q�쐬�i�����̖ʂ����j
        int wallIndex = points.Length;
        Vector3[] tempPoints = new Vector3[points.Length + size * size];
        Color[] tempColors = new Color[colors.Length + size * size];

        // �����̒n�ʃf�[�^���R�s�[
        points.CopyTo(tempPoints, 0);
        colors.CopyTo(tempColors, 0);

        // ��1�iZ�����̕ǁj
        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                tempPoints[wallIndex++] = new Vector3(x * spacing, y * spacing, size * spacing);  // z = size �̈ʒu�ɕǂ�z�u
                tempColors[wallIndex - 1] = Color.white; // �ǂ����ɐݒ�
            }
        }

        points = tempPoints;
        colors = tempColors;

        // �}�e���A����K�p����
        ApplyMaterial();
    }

    // �}�e���A���K�p
    private void ApplyMaterial()
    {
        // MeshRenderer���擾�܂��͐���
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer == null)
        {
            meshRenderer = gameObject.AddComponent<MeshRenderer>();
        }

        // �}�e���A����V�����쐬���A�J�X�^���V�F�[�_�[��ݒ�
        if (pointCloudMaterial == null)
        {
            pointCloudMaterial = new Material(Shader.Find("Unlit/VertexColor"));
        }

        // ���_�J���[�𔽉f���邽�߂Ƀ}�e���A���ɐݒ�
        meshRenderer.material = pointCloudMaterial;
    }

    // �F�𓮓I�ɕύX���郁�\�b�h
    public void UpdatePointColor(int index, Color newColor)
    {
        if (index >= 0 && index < colors.Length)
        {
            colors[index] = newColor;
            ApplyMaterial();  // �}�e���A�����X�V���ĕύX�𔽉f
        }
    }
    public Color GetPointColor(int index)
    {
        if (index >= 0 && index < colors.Length)
        {
            return colors[index];
        }
        else
        {
            Debug.LogWarning($"Invalid index: {index}");
            return Color.white; // �G���[���͔��F��Ԃ�
        }
    }


    public Vector3[] GetPCL()
    {
        return points;
    }

    public Color[] GetPCLColor()
    {
        return colors;
    }

    public Vector3 GetSize()
    {
        return new Vector3(mockSize, mockSize, mockSize);
    }
}
*/

using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class MockPointCloudSubscriber : MonoBehaviour
{
    private Vector3[] pcl;
    private Color[] pcl_color;
    private Vector2 size;

    public void LoadFromCSV(string filePath)
    {
        var pclList = new List<Vector3>();
        var colorList = new List<Color>();

        if (!File.Exists(filePath))
        {
            UnityEngine.Debug.LogError("CSV file not found at: " + filePath);
            return;
        }

        string[] lines = File.ReadAllLines(filePath);
        for (int i = 1; i < lines.Length; i++)  // Skip header
        {
            string[] tokens = lines[i].Split(',');
            if (tokens.Length >= 6 &&
                float.TryParse(tokens[0], out float x) &&
                float.TryParse(tokens[1], out float y) &&
                float.TryParse(tokens[2], out float z) &&
                int.TryParse(tokens[3], out int r) &&
                int.TryParse(tokens[4], out int g) &&
                int.TryParse(tokens[5], out int b))
            {
                pclList.Add(new Vector3(x, y, z));
                colorList.Add(new Color(r / 255f, g / 255f, b / 255f));
            }
        }

        pcl = pclList.ToArray();
        pcl_color = colorList.ToArray();
        size = new Vector2(Mathf.Sqrt(pcl.Length), Mathf.Sqrt(pcl.Length)); // ���̃T�C�Y����

        UnityEngine.Debug.Log("Loaded point cloud from CSV with " + pcl.Length + " points.");
    }

    public Vector3[] GetPCL() => pcl;
    public Color[] GetPCLColor() => pcl_color;
    public Vector2 GetSize() => size;
}
