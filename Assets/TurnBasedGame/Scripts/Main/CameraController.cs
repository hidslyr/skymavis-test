using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace TurnBaseGame
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] CinemachineVirtualCamera orthoCamera;
        [SerializeField] Transform orthoCameraPivot;
        [SerializeField] CinemachineVirtualCamera isometricCamera;
        [SerializeField] InputManager inputManager;

        [Range(1f, 10.0f)]
        [SerializeField] float dragSensitivity;
        [Range(1f, 10.0f)]
        [SerializeField] float dragRoughness;

        [Range(100f, 1000f)]
        [SerializeField] float zoomSensitivity;
        [Range(1f, 10.0f)]
        [SerializeField] float zoomRoughness;

        private bool enableInputProcess = false;
        private float targetOrthoSize;
        private Vector3 targetPosition;

        void Start()
        {
            inputManager.OnFingerDragMove += DragCamera;
            inputManager.OnMouseScrollWheelMoved += ZoomCamera;
            targetPosition = orthoCameraPivot.position;
        }

        public void EnableCameraControl(bool enabled)
        {
            enableInputProcess = enabled;
        }

        public void SwitchCamera()
        {
            isometricCamera.enabled = !isometricCamera.enabled;
            orthoCamera.enabled = !orthoCamera.enabled;
        }

        private void DragCamera(int touchIndex, Vector2 touchPos, Vector2 delta)
        {
            if (!enableInputProcess || !orthoCamera.enabled)
            {
                return;
            }

            Vector3 changeInPosition = 
                new Vector3(-delta.x * Time.deltaTime * dragSensitivity, 
                0, 
                -delta.y * Time.deltaTime * dragSensitivity);

            targetPosition += changeInPosition;
        }

        private void ZoomCamera(float axis)
        {
            if (!enableInputProcess || !orthoCamera.enabled)
            {
                return;
            }

            // Scroll data returned as 0.1 if scroll up or -0.1 if scroll down
            float signed = axis * 100f; 
            targetPosition += Vector3.up * signed * Time.deltaTime * zoomSensitivity;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                SwitchCamera();
            }

            orthoCameraPivot.position = Vector3.Lerp(
                orthoCamera.transform.position,
                targetPosition,
                Time.deltaTime * dragRoughness);
        }
    }

}
