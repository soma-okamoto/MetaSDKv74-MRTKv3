using MixedReality.Toolkit.UX;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using MixedReality.Toolkit.SpatialManipulation;
using MixedReality.Toolkit.Input;
using Microsoft.MixedReality.OpenXR;
using static MixedReality.Toolkit.SpatialManipulation.ObjectManipulator;
using UnityEngine.XR.Interaction.Toolkit;

public class MoveRobotController : MonoBehaviour
{
    [SerializeField] private PathPublisher pathPublisher;

    [SerializeField] private GameObject YoubotObject;
    [SerializeField] private GameObject BBox;

    private bool isManipulating = false;

    [SerializeField] private GameObject WayPointObject; // ��������Waypoint�I�u�W�F�N�g
    private float generationDistance = 0.5f;

    private Vector3 lastPosition;
    private Vector3 currentPosition;
    private float movedDistance = 0.0f;

    private List<GameObject> generatedObjects = new List<GameObject>();

    [SerializeField] private GameObject lineRendererPrefab; // LineRenderer��Prefab
    private LineRenderer lineRenderer;
    private LineRenderer lineRenderer_Origin;

    [SerializeField] private GameObject WayPoints;

    [SerializeField] private GameObject Origin;
    private List<GameObject> generatedWayPointLists_Origin = new List<GameObject>();

    private bool isRobotMoveSetting = false;

    [SerializeField] private UpdateYouBotTransform updateYouBotTransform;

    private GameObject lastGeneratedWayPoint = null;
    private XRGrabInteractable bboxGrabInteractable;
/*
    private void Start()
    {
        bboxGrabInteractable = BBox.GetComponent<XRGrabInteractable>();

        bboxGrabInteractable.selectEntered.AddListener(OnManipulationStartedHandler);
        bboxGrabInteractable.selectExited.AddListener(OnManipulationEndedHandler);
    }

    private void OnDestroy()
    {
        if (bboxGrabInteractable != null)
        {
            bboxGrabInteractable.selectEntered.RemoveListener(OnManipulationStartedHandler);
            bboxGrabInteractable.selectExited.RemoveListener(OnManipulationEndedHandler);
        }
    }*/
    public void PressRobotMoveSettingButton()
    {
        isRobotMoveSetting = true;



        Vector3 direction = Camera.main.transform.position - BBox.transform.position;
        Quaternion rotation = Quaternion.LookRotation(direction);
        //BBox.transform.localRotation = rotation * Quaternion.Euler(-90, 0, 0);
        BBox.transform.localRotation = rotation * Quaternion.Euler(0, 0, 0);
        
        currentPosition = YoubotObject.transform.position;
        lastPosition = currentPosition;

        // LineRenderer��������
        GameObject lineRendererObj = Instantiate(lineRendererPrefab);
        lineRenderer = lineRendererObj.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
        //lineRenderer.SetPosition(0, currentPosition);

        // LineRenderer��������
        GameObject lineRendererObj_Origin = Instantiate(lineRendererPrefab);
        lineRenderer_Origin = lineRendererObj_Origin.GetComponent<LineRenderer>();
        lineRenderer_Origin.positionCount = 0;
        //lineRenderer_Origin.SetPosition(0, Origin.transform.position);


        //YoubotObject.SetActive(true);

        // BoundsControl �𖾎��I�ɗL��������
        var boundsControl = YoubotObject.GetComponent<BoundsControl>();
        if (boundsControl != null && !boundsControl.enabled)
        {
            Debug.Log("BoundsControl �������������̂ŗL�������܂�");
            boundsControl.enabled = true;
        }

        GenerateObject();



    }

    public void PressRobotMoveActionButton()
    {
        
        isRobotMoveSetting = false;
        if (generatedObjects[generatedObjects.Count - 1]!= null)
        {
           if ((generatedObjects[generatedObjects.Count - 1].transform.position - YoubotObject.transform.position).magnitude <= 1){
               generatedObjects.RemoveAt(generatedObjects.Count - 1);
               generatedWayPointLists_Origin.RemoveAt(generatedObjects.Count - 1);
            }
        }

        GenerateObject();
        pathPublisher.WayPointObjectList = generatedWayPointLists_Origin;
        pathPublisher.PublishStatus = true;
        updateYouBotTransform.isUpdate = true;
    }

    private void Update()
    {
        
        if (!isRobotMoveSetting) return; 

        currentPosition = YoubotObject.transform.localPosition;

        if (isManipulating)
        {
            movedDistance += Vector3.Distance(currentPosition, lastPosition);

            while (movedDistance >= generationDistance)
            {
                GenerateObject();
                movedDistance -= generationDistance;
                
            }
        }

        lastPosition = currentPosition;
    }

