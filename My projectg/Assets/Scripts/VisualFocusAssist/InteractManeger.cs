
using UnityEngine;
//using UnityFx.Outline;
using System.Collections.Generic;


public class InteractManager : MonoBehaviour
{/*
    public OutlineOnView _rayManager;
    private GameObject currentTarget;
    public BottleSync bottleSync; // �� Inspector �ŃA�T�C��

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

        // �}�X�^�[��
        var outline = obj.GetComponent<OutlineBehaviour>();
        if (outline != null) outline.enabled = false;
        obj.layer = LayerMask.NameToLayer("Default");

        // �T�u���ɂ����f
        if (bottleSync != null)
        {
            GameObject sub;
            if (bottleSync.TryGetSubFromMaster(obj, out sub))
            {
                var subOutline = sub.GetComponent<OutlineBehaviour>();
                if (subOutline != null) subOutline.enabled = false;
                sub.layer = LayerMask.NameToLayer("Default");
            }
        }
    }

    void EnableOutline(GameObject obj)
    {
        if (obj == null) return;

        // �}�X�^�[��
        var outline = obj.GetComponent<OutlineBehaviour>();
        if (outline != null) outline.enabled = true;
        obj.layer = LayerMask.NameToLayer("bottole");

        // �T�u���ɂ����f
        if (bottleSync != null)
        {
            GameObject sub;
            if (bottleSync.TryGetSubFromMaster(obj, out sub))
            {
                var subOutline = sub.GetComponent<OutlineBehaviour>();
                if (subOutline != null) subOutline.enabled = true;
                sub.layer = LayerMask.NameToLayer("bottole");
            }
        }
    }*/
}

