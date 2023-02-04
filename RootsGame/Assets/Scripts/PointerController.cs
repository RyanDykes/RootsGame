using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerController : MonoBehaviour
{
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

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (TreeController.Instance.IsAbilityActive)
            {
                Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
                if (groundPlane.Raycast(ray, out float hitPoint))
                    TreeController.Instance.SpawnAbility(ray.GetPoint(hitPoint));
            }
        }
    }
}