    public void OnManipulationStartedHandler(SelectEnterEventArgs args)
    {
        isManipulating = true;
        bboxGrabInteractable.enabled = false;
    }


    public void OnManipulationEndedHandler(SelectExitEventArgs args)
    {
        isManipulating = false;
        bboxGrabInteractable.enabled = true;
    }

    private void GenerateObject()
    {

        Vector3 spawnPosition = YoubotObject.transform.position;
        Quaternion spawnRotation = YoubotObject.transform.rotation; // �����͌��݂̌���
                                                                    // WayPoint�𐶐�
        GameObject newWayPoint = Instantiate(WayPointObject);
        newWayPoint.transform.position = spawnPosition;
        newWayPoint.transform.parent = WayPoints.transform;
        newWayPoint.transform.localRotation = Quaternion.identity;

        if (lastGeneratedWayPoint != null)
        {
            Vector3 direction = (lastGeneratedWayPoint.transform.position - spawnPosition).normalized;
            Debug.Log(direction);
            if (direction != Vector3.zero)
            {
                spawnRotation = Quaternion.LookRotation(direction, Vector3.up);
                newWayPoint.transform.rotation = spawnRotation * Quaternion.Euler(-90, -90, 0);

            }
        }



        Vector3 WaypointPositionForOrigin = newWayPoint.transform.localPosition;
        Quaternion WaypointRotationForOrigin = newWayPoint.transform.localRotation;

        GameObject newWayPoint_Origin = Instantiate(WayPointObject);
        newWayPoint_Origin.transform.parent = Origin.transform;
        newWayPoint_Origin.transform.localPosition = WaypointPositionForOrigin;
        newWayPoint_Origin.transform.localRotation = WaypointRotationForOrigin;

        lastGeneratedWayPoint = newWayPoint;

        generatedObjects.Add(newWayPoint);
        generatedWayPointLists_Origin.Add(newWayPoint_Origin);
        // LineRenderer�Ƀ|�C���g��ǉ�
        AddLinePoint(YoubotObject.transform.position, newWayPoint_Origin.transform.position);

        /*// �V�R�[�h�iXRGrabInteractable�ɐ؂�ւ��j
        var interactable = newWayPoint.GetComponent<XRGrabInteractable>();
        interactable.selectExited.AddListener(EditRouteObject);

        var interactableOrigin = newWayPoint_Origin.GetComponent<XRGrabInteractable>();
        interactableOrigin.selectExited.AddListener(EditRouteObject);*/

        /*newWayPoint.GetComponent<XRGrabInteractable>().selectExited.AddListener(EditRouteObject);
        newWayPoint_Origin.GetComponent<XRGrabInteractable>().selectExited.AddListener(EditRouteObject);*/
        newWayPoint.GetComponent<ObjectManipulator>().selectExited.AddListener(EditRouteObject);
        newWayPoint_Origin.GetComponent<ObjectManipulator>().selectExited.AddListener(EditRouteObject);

    }

    public void EditRouteObject(SelectExitEventArgs args)
    {
         GameObject currentlyManipulatedObject = args.interactableObject.transform.gameObject;

        int index = generatedWayPointLists_Origin.IndexOf(currentlyManipulatedObject);

        if(index >= 0)
        {
            Vector3 WaypointPositionForOrigin = currentlyManipulatedObject.transform.localPosition;
            generatedObjects[index].transform.localPosition = WaypointPositionForOrigin;
            generatedObjects[index].transform.localRotation = currentlyManipulatedObject.transform.localRotation;
            UpdateLinePoint(index, generatedObjects[index].transform.position, currentlyManipulatedObject.transform.position);
        }

        index = generatedObjects.IndexOf(currentlyManipulatedObject);

        if (index >= 0)
        {
            Vector3 WaypointPositionForOrigin = currentlyManipulatedObject.transform.localPosition;
            generatedWayPointLists_Origin[index].transform.localPosition = WaypointPositionForOrigin;
            generatedWayPointLists_Origin[index].transform.localRotation = currentlyManipulatedObject.transform.localRotation;
            UpdateLinePoint(index, currentlyManipulatedObject.transform.position, generatedWayPointLists_Origin[index].transform.position);
        }
    }

    private void AddLinePoint(Vector3 position,Vector3 positionForOrigin)
    {
        lineRenderer.positionCount += 1;
        lineRenderer_Origin.positionCount += 1;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, position);
        lineRenderer_Origin.SetPosition(lineRenderer_Origin.positionCount - 1, positionForOrigin);
    }

    public void UpdateLinePoint(int PositionCount,Vector3 UpdatePos,Vector3 UpdatePosForOrigin)
    {
        lineRenderer.SetPosition(PositionCount, UpdatePos);
        lineRenderer_Origin.SetPosition(PositionCount, UpdatePosForOrigin);
    }
}
