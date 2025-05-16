using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottleColorChanger : MonoBehaviour
{
    [SerializeField] private GameObject targetObject; // Stageオブジェクト
    private Renderer targetRenderer;
    private Material targetMaterial;

    void Start()
    {
        // StageオブジェクトのRendererを取得
        targetRenderer = targetObject.GetComponent<Renderer>();
        targetMaterial = targetRenderer.material;
    }

    void Update()
    {
        // Bottleタグを持つオブジェクト（targetObject）との距離を計算
        GameObject[] bottles = GameObject.FindGameObjectsWithTag("bottle");

        foreach (GameObject bottle in bottles)
        {
            // Bottleの位置をターゲット位置としてシェーダーに渡す
            targetMaterial.SetVector("_TargetPosition", bottle.transform.position);
        }
    }
}
