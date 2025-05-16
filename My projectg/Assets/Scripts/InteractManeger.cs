

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractManeger : MonoBehaviour
{
    public GameObject currentDrawer; // 現在選択されているオブジェクト
    public OutlineOnView _rayManager; // レイキャストマネージャー
    private GameObject raycastHit;

    // レイヤーを切り替えるメソッド
    void switchLayer(GameObject obj, string layerName)
    {
        obj.layer = LayerMask.NameToLayer(layerName);
    }

    // 現在のオブジェクトをリセット
    void resetCurrentDrawer()
    {
        if (currentDrawer != null)
        {
            // レイヤーを元に戻す
            switchLayer(currentDrawer, "Default");

            // アウトラインを無効化
            var outline = currentDrawer.GetComponent<UnityFx.Outline.OutlineBehaviour>();
            if (outline != null)
            {
                outline.enabled = false;
            }

            currentDrawer = null;
        }
    }

    // Start は初期化処理
    void Start()
    {
    }

    // Update は毎フレーム呼び出される
    void Update()
    {
        raycastHit = _rayManager.hitObject;

        // ボトルまたは缶にヒットした場合
        if (raycastHit != null && (raycastHit.tag == "bottle" || raycastHit.tag == "can"))
        {
            resetCurrentDrawer();

            currentDrawer = raycastHit;
            switchLayer(currentDrawer, "bottole");

            // アウトラインを有効化
            var outline = currentDrawer.GetComponent<UnityFx.Outline.OutlineBehaviour>();
            if (outline != null)
            {
                outline.enabled = true;
            }

            // サブボトルのアウトラインを同期
            OutlineOnView outlineManager = FindObjectOfType<OutlineOnView>();
            if (outlineManager != null && outlineManager.masterToSubMapping.ContainsKey(currentDrawer))
            {
                var subBottle = outlineManager.masterToSubMapping[currentDrawer];
                var subOutline = subBottle.GetComponent<UnityFx.Outline.OutlineBehaviour>();
                if (subOutline != null)
                {
                    subOutline.enabled = true;
                }
            }
        }
        else
        {
            // ヒットしていない場合
            resetCurrentDrawer();
        }
    }
}


