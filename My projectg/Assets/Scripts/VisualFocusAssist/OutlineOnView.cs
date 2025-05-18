
using UnityEngine;
using MixedReality.Toolkit.SpatialManipulation;
using System.Collections.Generic;
using UnityEngine;
using UnityFx.Outline;
using MixedReality.Toolkit.SpatialManipulation;
using MixedReality.Toolkit.Input;

public class OutlineOnView : MonoBehaviour
{
    public Camera playerCamera;
    [SerializeField]private float maxRaycastDistance = 0.75f;
    private RaycastHit hitObj;

    [SerializeField, Tooltip("掴んだときに表示するオブジェクト")]
    private GameObject targetUI;  // Inspector で表示対象のUIを指定
    public BottleSync bottleSync; // ← Inspector でアサイン


    public GameObject hitObject { get; private set; }  // 他スクリプトから取得可能に

    private GameObject GetCurrentlyGrabbedObject()
    {
        var manipulators = FindObjectsOfType<ObjectManipulator>();
        foreach (var manipulator in manipulators)
        {
            if (manipulator.interactorsSelecting != null && manipulator.interactorsSelecting.Count > 0)
                return manipulator.gameObject;
        }
        return null;
    }

    void Update()
    {
        GameObject grabbed = GetCurrentlyGrabbedObject();
        GameObject raycasted = null;

        if (Physics.Raycast(new Ray(playerCamera.transform.position, playerCamera.transform.forward), out hitObj, maxRaycastDistance))
        {
            raycasted = hitObj.collider.gameObject;
        }


        



        if (grabbed != null && grabbed.CompareTag("bottle"))
        {
            hitObject = grabbed;

            //  掴んでいるときは UI 表示
            if (targetUI != null && !targetUI.activeSelf)
            {
                targetUI.SetActive(true);
            }
        }
        else
        {
            if (targetUI != null && targetUI.activeSelf)
            {
                targetUI.SetActive(false);
            }

            if (raycasted != null && raycasted.CompareTag("bottle"))
            {
                hitObject = raycasted;
            }
            else
            {
                hitObject = null;
            }
        }

        if (hitObject != null && bottleSync != null)
        {
            bottleSync.SetCurrentHitObject(hitObject);
        }

    }
    public GameObject GetTargetUI()
    {
        return targetUI;
    }


}
