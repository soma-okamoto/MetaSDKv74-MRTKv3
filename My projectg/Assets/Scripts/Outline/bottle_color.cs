using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bottle_color : MonoBehaviour
{
    public OutlineOnView _rayManager;

    [ColorUsage(false, true)] public Color PickColor;       // 選択中
    [ColorUsage(false, true)] public Color OtherColor;      // 非選択（透明度あり）
    [ColorUsage(false, true)] public Color OriginalColor;   // 選択なし

    public float fadeSpeed = 15f;            // フェード速度（大きいほど速い）

    private Dictionary<GameObject, Color> originalColors = new Dictionary<GameObject, Color>();
    private Dictionary<GameObject, Material> materials = new Dictionary<GameObject, Material>();
    private GameObject[] bottles;
    // 透明かどうかを記録しておいて、変わった時だけ切り替える
    private Dictionary<GameObject, bool> isTransparent = new Dictionary<GameObject, bool>();

    void Start()
    {
        bottles = GameObject.FindGameObjectsWithTag("bottle");

        foreach (var bottle in bottles)
        {
            var renderer = bottle.GetComponent<Renderer>();
            if (renderer != null)
            {
                Material mat = new Material(renderer.material); // 個別化
                SetMaterialTransparent(mat);                    // 半透明対応（デフォルトでは透明にしておく）
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

            // 状態が変わったときだけ描画モードを変更
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



    // マテリアルを透明に対応させる
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

    // Inspector から設定できるように（HDR対応）
    [ColorUsage(false, true)] public Color PickColor;         // 対象の色
    [ColorUsage(false, true)] public Color OtherColor ;       // 対象外の色
    [ColorUsage(false, true)] public Color OriginalColor;   // hitObjectがないとき

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
                // マテリアルをインスタンス化（共有マテリアルを汚さないように）
                renderer.material = new Material(renderer.material);

                // 初期色を保存（MaterialのBaseColorを取得）
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
                    // 何も選択されていないとき → オリジナル色
                    targetColor = originalColors[bottle];
                }
                else if (bottle == hit)
                {
                    // 選択中のボトル → PickColor
                    targetColor = PickColor;
                }
                else
                {
                    // 選択されていない他のボトル → OtherColor
                    targetColor = OtherColor;
                }

                renderer.material.SetColor("_BaseColor", targetColor);
            }
        }
    }
}
*/