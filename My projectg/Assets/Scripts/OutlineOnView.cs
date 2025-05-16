



using System.Collections.Generic;
using UnityEngine;
using UnityFx.Outline;

public class OutlineOnView : MonoBehaviour
{
    public Camera playerCamera;
    private float maxRaycastDistance = 0.5f;
    private Ray ray;
    private RaycastHit hitObj;
    public GameObject hitObject;
    private GameObject previousHitObject;

    public Vector3 bottlePosition = Vector3.zero;

    private GameObject[] masterBottles;
    private Dictionary<GameObject, Color> originalColors = new Dictionary<GameObject, Color>(); // ボトルの元の色を保持
    public Dictionary<GameObject, GameObject> masterToSubMapping = new Dictionary<GameObject, GameObject>(); // マスターとサブの対応関係

    void Start()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }

        // マスターボトルを取得
        masterBottles = GameObject.FindGameObjectsWithTag("bottle");
        foreach (var masterBottle in masterBottles)
        {
            var renderer = masterBottle.GetComponent<Renderer>();
            if (renderer != null)
            {
                //originalColors[masterBottle] = renderer.material.color;
                renderer.material = new Material(renderer.material); // インスタンス化
                originalColors[masterBottle] = renderer.material.GetColor("_BaseColor");
            }

            // 対応するサブボトルを探す
            GameObject subBottle = GameObject.Find(masterBottle.name.Replace("Master", "Sub"));
            if (subBottle != null)
            {
                masterToSubMapping[masterBottle] = subBottle;
            }
        }
    }

    void Update()
    {
        ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out hitObj, maxRaycastDistance))
        {
            hitObject = hitObj.collider.gameObject;

            if (hitObject != previousHitObject)
            {
                // 前のオブジェクトのアウトラインを無効化
                if (previousHitObject != null)
                {
                    ToggleOutline(previousHitObject, false);
                }

                // 現在のオブジェクトのアウトラインを有効化
                ToggleOutline(hitObject, true);

                previousHitObject = hitObject;
            }

            // ペットボトルに当たった場合、その座標を格納
            if (hitObject != null && hitObject.CompareTag("bottle"))
            {
                bottlePosition = hitObject.transform.position;
                //UnityEngine.Debug.Log("Pet Bottle Position: " + bottlePosition);
            }
        }
        else
        {
            // レイが何にも当たらない場合
            if (previousHitObject != null)
            {
                ToggleOutline(previousHitObject, false);
                previousHitObject = null;
            }

            hitObject = null;
            bottlePosition = Vector3.zero;
        }

        // 色の更新処理
        foreach (var masterBottle in masterBottles)
        {
            var renderer = masterBottle.GetComponent<Renderer>();
            if (renderer != null)
            {
                Color newColor;

                if (hitObject != null && masterBottle == hitObject)
                {
                    // ヒットしたボトルは通常の色
                    //renderer.material.color = originalColors[masterBottle];
                    newColor = originalColors[masterBottle];
                }
                else if (hitObject != null)
                {
                    // ヒットしていないボトルはグレーアウト
                    //renderer.material.color = Color.gray;
                    newColor = Color.gray;
                }
                else
                {
                    // 全て通常の色
                    //renderer.material.color = originalColors[masterBottle];
                    newColor = originalColors[masterBottle];

                }

                /*// サブボトルに色を同期
                if (masterToSubMapping.ContainsKey(masterBottle))
                {
                    var subBottle = masterToSubMapping[masterBottle];
                    var subRenderer = subBottle.GetComponent<Renderer>();
                    if (subRenderer != null)
                    {
                        subRenderer.material.color = renderer.material.color;
                    }
                }*/

                renderer.material.SetColor("_BaseColor", newColor);
            }
        }
    }

    private void ToggleOutline(GameObject obj, bool enable)
    {
        var outline = obj.GetComponent<OutlineBehaviour>();
        if (outline != null)
        {
            outline.enabled = enable;
        }

        // サブボトルのアウトラインも同期
        if (masterToSubMapping.ContainsKey(obj))
        {
            var subBottle = masterToSubMapping[obj];
            var subOutline = subBottle.GetComponent<OutlineBehaviour>();
            if (subOutline != null)
            {
                subOutline.enabled = enable;
            }
        }
    }
}
