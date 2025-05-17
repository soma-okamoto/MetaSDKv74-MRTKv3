using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using RosSharp.RosBridgeClient;
using System.Diagnostics;
public class PointCloudRenderer : MonoBehaviour
{
    public PointCloudSubscriber subscriber;
    public MockPointCloudSubscriber mockSubscriber;  // MockPointCloudSubscriber �Q��

    Mesh mesh;
    MeshRenderer meshRenderer;
    MeshFilter mf;
    public Material _material;

    public bool useMockData = false;  // ���b�N�f�[�^���g�p���邩�ǂ���
    public int mockWidth;
    public int mockHeight;
    public float mockSpacing;

    // The size, positions and colours of each of the pointcloud
    public float pointSize = 1f;

    [SerializeField] private BoxCollider BoundingBox;

    private Vector3[] positions = new Vector3[] { new Vector3(0, 0, 0), new Vector3(0, 1, 0) };
    private Color[] colours = new Color[] { new Color(1f, 0f, 0f), new Color(0f, 1f, 0f) };
    private Vector2 size;

    public Transform offset; // VR�œ_�Q�̃I���W���𒲐����邽�߂̃I�u�W�F�N�g
    public Vector3 BBoxSize;
    public Vector3 BBoxPos;
    public bool StopGetPointCloud = false;

    void Start()
    {
        // ���S�ɕK�v�ȃR���|�[�l���g���擾�E����
        meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer == null)
            meshRenderer = gameObject.AddComponent<MeshRenderer>();

        mf = GetComponent<MeshFilter>();
        if (mf == null)
            mf = gameObject.AddComponent<MeshFilter>();

        // �}�e���A�����ݒ肳��Ă��邩�`�F�b�N
        if (_material != null)
        {
            meshRenderer.material = new Material(_material);
        }
        else
        {
            UnityEngine.Debug.LogWarning("Material not assigned to PointCloudRenderer");
        }

        mesh = new Mesh
        {
            indexFormat = UnityEngine.Rendering.IndexFormat.UInt32
        };

        // PointCloudSubscriber ���w�肳��Ă��Ȃ���Ύ����擾
        if (subscriber == null)
        {
            subscriber = GetComponent<PointCloudSubscriber>();
            if (subscriber == null)
            {
                UnityEngine.Debug.LogWarning("PointCloudSubscriber not found on this GameObject.");
            }
        }

        // MockPointCloudSubscriber �̎Q�Ɛݒ�
        if (mockSubscriber == null && useMockData)
        {
            mockSubscriber = GetComponent<MockPointCloudSubscriber>();
            if (mockSubscriber == null)
            {
                UnityEngine.Debug.LogWarning("MockPointCloudSubscriber not found on this GameObject.");
            }
        }

        if (BoundingBox == null)
        {
            BoundingBox = GetComponentInChildren<BoxCollider>();
            if (BoundingBox == null)
            {
                UnityEngine.Debug.LogWarning("BoundingBox (BoxCollider) not found in children.");
            }
        }
    }

    IEnumerator UpdateMesh()
    {
        // useMockData �� true �̏ꍇ�̓��b�N�f�[�^���g�p
        if (useMockData && mockSubscriber != null)
        {
            positions = mockSubscriber.GetPCL();
            colours = mockSubscriber.GetPCLColor();
            size = mockSubscriber.GetSize();
        }
        // useMockData �� false �̏ꍇ�͒ʏ�� PointCloudSubscriber �̃f�[�^���g�p
        else if (subscriber != null)
        {
            positions = subscriber.GetPCL();
            colours = subscriber.GetPCLColor();
            size = subscriber.GetSize();
        }
        else
        {
            yield break;
        }

        if (positions == null)
        {
            yield return null;
        }

        mesh.Clear();
        mesh.vertices = positions;
        mesh.colors = colours;

        if (positions != null)
        {
            List<int> indices = new List<int>();
            for (int i = 0; i < positions.Length; i++)
            {
                indices.Add(i);
            }

            mesh.SetIndices(indices.ToArray(), MeshTopology.Points, 0);
            mf.mesh = mesh;

            Vector3 center = Vector3.zero;
            foreach (Vector3 position in positions)
            {
                center += position;
            }
            center /= positions.Length;

            // Bounding Box �̌v�Z
            float minx = float.MaxValue;
            float miny = float.MaxValue;
            float minz = float.MaxValue;

            float maxx = float.MinValue;
            float maxy = float.MinValue;
            float maxz = float.MinValue;

            foreach (Vector3 position in positions)
            {
                minx = Mathf.Min(minx, position.x);
                miny = Mathf.Min(miny, position.y);
                minz = Mathf.Min(minz, position.z);
                maxx = Mathf.Max(maxx, position.x);
                maxy = Mathf.Max(maxy, position.y);
                maxz = Mathf.Max(maxz, position.z);
            }

            BoundingBox.center = -center;
            BoundingBox.size = new Vector3(Mathf.Abs(minx) + Mathf.Abs(maxx) + 0.3f, Mathf.Abs(miny) + Mathf.Abs(maxy) + 0.3f, (Mathf.Abs(minz) + Mathf.Abs(maxz)) / 2 + 0.3f);
        }
    }

    void Update()
    {
        if (!StopGetPointCloud)
        {
            meshRenderer.material.SetFloat("_PointSize", pointSize);
            StartCoroutine("UpdateMesh");
            BBoxSize = BoundingBox.transform.localScale;
            BBoxPos = BoundingBox.transform.localPosition;
        }
    }
    public Vector3[] GetPointCloud()
    {
        return positions;
    }


    public Color[] GetColors()
    {
        return colours;
    }

    public void UpdateColors(Color[] newColors)
    {
        colours = newColors;
        if (mesh != null && colours != null)
        {
            mesh.colors = colours;
        }
    }

    public int GetClosestPointIndex(Vector3 targetPos)
    {
        if (positions == null || positions.Length == 0)
            return -1;

        float minDistSqr = float.MaxValue;
        int closestIndex = -1;

        for (int i = 0; i < positions.Length; i++)
        {
            float dist = (positions[i] - targetPos).sqrMagnitude;
            if (dist < minDistSqr)
            {
                minDistSqr = dist;
                closestIndex = i;
            }
        }

        return closestIndex;
    }


}

