using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace TurnBaseGame
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] Camera orthoCamera;
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
        private Vector3 targetOrthoPosition;

        void Start()
        {
            inputManager.OnFingerDragMove += DragCamera;
            inputManager.OnMouseScrollWheelMoved += ZoomCamera;
            targetOrthoSize = orthoCamera.orthographicSize;
            targetOrthoPosition = orthoCamera.transform.position;
        }

        public void EnableCameraControl(bool enabled)
        {
            enableInputProcess = enabled;
        }

        private void DragCamera(int touchIndex, Vector2 touchPos, Vector2 delta)
        {
            if (!enableInputProcess)
            {
                return;
            }

            Vector3 changeInPosition = 
                new Vector3(-delta.x * Time.deltaTime * dragSensitivity, 
                0, 
                -delta.y * Time.deltaTime * dragSensitivity);

            targetOrthoPosition += changeInPosition;
        }

        private void ZoomCamera(float up)
        {
            targetOrthoSize += up * Time.deltaTime * zoomSensitivity;
        }

        private void Update()
        {
            orthoCamera.orthographicSize = 
                Mathf.Lerp(orthoCamera.orthographicSize, 
                targetOrthoSize, 
                Time.deltaTime * zoomRoughness);

            orthoCamera.transform.position = Vector3.Lerp(
                orthoCamera.transform.position,
                targetOrthoPosition,
                Time.deltaTime * dragRoughness);
        }
    }

}
