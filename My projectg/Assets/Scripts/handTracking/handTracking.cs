/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft;
using MixedReality.Toolkit.Input;
using MixedReality.Toolkit.Utilities;



public class handTracking : MonoBehaviour
{

    public GameObject sphereMarker;
    public GameObject maincam;

    //GameObject middleObject;
    public GameObject middleObject;


    public GameObject indexObject;

    Vector3 indexPosition;

    [SerializeField] private airTap_distance AirTap_Distance;

    [SerializeField] private GameObject origin;

    [SerializeField] private Transform handPositionFromOrigin;

    private float maxDistance = 0.7f;

    private float minDistance = 0.05f;

    private float minHight = 0.0f;
    void Start()
    {
        middleObject = Instantiate(sphereMarker, this.transform);



        thumbObject = Instantiate(sphereMarker, this.transform);
        indexObject = Instantiate(sphereMarker, this.transform);
        ringObject = Instantiate(sphereMarker, this.transform);
        pinkyObject = Instantiate(sphereMarker, this.transform);

        middleObject.SetActive(false);

    }

    void Update()
    {
        maincam.GetComponent<Renderer>();
        middleObject.GetComponent<Renderer>().enabled = false;



        thumbObject.GetComponent<Renderer>().enabled = false;
        indexObject.GetComponent<Renderer>().enabled = false;
        ringObject.GetComponent<Renderer>().enabled = false;
        pinkyObject.GetComponent<Renderer>().enabled = false;


        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.MiddleKnuckle, Handedness.Left, out MixedRealityPose middlepose))
        {
            middleObject.GetComponent<Renderer>().enabled = true;
            Quaternion rotation = GetHandRotationFromOrigin();
            middleObject.transform.position = (middlepose.Position + AirTap_Distance.middlePoint) / 3;
            middleObject.transform.rotation = middlepose.Rotation;
            handPositionFromOrigin.position = middleObject.transform.position;
        }


        //右手人差し指

        if (HandJointUtils.TryGetJointPose(TrackedHandJoint.IndexTip, Handedness.Right, out MixedRealityPose indexpose))
        {
            indexObject.SetActive(true);
            indexObject.GetComponent<Renderer>().enabled = true;
            indexObject.transform.position = indexpose.Position;
            indexObject.transform.rotation = indexpose.Rotation;

        }

    }



    public Vector3 GetThumbPosition()
    {
        //middleObject.GetComponent<Renderer>().enabled = true;
        middleObject.SetActive(true);

        float x = middleObject.transform.position.x - maincam.transform.position.x;
        float y = middleObject.transform.position.y - maincam.transform.position.y;
        float z = middleObject.transform.position.z - maincam.transform.position.z;
        Vector3 pose = new Vector3(x, y, z);

        return pose;
    }
    public Quaternion GetThumbRotation()
    {
        //middleObject.GetComponent<Renderer>().enabled = true;
        middleObject.SetActive(true);

        float x = middleObject.transform.rotation.x - maincam.transform.rotation.x;
        float y = middleObject.transform.rotation.y - maincam.transform.rotation.y;
        float z = middleObject.transform.rotation.z - maincam.transform.rotation.z;
        float w = middleObject.transform.rotation.w - maincam.transform.rotation.w;
        Quaternion rotation = new Quaternion(x, y, z, w);
        return rotation;
    }

    public Vector3 GetHandPositionFromOrigin()
    {
        middleObject.SetActive(true);

        float x = handPositionFromOrigin.localPosition.x - origin.transform.localPosition.x;
        float y = handPositionFromOrigin.localPosition.y - origin.transform.localPosition.y;
        float z = handPositionFromOrigin.localPosition.z - origin.transform.localPosition.z;

        Vector3 direction = (handPositionFromOrigin.localPosition - origin.transform.localPosition).normalized;  // 点Aから点Bへの単位ベクトル
        float distance = Vector2.Distance(new Vector2(handPositionFromOrigin.localPosition.x, handPositionFromOrigin.localPosition.z), new Vector3(origin.transform.localPosition.x, origin.transform.localPosition.z)); // 点Aと点Bの現在の距離
        Vector3 pose = new Vector3(x, y, z);

        if (y < minHight)
        {
            y = minHight;
        }

        if (distance >= maxDistance)
        {
            // 点Bを点AからmaxDistanceだけ離れた位置に調整
            pose = origin.transform.localPosition + direction * maxDistance;
        }
        else if (distance < minDistance)
        {
            // 点Bを点AからminDistanceだけ離れた位置に調整
            pose = origin.transform.localPosition + direction * minDistance;
        }
        print(pose);
        return pose;
    }

    public Quaternion GetHandRotationFromOrigin()
    {
        //middleObject.GetComponent<Renderer>().enabled = true;
        middleObject.SetActive(true);

        Vector3 direction = handPositionFromOrigin.position - origin.transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction) * origin.transform.localRotation;
        //Debug.Log(rotation);
        return rotation;
    }
}
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft;
using MixedReality.Toolkit.Input;
using MixedReality.Toolkit;
using UnityEngine.XR;
using MixedReality.Toolkit.Subsystems;



