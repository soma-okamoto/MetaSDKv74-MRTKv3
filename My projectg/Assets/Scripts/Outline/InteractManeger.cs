
using UnityEngine;
using UnityFx.Outline;

public class InteractManager : MonoBehaviour
{
    public OutlineOnView _rayManager;
    private GameObject currentTarget;

    void Update()
    {
        GameObject hitObj = _rayManager.hitObject;

        if (hitObj != currentTarget)
        {
            ResetOutline(currentTarget);
            currentTarget = hitObj;
            EnableOutline(currentTarget);
        }
    }

    void ResetOutline(GameObject obj)
    {
        if (obj == null) return;

        var outline = obj.GetComponent<OutlineBehaviour>();
        if (outline != null) outline.enabled = false;

        obj.layer = LayerMask.NameToLayer("Default");
    }

    void EnableOutline(GameObject obj)
    {
        if (obj == null) return;

        var outline = obj.GetComponent<OutlineBehaviour>();
        if (outline != null) outline.enabled = true;

        obj.layer = LayerMask.NameToLayer("bottole"); // Å© typoíçà”ÅF"bottle"ÅH
    }
}
