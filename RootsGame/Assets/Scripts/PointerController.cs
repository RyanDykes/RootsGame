using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerController : MonoBehaviour, IPointerClickHandler
{
    public PointerEventData ActivePointerEventData = null;

    private Camera mainCam = null;
    private Plane groundPlane;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    private void Start()
    {
        groundPlane = new Plane(Vector3.up, Vector3.zero);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //-1 = Left mouse click
        if (eventData.pointerId != -1)
            return;

        Ray ray = mainCam.ScreenPointToRay(eventData.position);
        if (TreeController.Instance.IsAbilityActive)
        {
            if (groundPlane.Raycast(ray, out float hitPoint))
                TreeController.Instance.SpawnAbility(ray.GetPoint(hitPoint));
        }
    }

    Vector3 previousMousePosition = Vector3.zero;
    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
            previousMousePosition = Input.mousePosition;

        if (Input.GetMouseButton(1))
        {
            float dragAmount = (Input.mousePosition.x - previousMousePosition.x);
            CameraController.Instance.RotateCamera(dragAmount);
            previousMousePosition = Input.mousePosition;
        }
    }
}
