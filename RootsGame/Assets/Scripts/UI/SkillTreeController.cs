using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillTreeController : MonoBehaviour
{
    public static SkillTreeController Instance = null;

    [SerializeField] private GameObject abilityGroup = null;
    [SerializeField] private GameObject closeSKillTreeButton = null;

    private Transform mainCam = null;

    private void Awake()
    {
        Instance = this;
        mainCam = Camera.main.transform;
    }

    private void Start()
    {
        abilityGroup.SetActive(true);
        closeSKillTreeButton.SetActive(false);
    }

    private void Update()
    {
        Vector3 cameraPosition = mainCam.position;
        cameraPosition.y = 0f;
        transform.rotation = Quaternion.LookRotation(cameraPosition - transform.position);
    }

    private void OnDestroy()
    {
        Instance = null;
    }

    public void OpenSkillTree()
    {
        abilityGroup.SetActive(false);
        closeSKillTreeButton.SetActive(true);
        CameraController.Instance.TransitionToSkillTree();
    }

    public void CloseSkillTree()
    {
        abilityGroup.SetActive(true);
        closeSKillTreeButton.SetActive(false);
        CameraController.Instance.TransitionToSurfaceTree();
    }
}