public class handTracking : MonoBehaviour
{

    public GameObject sphereMarker;
    public GameObject maincam;

    //GameObject middleObject;
    public GameObject middleObject;


    public GameObject indexObject;

    Vector3 indexPosition;

    [SerializeField] private airTap_distance AirTap_Distance;

    [SerializeField] private GameObject origin;

    [SerializeField] private Transform handPositionFromOrigin;

    private float maxDistance = 0.7f;

    private float minDistance = 0.05f;

    private float minHight = 0.0f;
    void Start()
    {
        middleObject = Instantiate(sphereMarker, this.transform);



      /*  thumbObject = Instantiate(sphereMarker, this.transform);
        indexObject = Instantiate(sphereMarker, this.transform);
        ringObject = Instantiate(sphereMarker, this.transform);
        pinkyObject = Instantiate(sphereMarker, this.transform);*/

        middleObject.SetActive(false);

    }

    void Update()
    {
        maincam.GetComponent<Renderer>();
        middleObject.GetComponent<Renderer>().enabled = false;

        // HandsAggregatorSubsystem 経由で関節情報を取得
        var aggregator = XRSubsystemHelpers.HandsAggregator;
        if (aggregator != null &&
            aggregator.TryGetJoint(TrackedHandJoint.MiddleProximal, XRNode.LeftHand, out HandJointPose middlepose))
        {
            middleObject.GetComponent<Renderer>().enabled = true;
            Quaternion rotation = GetHandRotationFromOrigin();
            //middleObject.transform.position = (middlepose.Position + AirTap_Distance.middlePoint) / 3;
            middleObject.transform.position = (middlepose.Position + AirTap_Distance.middlePoint) / 2;


            middleObject.transform.rotation = middlepose.Rotation;
            handPositionFromOrigin.position = middleObject.transform.position;
        }


    }



    public Vector3 GetThumbPosition()
    {
        //middleObject.GetComponent<Renderer>().enabled = true;
        middleObject.SetActive(true);

        float x = middleObject.transform.position.x - maincam.transform.position.x;
        float y = middleObject.transform.position.y - maincam.transform.position.y;
        float z = middleObject.transform.position.z - maincam.transform.position.z;
        Vector3 pose = new Vector3(x, y, z);

        return pose;
    }
    public Quaternion GetThumbRotation()
    {
        //middleObject.GetComponent<Renderer>().enabled = true;
        middleObject.SetActive(true);

        float x = middleObject.transform.rotation.x - maincam.transform.rotation.x;
        float y = middleObject.transform.rotation.y - maincam.transform.rotation.y;
        float z = middleObject.transform.rotation.z - maincam.transform.rotation.z;
        float w = middleObject.transform.rotation.w - maincam.transform.rotation.w;
        Quaternion rotation = new Quaternion(x, y, z, w);
        return rotation;
    }

    public Vector3 GetHandPositionFromOrigin()
    {
        middleObject.SetActive(true);

        float x = handPositionFromOrigin.localPosition.x - origin.transform.localPosition.x;
        float y = handPositionFromOrigin.localPosition.y - origin.transform.localPosition.y;
        float z = handPositionFromOrigin.localPosition.z - origin.transform.localPosition.z;

        Vector3 direction = (handPositionFromOrigin.localPosition - origin.transform.localPosition).normalized;  // 点Aから点Bへの単位ベクトル
        float distance = Vector2.Distance(new Vector2(handPositionFromOrigin.localPosition.x, handPositionFromOrigin.localPosition.z), new Vector3(origin.transform.localPosition.x, origin.transform.localPosition.z)); // 点Aと点Bの現在の距離
        Vector3 pose = new Vector3(x, y, z);

        if (y < minHight)
        {
            y = minHight;
        }

        if (distance >= maxDistance)
        {
            // 点Bを点AからmaxDistanceだけ離れた位置に調整
            pose = origin.transform.localPosition + direction * maxDistance;
        }
        else if (distance < minDistance)
        {
            // 点Bを点AからminDistanceだけ離れた位置に調整
            pose = origin.transform.localPosition + direction * minDistance;
        }
        //print(pose);
        return pose;
    }

    public Quaternion GetHandRotationFromOrigin()
    {
        //middleObject.GetComponent<Renderer>().enabled = true;
        middleObject.SetActive(true);

        Vector3 direction = handPositionFromOrigin.position - origin.transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction) * origin.transform.localRotation;
        //Debug.Log(rotation);
        return rotation;
    }
}
