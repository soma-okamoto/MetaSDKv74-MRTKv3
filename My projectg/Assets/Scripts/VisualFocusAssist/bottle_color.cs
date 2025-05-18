using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bottle_color : MonoBehaviour
{
    public OutlineOnView _rayManager;
    public BottleSync bottleSync;  // Inspector�Őݒ�

    [ColorUsage(false, true)] public Color PickColor;       // �I��
    [ColorUsage(false, true)] public Color OtherColor;      // ��I���i�����x����j
    [ColorUsage(false, true)] public Color OriginalColor;   // �I���Ȃ�

    public float fadeSpeed = 15f;            // �t�F�[�h���x�i�傫���قǑ����j

    private Dictionary<GameObject, Color> originalColors = new Dictionary<GameObject, Color>();
    private Dictionary<GameObject, Material> materials = new Dictionary<GameObject, Material>();
    private Dictionary<GameObject, List<GameObject>> masterToSubMapping = new Dictionary<GameObject, List<GameObject>>();


    private GameObject[] bottles;
    // �������ǂ������L�^���Ă����āA�ς�����������؂�ւ���
    private Dictionary<GameObject, bool> isTransparent = new Dictionary<GameObject, bool>();

    void Start()
    {
        bottles = GameObject.FindGameObjectsWithTag("bottle");

        // BottleSync����}�b�s���O���󂯎��
        var syncMapping = bottleSync.GetMasterToSubMapping();
        foreach (var entry in syncMapping)
        {
            GameObject master = entry.Key;
            GameObject sub = entry.Value;

            // master
            var masterRenderer = master.GetComponent<Renderer>();
            if (masterRenderer != null)
            {
                Material mat = new Material(masterRenderer.material); // �ʉ�
                SetMaterialTransparent(mat);
                masterRenderer.material = mat;

                materials[master] = mat;
                originalColors[master] = mat.GetColor("_BaseColor");
                isTransparent[master] = false;
            }

            // sub
            var subRenderer = sub.GetComponent<Renderer>();
            if (subRenderer != null)
            {
                Material subMat = new Material(subRenderer.material);
                SetMaterialTransparent(subMat);
                subRenderer.material = subMat;
            }

            if (!masterToSubMapping.ContainsKey(master))
                masterToSubMapping[master] = new List<GameObject>();

            masterToSubMapping[master].Add(sub);
        }
    }





    void Update()
    {
        GameObject hit = _rayManager.hitObject;

        // �}�X�^�[-�T�u�̑Ή����X�g���L���ȃ{�g���̂ݑΏ�
        foreach (var bottle in bottles)
        {
            if (!materials.ContainsKey(bottle)) continue;

            // --- �F������ ---
            Color targetColor;
            bool shouldBeTransparent;

            if (hit == null)
            {
                targetColor = originalColors[bottle];
                shouldBeTransparent = false;
            }
            else if (hit == bottle)
            {
                // �N���b�N���ꂽ�}�X�^�[���������g�̂Ƃ�
                targetColor = PickColor;
                shouldBeTransparent = false;
            }
            else
            {
                targetColor = OtherColor;
                shouldBeTransparent = true;
            }

            // --- �}�X�^�[�̐F���X�V ---
            Material mat = materials[bottle];
            if (!isTransparent.ContainsKey(bottle) || isTransparent[bottle] != shouldBeTransparent)
            {
                if (shouldBeTransparent)
                    SetMaterialTransparent(mat);
                else
                    SetMaterialOpaque(mat);

                isTransparent[bottle] = shouldBeTransparent;
            }

            Color currentColor = mat.GetColor("_BaseColor");
            Color newColor = Color.Lerp(currentColor, targetColor, Time.deltaTime * fadeSpeed);
            mat.SetColor("_BaseColor", newColor);

            // --- �Ή�����T�u�{�g���ɂ̂ݐF���f ---
            if (masterToSubMapping.TryGetValue(bottle, out List<GameObject> subList))
            {
                foreach (var subBottle in subList)
                {
                    if (subBottle == null) continue;
                    var subRenderer = subBottle.GetComponent<Renderer>();
                    if (subRenderer != null)
                    {
                        subRenderer.material.color = newColor;

                        if (shouldBeTransparent)
                            SetMaterialTransparent(subRenderer.material);
                        else
                            SetMaterialOpaque(subRenderer.material);
                    }
                }
            }
        }
    }




    // �}�e���A���𓧖��ɑΉ�������
    void SetMaterialTransparent(Material mat)
    {
        mat.SetFloat("_Surface", 1); // Transparent
        mat.SetOverrideTag("RenderType", "Transparent");
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.EnableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
    }

    void SetMaterialOpaque(Material mat)
    {
        mat.SetFloat("_Surface", 0); // Opaque
        mat.SetOverrideTag("RenderType", "Opaque");
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
        mat.SetInt("_ZWrite", 1);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.DisableKeyword("_ALPHABLEND_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Geometry;
    }

}
