using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance = null;
    [SerializeField] private Transform cameraPivot = null;
    [SerializeField] private float cameraSpeed = 10f;

    private void Awake()
    {
        Instance = this;
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public void RotateCamera(float dragAmount)
    {
        cameraPivot.RotateAround(Vector3.zero, Vector3.up, dragAmount * cameraSpeed * Time.deltaTime);
    }
}
