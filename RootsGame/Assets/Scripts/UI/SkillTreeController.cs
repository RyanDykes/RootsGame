using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillTreeController : MonoBehaviour
{
    public static SkillTreeController Instance = null;

    private void Awake()
    {
        Instance = this;
    }

    private void OnDestroy()
    {
        Instance = null;
    }
}
