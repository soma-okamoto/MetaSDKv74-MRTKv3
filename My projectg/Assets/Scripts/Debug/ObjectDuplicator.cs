using MixedReality.Toolkit.SpatialManipulation;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ObjectDuplicator : MonoBehaviour
{
    [Tooltip("�������������̃I�u�W�F�N�g")]
    public GameObject objectToDuplicate;

    [Tooltip("�������Ɉʒu���ǂꂾ�����炷��")]
    public Vector3 offset = new Vector3(1, 0, 0);

    [Tooltip("�������ɂǂꂾ����]�����邩 (Euler �p�x)")]
    public Vector3 rotationEuler = Vector3.zero;

    [Tooltip("�������̃X�P�[��")]
    public Vector3 scale = Vector3.one;
    // youbot���ꎞ�I�ɔ�A�N�e�B�u�ɂ������X�g
    private List<GameObject> duplicatedYoubots = new List<GameObject>();

    public void DuplicateObject()
    {
        GameObject duplicatedObject = null; // �� �����Ő錾���Ă���

        if (objectToDuplicate != null)
        {
            // Rotation�͌��̉�] + �w���]
            Quaternion newRotation = objectToDuplicate.transform.rotation * Quaternion.Euler(rotationEuler);

            // �������ĐV�����ʒu�E��]�Ő���
            duplicatedObject = Instantiate(
                objectToDuplicate,
                objectToDuplicate.transform.position + offset,
                newRotation
            );

            duplicatedObject.name = objectToDuplicate.name + "_Copy";
            // ObjectManipulator ��L����
            var manipulator = duplicatedObject.GetComponent<ObjectManipulator>();
            if (manipulator != null)
            {
                manipulator.enabled = true;
            }
            // Scale�͎w�肵�����̂�K�p�i���̃X�P�[�����g�������Ȃ� objectToDuplicate.transform.localScale �ɕύX�j
            duplicatedObject.transform.localScale = scale;

            // PointCloudRenderer �� subscriber �̐ݒ���R�s�[
            var originalRenderer = objectToDuplicate.GetComponentInChildren<PointCloudRenderer>();
            var originalSubscriber = originalRenderer?.subscriber;

            var duplicatedRenderer = duplicatedObject.GetComponentInChildren<PointCloudRenderer>();
            if (duplicatedRenderer != null && originalSubscriber != null)
            {
                duplicatedRenderer.subscriber = originalSubscriber;
            }
            DeactivateWaypointsInHierarchy(duplicatedObject);

            
            HandleYoubotActivation(objectToDuplicate, duplicatedObject);
        }
        else
        {
            UnityEngine.Debug.LogWarning("objectToDuplicate���ݒ肳��Ă��܂���I");
        }
        // �����Ŏg����悤�ɂȂ�
        if (duplicatedObject != null)
        {
            DeactivateWaypointsInHierarchy(duplicatedObject);
        }
        SetTopMostFirstActive_OthersInactive();
        ReactivateYoubots();
    }
    

    void SetTopMostFirstActive_OthersInactive()
    {
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        List<GameObject> matches = new List<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.name == "BoundingBoxWithTraditionalHandles(Clone)" && obj.hideFlags == HideFlags.None)
            {
                matches.Add(obj);
            }
        }

        if (matches.Count == 0)
        {
            UnityEngine.Debug.LogWarning("BoundingBoxWithTraditionalHandles(Clone) ��������܂���ł����I");
            return;
        }

        // �K�w���󂢏��Ƀ\�[�g���A�����[���Ȃ猩���������i=���̂܂܂̏��j
        matches.Sort((a, b) =>
        {
            int depthA = GetHierarchyDepth(a.transform);
            int depthB = GetHierarchyDepth(b.transform);
            return depthA.CompareTo(depthB); // depth ���󂢂قǐ��
        });

        // �ŏ���1���A�N�e�B�u�A����ȊO���A�N�e�B�u
        bool found = false;
        foreach (var obj in matches)
        {
            if (!found)
            {
                obj.SetActive(true);
                found = true;
                UnityEngine.Debug.Log($"�A�N�e�B�u��: {obj.name} ({GetHierarchyPath(obj.transform)})");
            }
            else
            {
                obj.SetActive(false);
            }
        }
    }

    int GetHierarchyDepth(Transform t)
    {
        int depth = 0;
        while (t.parent != null)
        {
            depth++;
            t = t.parent;
        }
        return depth;
    }

    string GetHierarchyPath(Transform t)
    {
        List<string> path = new List<string>();
        while (t != null)
        {
            path.Insert(0, t.name);
            t = t.parent;
        }
        return string.Join("/", path);
    }
    void DeactivateWaypointsInHierarchy(GameObject root)
    {
        Transform[] children = root.GetComponentsInChildren<Transform>(true); // ��A�N�e�B�u�Ȏq���܂߂Ď擾
        foreach (Transform child in children)
        {
            if (child.name == "WayPoints")
            {
                child.gameObject.SetActive(false);
                UnityEngine.Debug.Log($"Waypoints���A�N�e�B�u��: {GetHierarchyPath(child)}");
            }
        }
    }
    void HandleYoubotActivation(GameObject original, GameObject duplicated)
    {
        // ���I�u�W�F�N�g����youbot��T��
        var originalYoubot = FindYoubotInHierarchy(original);
        var duplicatedYoubot = FindYoubotInHierarchy(duplicated);

        if (originalYoubot != null && duplicatedYoubot != null)
        {
            if (originalYoubot.activeSelf)
            {
                duplicatedYoubot.SetActive(false); // �ꎞ�I�ɖ�����
                duplicatedYoubots.Add(duplicatedYoubot); // ��ŗL�������邽�߂ɕۑ�
                UnityEngine.Debug.Log($"������� youbot ���A�N�e�B�u��: {GetHierarchyPath(duplicatedYoubot.transform)}");
            }
        }
    }

    GameObject FindYoubotInHierarchy(GameObject root)
    {
        Transform[] children = root.GetComponentsInChildren<Transform>(true);
        foreach (Transform t in children)
        {
            if (t.name == "youbot")
            {
                return t.gameObject;
            }
        }
        return null;
    }

    void ReactivateYoubots()
    {
        foreach (var youbot in duplicatedYoubots)
        {
            if (youbot != null)
            {
                youbot.SetActive(true);
                UnityEngine.Debug.Log($"youBot�ăA�N�e�B�u��: {GetHierarchyPath(youbot.transform)}");
            }
        }
        duplicatedYoubots.Clear();
    }


}
