using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bottle_color : MonoBehaviour
{
    public OutlineOnView _rayManager;

    [ColorUsage(false, true)] public Color PickColor;       // �I��
    [ColorUsage(false, true)] public Color OtherColor;      // ��I���i�����x����j
    [ColorUsage(false, true)] public Color OriginalColor;   // �I���Ȃ�

    public float fadeSpeed = 15f;            // �t�F�[�h���x�i�傫���قǑ����j

    private Dictionary<GameObject, Color> originalColors = new Dictionary<GameObject, Color>();
    private Dictionary<GameObject, Material> materials = new Dictionary<GameObject, Material>();
    private GameObject[] bottles;
    // �������ǂ������L�^���Ă����āA�ς�����������؂�ւ���
    private Dictionary<GameObject, bool> isTransparent = new Dictionary<GameObject, bool>();

    void Start()
    {
        bottles = GameObject.FindGameObjectsWithTag("bottle");

        foreach (var bottle in bottles)
        {
            var renderer = bottle.GetComponent<Renderer>();
            if (renderer != null)
            {
                Material mat = new Material(renderer.material); // �ʉ�
                SetMaterialTransparent(mat);                    // �������Ή��i�f�t�H���g�ł͓����ɂ��Ă����j
                renderer.material = mat;

                materials[bottle] = mat;
                originalColors[bottle] = mat.GetColor("_BaseColor");

               
                isTransparent[bottle] = false;
            }
        }
    }



    void Update()
    {
        GameObject hit = _rayManager.hitObject;

        foreach (var bottle in bottles)
        {
            if (!materials.ContainsKey(bottle)) continue;
            Material mat = materials[bottle];

            Color targetColor;
            bool shouldBeTransparent;

            if (hit == null)
            {
                targetColor = originalColors[bottle];
                shouldBeTransparent = false;
            }
            else if (bottle == hit)
            {
                targetColor = PickColor;
                shouldBeTransparent = false;
            }
            else
            {
                targetColor = OtherColor;
                shouldBeTransparent = true;
            }

            // ��Ԃ��ς�����Ƃ������`�惂�[�h��ύX
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

/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bottle_color : MonoBehaviour
{
    public OutlineOnView _rayManager;

    // Inspector ����ݒ�ł���悤�ɁiHDR�Ή��j
    [ColorUsage(false, true)] public Color PickColor;         // �Ώۂ̐F
    [ColorUsage(false, true)] public Color OtherColor ;       // �ΏۊO�̐F
    [ColorUsage(false, true)] public Color OriginalColor;   // hitObject���Ȃ��Ƃ�

    private Dictionary<GameObject, Color> originalColors = new Dictionary<GameObject, Color>();
    private GameObject[] bottles;

    void Start()
    {
        bottles = GameObject.FindGameObjectsWithTag("bottle");

        foreach (var bottle in bottles)
        {
            var renderer = bottle.GetComponent<Renderer>();
            if (renderer != null)
            {
                // �}�e���A�����C���X�^���X���i���L�}�e���A���������Ȃ��悤�Ɂj
                renderer.material = new Material(renderer.material);

                // �����F��ۑ��iMaterial��BaseColor���擾�j
                originalColors[bottle] = renderer.material.GetColor("_BaseColor");
            }
        }
    }

    void Update()
    {
        GameObject hit = _rayManager.hitObject;

        foreach (var bottle in bottles)
        {
            var renderer = bottle.GetComponent<Renderer>();
            if (renderer != null)
            {
                Color targetColor;

                if (hit == null)
                {
                    // �����I������Ă��Ȃ��Ƃ� �� �I���W�i���F
                    targetColor = originalColors[bottle];
                }
                else if (bottle == hit)
                {
                    // �I�𒆂̃{�g�� �� PickColor
                    targetColor = PickColor;
                }
                else
                {
                    // �I������Ă��Ȃ����̃{�g�� �� OtherColor
                    targetColor = OtherColor;
                }

                renderer.material.SetColor("_BaseColor", targetColor);
            }
        }
    }
}
*/