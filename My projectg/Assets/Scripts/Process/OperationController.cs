using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using MixedReality.Toolkit.UX;
using MixedReality.Toolkit.Input; // ←追加（必要なら）
using UnityEngine.XR.Interaction.Toolkit;
using MixedReality.Toolkit.SpatialManipulation;


public class OperationController : MonoBehaviour
{
    [SerializeField] private GameObject rosConnector;
    //s[SerializeField] private GameObject mark;
    [SerializeField] private ObjectGenerationTest objectGenerationTest;
    [SerializeField] private TextMeshPro OperationUI;
    [SerializeField] private GameObject BBox;
    public void operationButton()
    {
        //rosConnector.GetComponent<RosSharp.RosBridgeClient.BaseMovePublisher>().enabled = false;
        //mark.SetActive(true);
        OperationUI.text = "Start Operation";
        Invoke("operation", 3f);
        objectGenerationTest.enabled = false;
    }

    public void operation()
    {
        rosConnector.GetComponent<RosSharp.RosBridgeClient.handPosePublisher>().enabled = true;
        rosConnector.GetComponent<RosSharp.RosBridgeClient.airTapPublisher>().enabled = true;
        rosConnector.GetComponent<Float32MultiSubscriber>().enabled = true;
        this.gameObject.SetActive(false);

        BBox.GetComponent<BoundsControl>().enabled = false;
        BBox.GetComponent<BoxCollider>().enabled = false;
        BBox.GetComponent<ObjectManipulator>().enabled = false;
    }
}
