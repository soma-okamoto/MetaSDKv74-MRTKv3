
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
        }
        else if (raycasted != null && raycasted.CompareTag("bottle"))
        {
            hitObject = raycasted;
        }
        else
        {
            hitObject = null; 
        }
    }

}
