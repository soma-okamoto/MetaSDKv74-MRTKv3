using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RosSharp.RosBridgeClient.MessageTypes.Nav;
using RosSharp.RosBridgeClient;

public class SuggestionGenerateRoute : MonoBehaviour
{
    [SerializeField] private GameObject RouteObject; // �o�H�������I�u�W�F�N�g�̃v���n�u
    private List<GameObject> instantiatedObjects = new List<GameObject>(); // ���������I�u�W�F�N�g���Ǘ�

    [SerializeField] private GameObject InitializeYouBotPosition;

    [SerializeField] private SuggestionRouteSubscriber suggestionRouteSubscriber;

    private Path SuggestionRoutePath;

    [SerializeField] private GameObject lineRendererPrefab; // LineRenderer��Prefab
    private LineRenderer lineRenderer;

    [SerializeField] private GameObject SuggestionMessageObject;
    private float distanceFromCamera = 2.0f;

    private bool Suggestioned = false;
    private void Start()
    {
        GameObject lineRendererObj = Instantiate(lineRendererPrefab);
        lineRenderer = lineRendererObj.GetComponent<LineRenderer>();
        lineRenderer.positionCount = 0;
    }
    private void Update()
    {
        if(suggestionRouteSubscriber.messagePath!= null && suggestionRouteSubscriber.messagePath.poses.Length > 0)
        {
            if(SuggestionMessageObject.activeSelf == false && !Suggestioned)
            {
                Suggestioned = true;
                Camera mainCamera = Camera.main;
                if (mainCamera != null)
                {
                    // �J�����̑O���ɃI�u�W�F�N�g��z�u
                    Vector3 positionInFrontOfCamera = mainCamera.transform.position + mainCamera.transform.forward * distanceFromCamera;
                    SuggestionMessageObject.transform.position = positionInFrontOfCamera;

                    // �J�����̌����ɍ��킹�ĉ�]��ݒ�
                    SuggestionMessageObject.transform.rotation = Quaternion.LookRotation(mainCamera.transform.forward);

                    // �I�u�W�F�N�g��L����
                    SuggestionMessageObject.SetActive(true);
                }
                else
                {
                    Debug.LogWarning("Main Camera ��������܂���I");
                }

                SuggestionRoutePath = suggestionRouteSubscriber.messagePath;
                ClearExistingRouteObjects();
                GenerateRouteObjects(SuggestionRoutePath);
            }


        }
    }
    // �o�H��̊e�ʒu�ɃI�u�W�F�N�g�𐶐�
    private void GenerateRouteObjects(Path path)
    {
        foreach (var poseStamped in path.poses)
        {
            Debug.Log(poseStamped.pose.position);
            Vector3 position = new Vector3(
                (float)-poseStamped.pose.position.x,
                (float)InitializeYouBotPosition.transform.position.y,
                (float)-poseStamped.pose.position.y
            );

            Quaternion rotation = new Quaternion(
                (float)poseStamped.pose.orientation.z,
                (float)-poseStamped.pose.orientation.x,
                (float)poseStamped.pose.orientation.y,
                (float)-poseStamped.pose.orientation.w
            );

            GameObject routeObject = Instantiate(RouteObject);
            routeObject.transform.parent = InitializeYouBotPosition.transform;
            routeObject.transform.localPosition = new Vector3(position.x,0.147f,position.z);
            routeObject.transform.localRotation = Quaternion.identity;
            //routeObject.transform.Rotate(transform.up, 90);

            instantiatedObjects.Add(routeObject); // ���������I�u�W�F�N�g�����X�g�ɒǉ�

            AddLinePoint(instantiatedObjects[instantiatedObjects.Count - 1].transform.position,routeObject.transform.position);
        }
    }

    // �����̃I�u�W�F�N�g���N���A
    private void ClearExistingRouteObjects()
    {
        foreach (var obj in instantiatedObjects)
        {
            Destroy(obj);
        }
        instantiatedObjects.Clear();
        lineRenderer.positionCount = 0;
    }

    private void AddLinePoint(Vector3 position, Vector3 positionForOrigin)
    {
        lineRenderer.positionCount += 1;
        lineRenderer.SetPosition(lineRenderer.positionCount - 1, position);
    }
}
