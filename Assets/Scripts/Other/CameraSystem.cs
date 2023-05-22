using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour {
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;

    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float rotateSpeed = 100f;
    [SerializeField] private float zoomSpeed = 10f;

    [SerializeField] private float followOffsetMin = 5f;
    [SerializeField] private float followOffsetMax = 50f;
    [SerializeField] private float followOffsetMinY = 1f;
    [SerializeField] private float followOffsetMaxY = 50f;
    [SerializeField] private float followOffsetMaxZ = -5;
    [SerializeField] private float followOffsetMinZ = -15;


    [SerializeField] private bool keyMovementEnabled = false;
    [SerializeField] private bool edgeScrollingEnabled = true;




    private Vector3 followOffset;

    private void Awake() {
        followOffset = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
    }
    void Update() {
        if (keyMovementEnabled) {
            HandleMovement_WASD();
            HandleRotation();
        }
        if (edgeScrollingEnabled) {
            HandleMovement_EdgeScrolling();
        }
        //HandleZoom_ScrollWheel();
        HandleZoom_Y();
    }

    private void HandleMovement_WASD() {
        Vector3 inputDir = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W)) {
            inputDir.z = +1f;
        }
        if (Input.GetKey(KeyCode.S)) {
            inputDir.z = -1f;
        }
        if (Input.GetKey(KeyCode.A)) {
            inputDir.x = -1f;
        }
        if (Input.GetKey(KeyCode.D)) {
            inputDir.x = +1f;
        }
        Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    private void HandleMovement_EdgeScrolling() {
        Vector3 inputDir = new Vector3(0, 0, 0);
        int edgeScrollSize = 20;
        if (Input.mousePosition.x < edgeScrollSize) {
            inputDir.x = -1f;
        }
        if (Input.mousePosition.x > Screen.width - edgeScrollSize) {
            inputDir.x = +1f;
        }
        if (Input.mousePosition.y < edgeScrollSize) {
            inputDir.z = -1f;
        }
        if (Input.mousePosition.y > Screen.height - edgeScrollSize) {
            inputDir.z = +1f;
        }
        Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;
        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    private void HandleRotation() {
        float rotateDir = 0f;
        if (Input.GetKey(KeyCode.Q)) {
            rotateDir = -1;
        }
        if (Input.GetKey(KeyCode.E)) {
            rotateDir = +1;
        }
        transform.eulerAngles += new Vector3(0, rotateDir * rotateSpeed * Time.deltaTime, 0);
    }

    private void HandleZoom_Move() {
        float zoomAmount = 3f;
        Vector3 zoomDir = followOffset.normalized;
        if (Input.mouseScrollDelta.y > 0) {
            followOffset -= zoomDir * zoomAmount;
        }
        if (Input.mouseScrollDelta.y < 0) {
            followOffset += zoomDir * zoomAmount;
        }
        if (followOffset.magnitude < followOffsetMin) {
            followOffset = followOffsetMin * zoomDir;
        }
        if (followOffset.magnitude > followOffsetMax) {
            followOffset = followOffsetMax * zoomDir;
        }
        cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset =
            Vector3.Lerp(cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset, followOffset, Time.deltaTime * zoomSpeed);
    }

    private void HandleZoom_Y() {
        float zoomAmount = 6f;
        if (Input.mouseScrollDelta.y > 0) {
            followOffset.y -= zoomAmount;
        }
        if (Input.mouseScrollDelta.y < 0) {
            followOffset.y += zoomAmount;
        }

        followOffset.z = Mathf.Clamp(followOffset.z, followOffsetMinZ, followOffsetMaxZ);
        followOffset.y = Mathf.Clamp(followOffset.y, followOffsetMinY, followOffsetMaxY);
        cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset =
            Vector3.Lerp(cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset, followOffset, Time.deltaTime * zoomSpeed);
    }
}
