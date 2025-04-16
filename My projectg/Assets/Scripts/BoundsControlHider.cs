using UnityEngine;
using UnityEngine.Events;
using MixedReality.Toolkit.Input;
using MixedReality.Toolkit.UX;
using Microsoft.MixedReality.OpenXR;
using MixedReality.Toolkit.SpatialManipulation;
using UnityEngine.XR.Interaction.Toolkit;


public class BoundsControlHider : MonoBehaviour
{
    private ObjectManipulator manipulator;

    void Awake()
    {
        manipulator = GetComponent<ObjectManipulator>();

        if (manipulator != null)
        {
            manipulator.firstSelectEntered.AddListener(OnGrabStarted);
            manipulator.lastSelectExited.AddListener(OnGrabEnded);
           
        }
        else
        {
            Debug.LogWarning("ObjectManipulator が見つかりません！");
        }
    }

    private void OnGrabStarted(SelectEnterEventArgs args)
    {
        SetAllBoundsHandlesActive(false);
        Debug.Log("掴んだので全BoundsControlのHandlesを非表示にしました");
    }

    private void OnGrabEnded(SelectExitEventArgs args)
    {
        SetAllBoundsHandlesActive(true);
        Debug.Log("離したので全BoundsControlのHandlesを再表示しました");
    }

    private void SetAllBoundsHandlesActive(bool active)
    {
        // MRTK3では BoundsControlRuntime が使われている可能性がある
        var boundsControls = FindObjectsOfType<BoundsControl>(true);
        foreach (var bounds in boundsControls)
        {
            bounds.HandlesActive = active;
        }
    }
}
