using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerController : MonoBehaviour
{
    private Camera mainCam = null;
    private Ray ray;
    private Plane groundPlane;
    private int layer_mask;

    private void Awake()
    {
        mainCam = Camera.main;
        layer_mask = LayerMask.GetMask("UI");
    }

    private void Start()
    {
        groundPlane = new Plane(Vector3.up, Vector3.zero);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ray = mainCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, layer_mask))
                return;

            if (TreeController.Instance.IsAbilityActive)
            {
                if (groundPlane.Raycast(ray, out float hitPoint))
                    TreeController.Instance.SpawnAbility(ray.GetPoint(hitPoint));
            }
        }
    }
}
