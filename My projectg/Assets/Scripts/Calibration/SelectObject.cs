﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectObject : MonoBehaviour
{
    [SerializeField] RosSharp.RosBridgeClient.AfterObjectFloat32Publisher afterObjectFloat32;
    [SerializeField] RosSharp.RosBridgeClient.BeforeObjectFloat32Publisher beforeObjectFloat32;
    [SerializeField] RosSharp.RosBridgeClient.CalibrationFloat32Publisher calibrationFloat32Publisher;
    [SerializeField] CalibrationFloat32Subscriber calibrationFloat32Subscriber;
    public List<GameObject> ObjectList;
    public List<float> beforeList;
    public List<float> afterList;
    public float[] beforeData;
    public float[] afterData;
    [SerializeField] private GameObject Origin;


    [ColorUsage(false, true)] public Color CalibrationColor;
    [ColorUsage(false, true)] public Color SelectColor;
    [ColorUsage(false, true)] public Color PickColor;
    [ColorUsage(false, true)] public Color DefalutColor;

    bool active = true;

    [SerializeField] private ObjectGenerationTest objectGeneration;
    public List<string> NotSelectObjectList;

    public bool CalibrationMode;
    public bool MoveMode;

    public List<GameObject> PickObjectList;
    public List<float> PickObjectPositionList;

    void Start()
    {
        NotSelectObjectList = objectGeneration.ObjectNameList;
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.tag == "bottle")
        {
            if (CalibrationMode == true)
            {
                if (!ObjectList.Contains(other.gameObject))
                {
                    other.gameObject.GetComponent<MeshRenderer>().material.color = SelectColor;
                    beforeList.Add(other.gameObject.transform.localPosition.x);
                    beforeList.Add(other.gameObject.transform.localPosition.y);
                    beforeList.Add(other.gameObject.transform.localPosition.z);
                    ObjectList.Add(other.gameObject);
                    NotSelectObjectList.Remove(other.gameObject.name);
                }
            }

            if(MoveMode == true)
            {
                if (!PickObjectList.Contains(other.gameObject))
                {
                    other.gameObject.GetComponent<MeshRenderer>().material.color = PickColor;
                    PickObjectPositionList.Add(other.gameObject.transform.localPosition.x);
                    PickObjectPositionList.Add(other.gameObject.transform.localPosition.y);
                    PickObjectPositionList.Add(other.gameObject.transform.localPosition.z);
                    PickObjectList.Add(other.gameObject);
                }
            }
        }
    }
    public void CalibrationButton()
    {
        afterObjectFloat32.enabled = true;
        beforeObjectFloat32.enabled = true;
        calibrationFloat32Publisher.enabled = true;
        calibrationFloat32Subscriber.enabled = true;
        foreach (var name in NotSelectObjectList)
        {
            for (int j = 0; j < objectGeneration.before_obj.Count; j++)
            {
                if (objectGeneration.before_obj[j].name == name)
                {
                    objectGeneration.before_obj[j].gameObject.GetComponent<MeshRenderer>().material.color = CalibrationColor;
                }
            }
        }
    }

    public void PickModeButton()
    {
        CalibrationMode = false;
        MoveMode = !MoveMode;
    }

    private void ChangeObjectColor()
    {
        foreach (GameObject obj in objectGeneration.GenerateObjects)
        {
            obj.gameObject.GetComponent<MeshRenderer>().material.color = DefalutColor;
        }
    }

    public float[] AfterObjectMessage ()
    {
        if(active)
        {
            foreach (var obj in ObjectList)
            {
                afterList.Add(obj.transform.localPosition.x);
                afterList.Add(obj.transform.localPosition.y);
                afterList.Add(obj.transform.localPosition.z);
            }
            active = false;
        }

        beforeData = new float[] { beforeList[0], beforeList[1], beforeList[2], beforeList[3], beforeList[4], beforeList[5], beforeList[6], beforeList[7], beforeList[8], beforeList[9], beforeList[10], beforeList[11] };
        afterData = new float[] { afterList[0], afterList[1], afterList[2], afterList[3], afterList[4], afterList[5], afterList[6], afterList[7], afterList[8], afterList[9], afterList[10], afterList[11] };

        return afterData;
    }

    public float[] BeforeObjectMessage()
    {
        beforeData = new float[] { beforeList[0], beforeList[1], beforeList[2], beforeList[3], beforeList[4], beforeList[5], beforeList[6], beforeList[7], beforeList[8], beforeList[9], beforeList[10], beforeList[11] };

        return beforeData;
    }

    public float[] PickObjectMessage()
    {
        beforeData = PickObjectPositionList.ToArray();

        return beforeData;
    }
}
