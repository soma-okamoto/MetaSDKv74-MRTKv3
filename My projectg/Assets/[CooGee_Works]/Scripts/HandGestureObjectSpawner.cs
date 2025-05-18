using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class HandGestureObjectSpawner : MonoBehaviour
{
    public InputActionReference middlePressedAction; // Meta Aim Hand��middlePressed�A�N�V����
    public InputActionReference handPositionAction; // ��̈ʒu���g���b�L���O���邽�߂̃A�N�V����
    public GameObject objectToSpawn; // �X�|�[������I�u�W�F�N�g
    public float speedThreshold = 1.0f; // ���x�̂������l

    private Vector3 previousPosition;
    private bool isMiddlePressed;
    private bool isHandTrackingInitialized = false;

    void OnEnable()
    {
        middlePressedAction.action.performed += OnMiddlePressed;
        middlePressedAction.action.canceled += OnMiddleReleased;
        handPositionAction.action.Enable();
    }

    void OnDisable()
    {
        middlePressedAction.action.performed -= OnMiddlePressed;
        middlePressedAction.action.canceled -= OnMiddleReleased;
        handPositionAction.action.Disable();
    }


    void Update()
    {
        if (!isHandTrackingInitialized)
        {
            // ��̏����ʒu���擾
            previousPosition = handPositionAction.action.ReadValue<Vector3>();
            isHandTrackingInitialized = true;
            return;
        }

        Vector3 currentPosition = handPositionAction.action.ReadValue<Vector3>();
        Vector3 velocity = (currentPosition - previousPosition) / Time.deltaTime;

        if (isMiddlePressed && velocity.y < -speedThreshold)
        {
            SpawnObject();
        }

        else if (isMiddlePressed && velocity.y > speedThreshold)
        {
            DeactivateObject();
        }

        previousPosition = currentPosition;
    }

    private void OnMiddlePressed(InputAction.CallbackContext context)
    {
        isMiddlePressed = true;
    }

    private void OnMiddleReleased(InputAction.CallbackContext context)
    {
        isMiddlePressed = false;
    }

    private void SpawnObject()
    {
        if (objectToSpawn != null)
        {
            Animator animator = objectToSpawn.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetBool("ShowUI", true);
            }
        }
    }

    private void DeactivateObject()
    {
        if (objectToSpawn != null)
        {
            Animator animator = objectToSpawn.GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetBool("ShowUI", false);
            }
        }
    }
}
