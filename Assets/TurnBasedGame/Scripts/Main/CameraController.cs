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

        [Range(1f, 10f)]
        [SerializeField] float zoomSensitivity;
        [SerializeField] float orthoVcamMinY;

        private bool enableInputProcess = false;
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

            if (axis == 0)
            {
                return;
            }

            float signed = axis < 0 ? -1 : 1;
            targetPosition += Vector3.up * signed * Time.deltaTime * zoomSensitivity * 1000f;

            targetPosition.y = Mathf.Clamp(targetPosition.y, orthoVcamMinY, targetPosition.y);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                SwitchCamera();
            }

            orthoCameraPivot.position = targetPosition;
        }
    }
}
