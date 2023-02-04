using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SkillTreeController : MonoBehaviour
{
    public static SkillTreeController Instance = null;

    [SerializeField] private GameObject abilityGroup = null;
    [SerializeField] private GameObject closeSKillTreeButton = null;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        abilityGroup.SetActive(true);
        closeSKillTreeButton.SetActive(false);
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
